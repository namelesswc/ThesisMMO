﻿namespace JYW.ThesisMMO.MMOServer.Requests {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    internal class EnterWorldRequest : Operation {

        public EnterWorldRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Name { get; set; }

        [DataMember(Code = (byte)ParameterCode.Team)]
        public string Team { get; set; }

        [DataMember(Code = (byte)ParameterCode.Weapon)]
        public int Weapon { get; set; }

        [DataMember(Code = (byte)ParameterCode.ActionCode)]
        public int[] Skills { get; set; }
    }
}
