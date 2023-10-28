﻿using EntityStates;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.BaseStates
{
    public class BaseMeleeAttackButEpic : BaseSkillState, SteppedSkillDef.IStepSetter {
        public int swingIndex;

        protected string hitboxName = "Sword";

        protected DamageType damageType = DamageType.Generic;
        protected float damageCoefficient = 3.5f;
        protected float procCoefficient = 1f;
        protected float pushForce = 300f;
        protected Vector3 bonusForce = Vector3.zero;

        protected float baseDuration = 1f;
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected float baseEarlyExitTime = 0.4f;

        protected float hitStopDuration = 0.012f;
        protected float attackRecoil = 0.75f;
        protected float hitHopVelocity = 4f;
        protected bool cancelled = false;
        protected bool keypress;

        protected string swingSoundString = "";
        protected string hitSoundString = "";
        protected string muzzleString = "SwingCenter";
        protected string hitstopAnimationParameter;
        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;

        private float earlyExitTime;
        protected float duration;
        protected bool hasFired;
        private float hitPauseTimer;
        protected OverlapAttack attack;
        protected bool inHitPause;
        private bool hasHopped;
        protected float stopwatch;
        protected Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;
        protected bool rolledCrit;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExitTime = this.baseEarlyExitTime  * this.duration;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            base.StartAimMode(0.5f + this.duration, false);
            this.animator.SetBool("attacking", true);
            this.rolledCrit = base.RollCrit();

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
            }


            this.PlayAttackAnimation();

            this.attack = new OverlapAttack();
            this.attack.damageType = this.damageType;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = this.damageCoefficient * this.damageStat;
            this.attack.procCoefficient = this.procCoefficient;
            this.attack.hitEffectPrefab = this.hitEffectPrefab;
            this.attack.forceVector = this.bonusForce;
            this.attack.pushAwayForce = this.pushForce;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = rolledCrit;
            this.attack.impactSound = this.impactSound;

            //if (Modules.VRCompat.IsLocalVRPlayer(base.characterBody)) {
            //    attackEndTime -= attackStartTime * 0.5f;
            //    attackStartTime = 0;
            //}
        }

        protected virtual void PlayAttackAnimation()
        {
            base.PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), "Slash.playbackRate", this.duration, 0.05f);
        }

        public override void OnExit()
        {
            base.OnExit();

            this.animator.SetBool("attacking", false);
        }

        protected virtual void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.muzzleString, true);
            
            //Transform MuzzleTransform = FindModelChild(muzzleString);
            //EffectData effectData = new EffectData {
            //    origin = MuzzleTransform.position,
            //    rotation = MuzzleTransform.rotation,
            //};
            //EffectManager.SpawnEffect(this.swingEffectPrefab, effectData, true);
        }

        protected virtual void OnHitEnemyAuthority() => OnHitEnemyAuthority(null);
        protected virtual void OnHitEnemyAuthority(List<HurtBox> hits) {
            Util.PlaySound(this.hitSoundString, base.gameObject);

            if (!this.hasHopped) {
                if (base.characterMotor && !base.characterMotor.isGrounded && this.hitHopVelocity > 0f) {
                    base.SmallHop(base.characterMotor, (this.hitHopVelocity / Mathf.Sqrt(this.attackSpeedStat)));
                }

                this.hasHopped = true;
            }

            ApplyHitstop();
        }

        protected void ApplyHitstop() {

            if (!this.inHitPause && this.hitStopDuration > 0f) {
                this.storedVelocity = base.characterMotor.velocity;
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, this.hitstopAnimationParameter);
                this.hitPauseTimer = this.hitStopDuration / this.attackSpeedStat;
                this.inHitPause = true;
            }
        }

        protected virtual void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayAttackSpeedSound(this.swingSoundString, base.gameObject, this.attackSpeedStat);

                if (base.isAuthority)
                {
                    this.PlaySwingEffect();
                    base.AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
                }
            }

            if (base.isAuthority) {

                if (this.attack.Fire()) {
                    this.OnHitEnemyAuthority();
                }
            }
        }

        protected virtual void SetNextState()
        {
            base.outer.SetNextStateToMain();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                base.characterMotor.velocity = this.storedVelocity;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Swing.playbackRate", 0f);
            }


            bool fireStarted = stopwatch >= this.duration * attackStartTime;
            bool fireEnded = stopwatch >= this.duration * attackEndTime;

            //to guarantee attack comes out if at high attack speed the stopwatch skips past the firing duration between frames
            if ((fireStarted && !fireEnded) || (fireStarted && fireEnded && !this.hasFired)) {

                if (!hasFired) {
                    OnFireAttackEnter();
                }

                FireAttack();
            }

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                SetNextState();
                return;
            }
        }

        protected virtual void OnFireAttackEnter() { }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return fixedAge > earlyExitTime? InterruptPriority.Any: InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }

        public virtual void SetStep(int i) {
            swingIndex = i;
        }
    }
}