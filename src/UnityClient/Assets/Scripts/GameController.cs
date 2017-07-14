﻿using UnityEngine;

namespace JYW.ThesisMMO.UnityClient {

    using Core.MessageHandling.Events;

    public sealed class GameController : MonoBehaviour {
        
        private void Start() {
            EventOperations.FireEnqueuedEvents();
        }
    }
}