﻿using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class Utility1ChargeMeleeDash : BaseSkillState {

        public static float BaseMaxChargeDuration = 1;
        public static float MinDamageCoefficient = 1;
        public static float MaxDamageCoefficient = 5;

        private float _maxChargeDuration;
        private float _chargeTimer;
        private bool _playedMaxChargeEffect;
        private bool _success;

        public override void OnEnter() {
            base.OnEnter();

            _maxChargeDuration = BaseMaxChargeDuration / attackSpeedStat;

            PlayAnimation("Fullbody, overried", "charge", "dash.playbackRate", _maxChargeDuration);

            //_chargeSoundID = Util.PlaySound("Play_TeslaChargingUp", gameObject);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            //ManageSound();

            StartAimMode();

            _chargeTimer += Time.fixedDeltaTime;

            if (!_playedMaxChargeEffect && _chargeTimer > _maxChargeDuration) {
                _playedMaxChargeEffect = true;
                playMaxChargeEffect();
            }

            //base.characterBody.SetSpreadBloom(Mathf.Lerp(minBloomRadius, maxBloomRadius, _chargeTimer / _maxChargeDuration), true);

            if (isAuthority && !inputBank.skill1.down && !inputBank.skill3.down) {
                
                float dam = Mathf.Lerp(MinDamageCoefficient, MaxDamageCoefficient, _chargeTimer / _maxChargeDuration);
                //string shootSound = dam >= MaxDamageCoefficient ? "Play_RayGunBigClassic" : "Play_RayGun";
                _success = true;
                Utility1MeleeDashAttack newNextState = new Utility1MeleeDashAttack();
                newNextState.chargedDamage = dam;
                EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(newNextState);
                outer.SetNextStateToMain();
            }
        }

        private void playMaxChargeEffect() {
            //Util.PlaySound("Play_RayGunChargeMax", gameObject);

        }
        public override void OnExit() {
            base.OnExit();
            if (!_success) {
                PlayAnimation("Fullbody, overried", "BufferEmpty");
            }
        }
    }
}