﻿namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public sealed partial class EventOperations {

        /// <summary>  
        ///  Called whenever a new player enters the world.
        /// </summary>  
        public static Action<string, Vector2> NewPlayerEvent;

        private static void OnNewPlayerEvent(EventData eventData) {
            // TODO: Extend this event with more data later.
            var username = (string)eventData.Parameters[(byte)ParameterCode.Username];
            var vecPos = (Vector)eventData.Parameters[(byte)ParameterCode.Position];
            var vec2Pos = new Vector2(vecPos.X, vecPos.Y);

            if (NewPlayerEvent != null) {
                NewPlayerEvent(username, vec2Pos);
            }
            else {
                Debug.Log("No new player listener");
            }
        }
    }
}