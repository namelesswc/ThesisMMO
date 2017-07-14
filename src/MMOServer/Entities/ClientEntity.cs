﻿using ExitGames.Concurrency.Fibers;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Peers;

    internal class ClientEntity : Entity {

        //public Region CurrentWorldRegion { get; private set; }

        public override IFiber Fiber { get { return Peer.RequestFiber; } }

        private const float InterestRadius = 10f;
        private static readonly SendParameters DefaultSendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };

        public ClientEntity(string name, Vector position, Attribute[] attributes, MMOPeer peer) : base(name, position, attributes, peer) {
        }

        protected override void SetInterestArea() {
            m_InterestArea = new ClientInterestArea(this, InterestRadius);
        }

        //public override void Move(Vector position) {
        //    base.Move(position);
        //    UpdateInterestManagment();
        //}

        //public override void OnAddedToWorld() {
        //    UpdateInterestManagment();
        //}

        //private void UpdateInterestManagment() {
        //    var newRegion = World.Instance.GetRegionFromPoint(Position);
        //    if(CurrentWorldRegion != newRegion) {
        //        ChangeRegion(CurrentWorldRegion, newRegion);
        //    }

        //    m_InterestArea.UpdateRegionSubscription();
        //}



        /// <summary> 
        /// Use this method to update the peer.
        /// </summary>
        public override SendResult SendEvent(IEventData eventData, SendParameters sendParameters) {
            return Peer.SendEvent(eventData, sendParameters);
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// Default sendparameters are used. Reliable. Channel 0
        /// </summary>
        public override SendResult SendEvent(IEventData eventData) {
            return SendEvent(eventData, DefaultSendParameters);
        }

        public override IEventData GetEntitySnapshot() {
            var newPlayerEv = new NewPlayerEvent() {
                Name = Name,
                Position = Position,
                CurrentHealth = ((IntAttribute)GetAttribute(AttributeCode.Health)).GetValue(),
                MaxHealth = ((IntAttribute)GetAttribute(AttributeCode.MaxHealth)).GetValue()
            };
            return new EventData((byte)EventCode.NewPlayer, newPlayerEv);
        }
    }
}