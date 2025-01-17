﻿namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using JYW.ThesisMMO.Common.Codes;

    internal class IntModifier : Modifier {

        private ModifyMode m_Mode;
        private int m_Value;

        public IntModifier(ModifyMode mode, AttributeCode attribute, int value) {
            m_Attribute = attribute;
            m_Mode = mode;
            m_Value = value;
        }

        public override void ApplyEffect(Entity entity) {
            var attribute = entity.GetAttribute(m_Attribute) as IntAttribute;
            if(attribute == null) { return; }
            attribute.SetValue(m_Mode, m_Value);
        }
    }
}
