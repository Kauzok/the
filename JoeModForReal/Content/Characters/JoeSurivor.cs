﻿using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using ModdedEntityStates.Joe;
using UnityEngine;
using Modules.Characters;
using Modules.Survivors;
using Modules;
using JoeModForReal.Components;
using System.Runtime.CompilerServices;

namespace JoeModForReal.Content.Survivors {
    internal class JoeSurivor : SurvivorBase
    {
        public override string bodyName => "Joe";

        public const string JOE_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_";

        public override string survivorTokenPrefix => JOE_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyPrefabName = "JoeBody",
            bodyNameToken = JOE_PREFIX + "NAME",
            subtitleNameToken = JOE_PREFIX + "SUBTITLE",
            sortPosition = 69,

            characterPortrait = Modules.Assets.LoadCharacterIcon("joe_icon"),
            bodyColor = Color.magenta,

            crosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 2f,
            armor = 20f,

            jumpCount = 2,
            
            aimOriginPosition = new Vector3(0, 1.3f, 0),
            cameraParamsDepth = -10,
        
            cameraParamsVerticalOffset = 1.0f,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(ModdedEntityStates.Joe.JoeMain);

        public override ItemDisplaysBase itemDisplays => new JoeItemDisplays();

        public override ConfigEntry<bool> characterEnabledConfig => null;
        
        public override UnlockableDef characterUnlockableDef { get; }
        private static UnlockableDef masterySkinUnlockableDef;

        public static float DashArmor => TestValueManager.dashArmor;// 140;

        public static float TenticlesArmor => TestValueManager.tenticleArmor;
        public static float TenticleMoveSpeedAddition => TestValueManager.tenticleMove;
        public static float TenticleAttackSpeed = 0.2f;

        public static float TenticleBuffHealMultiplier = 0.03f;
        public static float TenticleMaxHealthMultiplier = 0.66f;

        public override void Initialize() {
            base.Initialize();

            Hook();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            bodyPrefab.AddComponent<JoeWeaponComponent>();
        }

        protected override void InitializeCharacterModel() {
            base.InitializeCharacterModel();
        }

        public override void InitializeDoppelganger(string clone) {
            base.InitializeDoppelganger("Loader");
        }

        //you ready for some stupid shit?

        public override void InitializeUnlockables()
        {
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        }

        public override void InitializeHitboxes()
        {
            //hitboxes already set up baybee
            return;

            //ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            //GameObject model = childLocator.gameObject;
            
            //Modules.Prefabs.SetupHitbox(model, "Sword1", childLocator.FindChild("hitboxSwing1"));
            //Modules.Prefabs.SetupHitbox(model, "Sword2", childLocator.FindChild("hitboxSwing2"));
            //Modules.Prefabs.SetupHitbox(model, "SwordJump", childLocator.FindChild("hitboxJumpSwing"));
        }

        public override void InitializeSkills() {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();

            if (Compat.ScepterInstalled) {
                InitializeScepterSkills();
            }
        }

        private void InitializePrimarySkills() {

            LookingDownSkillDef primarySkillDef = Modules.Skills.CreateSkillDef<LookingDownSkillDef>(
                new SkillDefInfo("JoeSwing",
                                 JOE_PREFIX + "PRIMARY_SWING_NAME",
                                 JOE_PREFIX + "PRIMARY_SWING_DESCRIPTION",
                                 Modules.Assets.LoadAsset<Sprite>("texIconPrimary"),
                                 new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.Primary1Swing)),
                                 "Weapon",
                                 true));
            primarySkillDef.stepCount = 2;
            primarySkillDef.stepGraceDuration = 1.2f;

            primarySkillDef.ConditionalIcon = Modules.Assets.LoadAsset<Sprite>("texIconPrimaryJumpSwing");
            primarySkillDef.ConditionalState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.Primary1JumpSwingFall));
            primarySkillDef.ConditionalRequriedStock = 0;
            primarySkillDef.LookingDownAngle = 42;

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);

            LookingDownSkillDef primarySkillDefSilly = Modules.Skills.CreateSkillDef<LookingDownSkillDef>(
                new SkillDefInfo("JoeSwingClassic",
                                 JOE_PREFIX + "PRIMARY_SWING_NAME_CLASSIC",
                                 JOE_PREFIX + "PRIMARY_SWING_DESCRIPTION",
                                 Modules.Assets.LoadAsset<Sprite>("texIconPrimary"),
                                 new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.PrimaryStupidSwing)),
                                 "Weapon",
                                 true) {
                    mustKeyPress = true
                });
            primarySkillDefSilly.stepCount = 2;
            primarySkillDefSilly.stepGraceDuration = 1;

            primarySkillDefSilly.ConditionalIcon = Modules.Assets.LoadAsset<Sprite>("texIconPrimaryJumpSwing");
            primarySkillDefSilly.ConditionalState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.Primary1JumpSwingFall));
            primarySkillDefSilly.ConditionalRequriedStock = 0;
            primarySkillDefSilly.LookingDownAngle = 42;


            CombinedSteppedSkillDef primarySkillDefKoal = Modules.Skills.CreateSkillDef<CombinedSteppedSkillDef>(
                new SkillDefInfo("koalswing",
                                 JOE_PREFIX + "PRIMARY_KOAL_NAME",
                                 JOE_PREFIX + "PRIMARY_KOAL_DESCRIPTION",
                                 Modules.Assets.LoadAsset<Sprite>("texIconPrimaryJumpSwing"),
                                 new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.KoalCombo)),
                                 "Weapon",
                                 true));
            primarySkillDefKoal.mustKeyPress = true;
            primarySkillDefKoal.stepCount = 4;
            primarySkillDefKoal.stepGraceDuration = 2f;
            primarySkillDefKoal.maxCombinedStepCount = 8;

            if (Modules.Config.Cursed) {
                Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefSilly, primarySkillDefKoal);
            }

            #region dev
            SkillDef primarySkillDefBomeb = Modules.Skills.CreateSkillDef(new SkillDefInfo("joeBomb",
                                                                     JOE_PREFIX + "PRIMARY_BOMB_NAME",
                                                                     JOE_PREFIX + "PRIMARY_BOMB_DESCRIPTION",
                                                                     null, //Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                     new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.ThrowBoom)),
                                                                     "Weapon",
                                                                     false));

            SkillDef primarySkillDefBomebe = Modules.Skills.CreateSkillDef(new SkillDefInfo("joeBomb2",
                                                                      JOE_PREFIX + "PRIMARY_BOMB_NAME",
                                                                      JOE_PREFIX + "PRIMARY_BOMB_DESCRIPTION",
                                                                      null, //Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                      new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.ThroBoomButCoolerQuestionMaark)),
                                                                      "Weapon",
                                                                      false));


            if (FacelessJoePlugin.andrew) {
                Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefBomeb, primarySkillDefBomebe);
            }
            #endregion dev
        }

        private void InitializeSecondarySkills() {

            SkillDef fireballSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = JOE_PREFIX + "SECONDARY_FIREBALL_NAME",
                skillNameToken = JOE_PREFIX + "SECONDARY_FIREBALL_NAME",
                skillDescriptionToken = JOE_PREFIX + "SECONDARY_FIREBALL_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconSecondary"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.Secondary1Fireball)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, fireballSkillDef);

            RepeatableSteppedSkillDef secondarySkillDefKoal = Modules.Skills.CreateSkillDef<RepeatableSteppedSkillDef>(new SkillDefInfo {
                skillName = JOE_PREFIX + "SECONDARY_KOAL_NAME",
                skillNameToken = JOE_PREFIX + "SECONDARY_KOAL_NAME",
                skillDescriptionToken = JOE_PREFIX + "SECONDARY_KOAL_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconPrimaryJumpSwing"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.KoalCombo2)),
                activationStateMachineName = "Slide",
                baseMaxStock = 2,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });
            secondarySkillDefKoal.stocksToConsumeAfterAllUses = 1;
            secondarySkillDefKoal.stepCount = 4;
            secondarySkillDefKoal.maxCombinedStepCount = 8;
            secondarySkillDefKoal.stepGraceDuration = 2f;


            if (Modules.Config.Cursed) {
                Modules.Skills.AddSecondarySkills(bodyPrefab, secondarySkillDefKoal);
            }
        }

        private void InitializeUtilitySkills() {

            SkillDef dashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = JOE_PREFIX + "UTILITY_DASH_NAME",
                skillNameToken = JOE_PREFIX + "UTILITY_DASH_NAME",
                skillDescriptionToken = JOE_PREFIX + "UTILITY_DASH_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Utility1Dash)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, dashSkillDef);
        }

        private void InitializeSpecialSkills() {

            SkillDef tenticlesSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = JOE_PREFIX + "SPECIAL_TENTICLES_NAME",
                skillNameToken = JOE_PREFIX + "SPECIAL_TENTICLES_NAME",
                skillDescriptionToken = JOE_PREFIX + "SPECIAL_TENTICLES_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Special1Tenticles)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { JOE_PREFIX + "KEYWORD_TENTICLES" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, tenticlesSkillDef);
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            LookingDownSkillDef primarySkillDef = Modules.Skills.CreateSkillDef<LookingDownSkillDef>(
                new SkillDefInfo("JoeSwing",
                                 JOE_PREFIX + "PRIMARY_SWING_NAME",
                                 JOE_PREFIX + "PRIMARY_SWING_DESCRIPTION",
                                 Modules.Assets.LoadAsset<Sprite>("texIconScepterPrimary"),
                                 new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.PrimaryScepter1Swing)),
                                 "Weapon",
                                 true));
            primarySkillDef.stepCount = 2;
            primarySkillDef.stepGraceDuration = 1.2f;

            primarySkillDef.ConditionalIcon = Modules.Assets.LoadAsset<Sprite>("texIconScepterPrimaryJumpSwing");
            primarySkillDef.ConditionalState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.PrimaryScepter1JumpSwingFall));
            primarySkillDef.ConditionalRequriedStock = 0;
            primarySkillDef.LookingDownAngle = 42;

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(primarySkillDef, "JoeBody", SkillSlot.Primary, 0);
            if (Config.Cursed) {
                AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(primarySkillDef, "JoeBody", SkillSlot.Primary, 1);
            }
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();
            
            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                Assets.LoadAsset<Sprite>("texiconSkinDefault"),
                defaultRenderers,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
            };

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            //Material masteryMat = Modules.Assets.CreateMaterial("matHenryAlt");
            //CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            //{
            //    masteryMat,
            //    masteryMat,
            //    masteryMat,
            //    masteryMat
            //});

            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.developerPrefix + "_HENRY_BODY_MASTERY_SKIN_NAME",
            //    Assets.LoadAsset<Sprite>("texMasteryAchievement"),
            //    masteryRendererInfos,
            //    mainRenderer,
            //    model,
            //    masterySkinUnlockableDef);

            //masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            //{
            //    //new SkinDef.MeshReplacement
            //    //{
            //    //    mesh = Modules.Assets.LoadAsset<Mesh>("meshHenrySwordAlt"),
            //    //    renderer = defaultRenderers[0].renderer
            //    //},
            //    //new SkinDef.MeshReplacement
            //    //{
            //    //    mesh = Modules.Assets.LoadAsset<Mesh>("meshHenryAlt"),
            //    //    renderer = defaultRenderers[instance.mainRendererIndex].renderer
            //    //}
            //};

            //skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();
        }


        private void Hook() {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {

            if (sender.HasBuff(Buffs.TenticleBuff)) {
                args.moveSpeedMultAdd += TenticleMoveSpeedAddition * sender.GetBuffCount(Buffs.TenticleBuff);
                args.attackSpeedMultAdd += TenticleAttackSpeed * sender.GetBuffCount(Buffs.TenticleBuff);
                args.armorAdd += TenticlesArmor/* * sender.GetBuffCount(Buffs.TenticleBuff)*/;

                args.jumpPowerMultAdd += 0.5f;
            }

            if (sender.HasBuff(Buffs.DashArmorBuff)) {
                args.armorAdd += DashArmor;
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            orig(self, damageInfo);

            if (R2API.DamageAPI.HasModdedDamageType(damageInfo, Modules.DamageTypes.TenticleLifeStealing)) {

                CharacterBody characterBody = damageInfo.attacker?.GetComponent<CharacterBody>();
                if (characterBody && characterBody.HasBuff(Buffs.TenticleBuff)) {

                    float healthLimit = characterBody.maxHealth * TenticleMaxHealthMultiplier;

                    float healAmount = Mathf.Min(characterBody.maxHealth * TenticleBuffHealMultiplier, healthLimit - characterBody.healthComponent.health);
                    characterBody.healthComponent.Heal(healAmount, default(ProcChainMask));
                }
            }
        }
    }
}