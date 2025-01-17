﻿using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using Common.Codes;

    public static partial class EventOperations {

        /// <summary>  
        ///  If event listeners are not setup, incoming events are queued here. Fire them with FireEnqueuedEvents().
        /// </summary>
        private readonly static Queue<Action> EventQueue = new Queue<Action>();

        /// <summary>  
        ///  If event listeners are not setup, incoming events are queued. Fire them with this method.
        /// </summary>
        public static void FireEnqueuedEvents() {
            while (EventQueue.Count > 0) {
                EventQueue.Dequeue()();
            }
        }

        // TODO: Merge some event codes e.g. new player event and new entity event.
        public static void OnEvent(EventData eventData) {
            switch ((EventCode)eventData.Code) {
                case EventCode.ActionStateUpdate:
                    OnActionStateUpdateEvent(eventData);
                    return;
                case EventCode.AttributeChangedEvent:
                    AttributeUpdateEvent(eventData);
                    return;
                case EventCode.HealthUpdate:
                    OnHealthUpdateEvent(eventData);
                    return;
                case EventCode.EntityDeath:
                    OnEntityDeathEvent(eventData);
                    return;
                case EventCode.EntityExitRegion:
                    OnEntityExitRegionEvent(eventData);
                    return;
                case EventCode.FrequencyTable:
                    OnFrequencyTableEvent(eventData);
                    return;
                case EventCode.FilteredPosition:
                    OnFilteredMoveEvent(eventData);
                    return;
                case EventCode.Move:
                    OnMoveEvent(eventData);
                    return;
                case EventCode.NewEntity:
                    OnNewEntityEvent(eventData);
                    return;
                case EventCode.NewPlayer:
                    OnNewPlayerEvent(eventData);
                    return;
            }
            Debug.LogErrorFormat("Unimplemented event {0}.", (EventCode)eventData.Code);
        }

        private static void AttributeUpdateEvent(EventData eventData) {
            var attribute = (AttributeCode)eventData.Parameters[(byte)ParameterCode.AttributeCode];

            switch (attribute) {
                case AttributeCode.Speed:
                    OnSpeedUpdate(eventData);
                    return;
            }
            Debug.LogErrorFormat("Unimplemented attribute {0} update.", attribute);
        }
    }
}
