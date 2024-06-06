﻿using AliemMod.Content;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootSawedOffCharged : RayGunFire {

        public override float BaseDuration => 1f;

        public override float BaseDamageCoefficient => _chargedDamageCoefficient;
        public override string soundString => _chargedShootSound;
        public override string muzzleString => isOffHanded ? "BlasterMuzzleFar.R" : "BlasterMuzzleFar";
        public override GameObject muzzleEffectPrefab => Modules.Assets.sawedOffMuzzleFlash;

        public override GameObject projectile => Modules.Projectiles.SawedOffProjectilePrefabBig;

        private float _chargedDamageCoefficient;
        private string _chargedShootSound = "Play_AliemSawedOffCharged";

        public ShootSawedOffCharged() {
            _chargedDamageCoefficient = AliemConfig.M1_SawedOffCharged_Damage_Max.Value;
        }

        public ShootSawedOffCharged(float dam_) {
            _chargedDamageCoefficient = dam_;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (!isGrounded)
            {
                characterMotor.velocity = characterMotor.velocity * (1 - AliemConfig.shotgunKnockbackSpeedOverride.Value) - GetAimRay().direction * AliemConfig.M1_SawedOffCharged_SelfKnockback.Value;
            }
        }

        public override void FireProjectile()
        {
            if (isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                projectilePitchBonus *= (1 - Mathf.Max(0, Vector3.Dot(aimRay.direction.normalized, Vector3.up)));

                aimRay.direction = Util.ApplySpread(aimRay.direction, this.minSpread, this.maxSpread, 1f, 1f, 0f, this.projectilePitchBonus);
                aimRay = this.ModifyProjectileAimRay(aimRay);

                Vector3 horizontal = Vector3.Cross(aimRay.direction, Vector3.up).normalized;

                Ray leftRay = new Ray(aimRay.origin, aimRay.direction);
                leftRay.origin += horizontal * AliemConfig.   shotgunChargedSideShift.Value;
                leftRay.direction += horizontal * AliemConfig.shotgunChargedSideTurn .Value;

                FireProjectileSingle(leftRay);

                Ray rightRay = new Ray(aimRay.origin, aimRay.direction);
                rightRay.origin += horizontal * -AliemConfig.shotgunChargedSideShift.Value;
                rightRay.direction += horizontal * -AliemConfig.shotgunChargedSideTurn.Value;

                FireProjectileSingle(rightRay);

            }
        }

        private void FireProjectileSingle(Ray aimRay)
        {
            ProjectileManager.instance.FireProjectile(this.projectilePrefab,
                aimRay.origin,
                Util.QuaternionSafeLookRotation(aimRay.direction),
                base.gameObject,
                this.damageStat * this.damageCoefficient,
                this.force,
                Util.CheckRoll(this.critStat, base.characterBody.master),
                DamageColorIndex.Default,
                null,
                -1f);
        }

        protected override void ModifyState()
        {
            base.ModifyState();
            projectilePitchBonus = AliemConfig.ShotgunChargedPitch.Value;
        }

        public override void PlayAnimation(float duration) {
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", duration*2);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGunBig");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGunBig");
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}
