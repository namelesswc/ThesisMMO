﻿using JYW.ThesisMMO.MMOServer.CSAIM;

namespace JYW.ThesisMMO.MMOServer.Skills {
    class OrisonOfHealingData : SkillData {
        public override int CooldownInMs {
            get {
                return 6000;
            }
        }

        public override float MaxRange {
            get {
                return 8F;
            }
        }

        public override MsInInterval GetConsistencyRequirement() {
            if (m_ConsistencyOnCooldown) {
                return MsInInterval.Zero;
            }
            return new MsInInterval(MaxRange-1F, MaxRange+1F, 0, SkillTarget.FriendOnly);
        }
    }
}
