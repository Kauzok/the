﻿using System;
using RoR2;
using RoR2.Skills;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using R2API;
using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;
using EntityStates;
using UnityEngine;
using ModdedEntityStates.Desolator;

namespace Modules {

    public class SkillsPlusCompat {

        public static int SkillsPlusAdditionalTowers;

        public static void init() {

            SkillModifierManager.LoadSkillModifiers();
        }

        [SkillLevelModifier("Tesla_Primary_Zap", typeof(Zap))]
        public class TeslaPrimaryModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);
                if (skillState is Zap zapState) {
                    zapState.skillsPlusCasts = Mathf.FloorToInt(AdditiveScaling(0.0f, 0.5f, level));
                }
            }
        }

        [SkillLevelModifier("Tesla_Secondary_BigZap", typeof(AimBigZap), typeof(BigZap), typeof(TowerBigZap))]
        class TeslaSecondaryModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);

                //Helpers.LogWarning("running on " + skillState.GetType().ToString());

                if (skillState is AimBigZap aimBigZapState) {

                    aimBigZapState.skillsPlusMulti = MultScaling(1, .1f, level);

                } else if (skillState is BigZap bigZapState) {

                    bigZapState.skillsPlusAreaMulti = MultScaling(1, .1f, level);
                    bigZapState.skillsPlusDamageMulti = MultScaling(1f, 0.1f, level);

                } else if (skillState is TowerBigZap towerBigZapState) {

                    towerBigZapState.secondarySkillsPlusAreaMulti = MultScaling(1, .1f, level);
                    towerBigZapState.secondarySkillsPlusDamageMulti = MultScaling(1f, 0.05f, level);
                }
            }
        }

        [SkillLevelModifier("Tesla_Utility_ShieldZap", typeof(ShieldZapCollectDamage))]
        public class TeslaUtilityModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);
                if (skillState is ShieldZapCollectDamage shieldState) {
                    shieldState.skillsPlusSeconds = AdditiveScaling(0f, 0.5f, level);
                }
            }
        }

        [SkillLevelModifier("Tesla_Special_Tower", new Type[0])]//, typeof(DeployTeslaTower), typeof(TowerLifetime))]
        public class TeslaSpecialModifier : BaseSkillModifier {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = (int)AdditiveScaling(1, 0.334f, level);
                SkillsPlusAdditionalTowers = (int)AdditiveScaling(0, 0.334f, level);
                TowerLifetime.skillsPlusSeconds = AdditiveScaling(0, 1, level);
            }
        }

        //are scepter skills even getting upgraded?
        [SkillLevelModifier("Tesla_Special_Scepter_Tower", new Type[0])]
        public class TeslaSpecialScepterModifier : BaseSkillModifier {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = (int)AdditiveScaling(2, 0.334f, level);
                SkillsPlusAdditionalTowers = (int)AdditiveScaling(0, 0.334f, level);
                TowerLifetime.skillsPlusSeconds = AdditiveScaling(0, 1, level);
            }
        }


        [SkillLevelModifier("Desolator_Primary_Beam", typeof(RadBeam))]
        public class DesolatorPrimaryModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);
                if (skillState is RadBeam radBeam) {
                    radBeam.skillsPlusDurationMultiplier = MultScaling(1f, -0.1f, level);
                }
            }
        }
    }
}