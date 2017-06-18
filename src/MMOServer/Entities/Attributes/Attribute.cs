﻿using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.Common.Codes;

    internal abstract class Attribute {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected Entity m_Entity;

        internal protected AttributeCode AttributeCode { get; protected set; }

        internal Attribute(AttributeCode attributeCode) {
            AttributeCode = attributeCode;
        }

        internal void SetEntity(Entity entity) {
            m_Entity = entity;
        }
    }
}
