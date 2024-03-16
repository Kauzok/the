﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapCollectDamage : BaseSkillState {

        public static float ShieldBuffDuration = 4;

        public float skillsPlusSeconds = 0;

        public RoR2.CameraTargetParams.AimRequest aimRequest;

        private float blockedDamage = 0;

        private bool completed;
        private bool buffDetectedAtSomePoint;

        public override void OnEnter() {
            base.OnEnter();

            EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(new ShieldZapStart());

            TeslaZapBarrierController controller = GetComponent<TeslaZapBarrierController>();
            if (controller) {
                controller.StartRecordingDamage();
            }

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            if (!base.characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                CharacterModel characterModel = base.GetModelTransform().GetComponent<CharacterModel>();

                TemporaryOverlay temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = ShieldBuffDuration + 1;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matIsShocked");
                temporaryOverlay.AddToCharacerModel(characterModel);
            }

            if (NetworkServer.active) {

                Util.CleanseBody(base.characterBody, true, false, false, true, true, false);
                base.characterBody.AddTimedBuff(Modules.Buffs.zapShieldBuff, ShieldBuffDuration + skillsPlusSeconds);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!buffDetectedAtSomePoint) {
                buffDetectedAtSomePoint |= characterBody.HasBuff(Modules.Buffs.zapShieldBuff);
                return;
            }

            //UGLY HACK: client takes a sec to realize host has given the body a buff up in onEnter
            if (buffDetectedAtSomePoint && !characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                ShieldZapReleaseDamage newNextState = new ShieldZapReleaseDamage() {
                    aimRequest = this.aimRequest,
                    collectedDamage = blockedDamage,
                };
                /*EntityStateMachine.FindByCustomName(gameObject, "Weapon")*/outer.SetNextState(newNextState);
                completed = true;
                //base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (!completed) {
                if (aimRequest != null)
                    aimRequest.Dispose();
            }
        }
    }
}