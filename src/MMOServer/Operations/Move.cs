﻿namespace JYW.ThesisMMO.MMOServer.Operations {
    using Common.Types;
    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    class Move : Operation{
        public Move(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }
    }
}