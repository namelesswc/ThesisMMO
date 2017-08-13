﻿namespace JYW.ThesisMMO.MMOServer.Skills {
    class HammerBashData : SkillData {
        public override int CooldownInMs {
            get {
                return 5000;
            }
        }

        public override float MaxRange {
            get {
                return 3.25F;
            }
        }
    }
}