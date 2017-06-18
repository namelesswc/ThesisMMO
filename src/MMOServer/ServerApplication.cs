﻿namespace JYW.ThesisMMO.MMOServer {

    using Common.Types;
    using Photon.SocketServer;
    using Peers;
    using Protocol = Common.Types.Protocol;
    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;
    using log4net.Config;
    using System.IO;
    using AI;
    using System;
    using System.Reflection;

    /// <summary> 
    /// Main class of the server.
    /// </summary>
    sealed class ServerApplication : ApplicationBase {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private World m_World;
        private AILooper m_AIModule;

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(initRequest);
        }

        protected override void Setup() {
            SetupLogger();
            RegisterTypes();
            CreateWorld();
            CreateTestBots();
        }
        protected override void TearDown() {
            AILooper.Instance.Dispose();
            log.DebugFormat("------------------------Tear Down------------------------");
        }

        private void SetupLogger() {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            var configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists) {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            log.DebugFormat("------------------------Server Started------------------------");
        }

        private static void RegisterTypes() {
            Photon.SocketServer.Protocol.TryRegisterCustomType(
                typeof(Vector),
                (byte)Protocol.CustomTypeCodes.Vector,
                Protocol.SerializeVector,
                Protocol.DeserializeVector);
        }

        private void CreateWorld() {
            m_World = new World();
            log.DebugFormat("Created Game World.");
        }

        private void CreateTestBots() {
            EntityFactory.Instance.CreateAIBot("One Punch Man", new Vector(2, 2));
            EntityFactory.Instance.CreateAIBot("Ork 234932", new Vector(0, -2));
            EntityFactory.Instance.CreateAIBot("Ork 452537", new Vector(-3, 4));
        }
    }
}
