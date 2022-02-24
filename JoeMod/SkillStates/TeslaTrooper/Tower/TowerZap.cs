﻿using HenryMod.ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;                                 
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper.Tower {

    public class TowerZap: BaseTimedSkillState
    {
        public static float DamageCoefficient = 2.2f;
        public static float ProcCoefficient = 1f;

        public static float BaseDuration = 1f;
        public static float BaseStartCastTime = 1;

        public LightningOrb lightningOrb;
        public HurtBox lightningTarget;

        public static string PrepSound = "Play_prism_bpripow_prism_tower_prep";
        public static string ZapSound = "Play_tower_btesat1a_tesla_tower_attack";
        public static string ZapSoundCrit = "Play_tower_btesat1b_tesla_tower_attack";

        protected string zapSound = ZapSound;
        protected string zapSoundCrit = ZapSoundCrit;

        //protected float ownerDamage {
            
        //    get {
        //        float damage = 1;

        //        CharacterBody body = GetComponent<GenericOwnership>()?.ownerObject.GetComponent<CharacterBody>();
        //        if (body) {
        //            damage = body.damage;
        //        }

        //        return damage;
        //    }
        //}

        public override void OnEnter()
        {
            base.OnEnter();

            // is this redundant cause the cast time is the end and I could just do an onexit kinda thing?
            InitDurationValues(BaseDuration, BaseStartCastTime);

            lightningOrb = new LightningOrb {
                origin = base.GetModelChildLocator().FindChild("Orb").position,
                damageValue = DamageCoefficient * damageStat,
                isCrit = RollCrit(),
                //bouncesRemaining = 1,
                //damageCoefficientPerBounce = BounceDamageMultplier,
                //damageType = DamageType.SlowOnHit,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = 1f,
                bouncedObjects = new List<HealthComponent>(),
                lightningType = LightningOrb.LightningType.Loader,
                damageColorIndex = DamageColorIndex.Default,
                //range = SearchRange,
                canBounceOnSameTarget = true,
                speed = 1000
            };

            lightningOrb.target = lightningTarget;
            
            Util.PlaySound(PrepSound, gameObject);
            PlayAnimation("weapon", "PrepZap", "prep.playbackRate", base.duration);
            //playanimation or however I'm going to do the glow going up the pole
                //and the orb glowing
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (lightningTarget == null && !hasFired) {
                TotallyOriginalTrackerComponent tracker = GetComponent<TotallyOriginalTrackerComponent>();
                if (tracker) {
                    lightningTarget = tracker.GetTrackingTarget();
                }
            }
        }

        protected override void OnCastEnter() {
            //todo: custom lightningorb
            if (lightningTarget == null)
                return;

            fireOrb();

            string sound = ZapSound;
            if (lightningOrb.isCrit) sound = ZapSoundCrit;

            Util.PlaySound(sound, gameObject);

            //PlaySoundAuthority(sound);
            //playsound zap
        }

        protected virtual void fireSound() {

        }

        protected virtual void fireOrb() {

            //todo: custom lightningorb
            for (int i = 0; i < 3; i++) {

                OrbManager.instance.AddOrb(lightningOrb);
            }
        }
    }

}
