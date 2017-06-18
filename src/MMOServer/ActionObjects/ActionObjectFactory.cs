﻿using System;
using Photon.SocketServer;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using JYW.ThesisMMO.Common.Codes;

    /// <summary> 
    /// Creates ActionObjects.
    /// </summary>
    internal class ActionObjectFactory {

        private const string skillNamespace = "JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests.";

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public ReturnCode LastCreationFailReason { get; private set; }

        public ActionObject CreateActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request) {
            var action = (ActionCode)request.Parameters[(byte)ParameterCode.ActionCode];

            var stringType = skillNamespace + action + "Request";
            var actionType = Type.GetType(stringType);

            if(actionType == null) {
                log.DebugFormat("Type {0} was not found.", stringType);
                LastCreationFailReason = ReturnCode.OperationNotImplemented;
                return null;
            }

            return Activator.CreateInstance(actionType, actionSource, protocol, request) as ActionObject;
        }
    }
}
