﻿using EntityStates;
using R2API;
using RoR2;

namespace RA2Mod.Survivors.Desolator.States
{
    public class RadBeam : GenericBulletBaseState {

        public static float BaseDuration = 1.0f;
        public static float DamageCoefficient = 1.0f;

        public static int RadStacks = 2;
        public static float RadDamageMultiplier = 0.7f;

        public float skillsPlusDurationMultiplier = 1;

        public override void OnEnter() {

            EntityStates.Toolbot.FireSpear goodstate = new EntityStates.Toolbot.FireSpear();

            baseDuration = BaseDuration * skillsPlusDurationMultiplier;
            bulletCount = 1;
            maxDistance = goodstate.maxDistance;
            bulletRadius = goodstate.bulletRadius;
            useSmartCollision = true;
            damageCoefficient = DamageCoefficient;
            procCoefficient = 1f;
            force = 100f;
            minSpread = 0;
            maxSpread = 0;
            spreadPitchScale = 1f;
            spreadYawScale = 1f;
            spreadBloomValue = 2000f;//uh
            recoilAmplitudeY = 1;
            recoilAmplitudeX = 1;
            muzzleName = "MuzzleGauntlet";
            fireSoundString = "Play_Desolator_Beam_Short";
            muzzleFlashPrefab = goodstate.muzzleFlashPrefab;
            tracerEffectPrefab = DesolatorAssets.DesolatorTracerSnipe;
            hitEffectPrefab = DesolatorAssets.IrradiatedImpactEffect;
            base.OnEnter();

            //PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.05f);
            //GetModelAnimator().SetBool("isHandOut", true);
            //PlayAnimation("Gesture, Additive", "Shock");
            PlayAnimation("Desolator, Override", "DesolatorShoot");
        }
        public override void ModifyBullet(BulletAttack bulletAttack) {
            //bulletAttack.damageType = DamageType.BlightOnHit;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
            //DamageAPI.AddModdedDamageType(bulletAttack, Modules.DamageTypes.DesolatorArmorShred);
            DamageAPI.AddModdedDamageType(bulletAttack, DesolatorDamageTypes.DesolatorDotPrimary);
        }
    }
}
