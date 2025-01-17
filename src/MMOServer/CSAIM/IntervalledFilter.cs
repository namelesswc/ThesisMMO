﻿using System.Collections.Generic;
using System.Threading;
using ExitGames.Logging;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.CSAIM {

    using Common.Codes;
    using Common.Types;
    using Entities;
    using Events;
    using Events.Demo;
    using Properties;
    using Skills;

    internal class IntervalledFilter : PositionFilter {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly List<MsInInterval> m_MsInIntervals = new List<MsInInterval>();
        private readonly Dictionary<Entity, long> m_PositionTimestamps = new Dictionary<Entity, long>();

        private readonly object m_QueueLock = new object();
        private readonly List<Entity> m_QueuedPositionUpdates = new List<Entity>();
        private readonly Thread m_DequeueThread;
        private readonly List<Entity> m_RemoveList = new List<Entity>();
        private bool m_Dequeueing = true;
        private const int DequeueIntervalInMs = 30;

        public IntervalledFilter(ClientEntity entity) 
            : base (entity) {

            UpdateIntervals(m_AttachedEntity.EquippedSkills.GetSkillConsistencyRequirements());
            entity.EquippedSkills.SkillConsistencyUpdateEvent += UpdateIntervals;
            m_DequeueThread = new Thread(DequeueTask);
            m_DequeueThread.Start();
        }

        public void UpdateIntervals(IEnumerable<MsInInterval> consistencies) {
            m_MsInIntervals.Clear();
            AddDefaultIntervals();
            AddSkillIntervals(consistencies);
            SendIntervalsToClient();
        }

        private void DequeueTask() {
            while (m_Dequeueing) {
                lock (m_QueueLock) {
                    foreach (var entity in m_QueuedPositionUpdates) {
                        var curTime = GameTime.TimeMs;
                        if (m_PositionTimestamps[entity] + GetUpdateInterval(entity) <= curTime) {
                            UpdateClientPosition(entity);
                            m_RemoveList.Add(entity);
                        }
                        else if (Settings.Default.EvaluationMode) {
                            SendFilteredPosition(entity);
                        }
                    }
                    while (m_RemoveList.Count > 0) {
                        m_QueuedPositionUpdates.Remove(m_RemoveList[0]);
                        m_RemoveList.RemoveAt(0);
                    }
                }
                Thread.Sleep(DequeueIntervalInMs);
            }
        }

        public override void OnPositionUpdate(Entity entity) {
            lock (m_QueueLock) {
                // If no information about past update to this entity.
                if (!m_PositionTimestamps.ContainsKey(entity)) {
                    UpdateClientPosition(entity);
                    return;
                }

                if (!m_QueuedPositionUpdates.Contains(entity)) {
                    m_QueuedPositionUpdates.Add(entity);
                }
                //if (Settings.Default.EvaluationMode) {
                //    SendFilteredPosition(entity);
                //}
            }
        }

        private int GetUpdateInterval(Entity entity) {
            var targetFaction = entity.Team == m_AttachedEntity.Team ? SkillTarget.FriendOnly : SkillTarget.FoeOnly;
            var distance = Vector.Distance(entity.Position, m_AttachedEntity.Position);
            var interval = int.MaxValue;
            foreach (var msInIntervals in m_MsInIntervals) {
                if (!msInIntervals.IsInInterval(distance, targetFaction)) { continue; }
                if (msInIntervals.MilliSeconds > interval) { continue; }
                interval = msInIntervals.MilliSeconds;
            }
            return interval;
        }

        private long TimeSinceLastUpdate(Entity entity) {
            if (!m_PositionTimestamps.ContainsKey(entity)) { return -1L; }
            return GameTime.TimeMs - m_PositionTimestamps[entity];
        }

        private void AddDefaultIntervals() {
            m_MsInIntervals.Add(new MsInInterval(0F, 50F, 500));
            //m_MsInIntervals.Add(new MsInInterval(0F, 10F, 200));
        }

        private void AddSkillIntervals(IEnumerable<MsInInterval> consistencies) {
            m_MsInIntervals.AddRange(consistencies);
        }

        protected override void UpdateClientPosition(Entity entity) {
            base.UpdateClientPosition(entity);
            AddTimeStamp(entity);
        }

        protected void SendFilteredPosition(Entity entity) {
            EventMessage.CounterEventReceive.Increment();
            var moveEvent = new MoveEvent(entity.Name, entity.Position);
            IEventData eventData = new EventData((byte)EventCode.FilteredPosition, moveEvent);
            m_AttachedEntity.SendEvent(eventData, PositionSendParameters);
        }

        private void AddTimeStamp(Entity entity) {
            m_PositionTimestamps[entity] = GameTime.TimeMs;
        }

        // For demo only
        private void SendIntervalsToClient() {
            var ev = new IntervalTableEvent(m_MsInIntervals).ToEventData();
            m_AttachedEntity.SendEvent(ev);
        }
    }
}
