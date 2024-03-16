﻿using RoR2;
using RA2Mod.Modules.Achievements;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    //automatically creates language tokens $"ACHIEVMENT_{identifier.ToUpper()}_NAME" and $"ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class TeslaMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TESLA_PREFIX + "MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TESLA_PREFIX + "MASTERYUNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => TeslaTrooperSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}