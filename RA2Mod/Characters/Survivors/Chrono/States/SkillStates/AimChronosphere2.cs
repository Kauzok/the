﻿using EntityStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class AimChronosphere2 : AimChronosphereBase
    {
        public Vector3 originalPoint;
        
        private GameObject projectionGameObject;

        private float sqrRadius;

        public List<CharacterBody> teleporteeBodies = new List<CharacterBody>();
        public CameraTargetParams.CameraParamsOverrideHandle cameraOverride;
        public Transform origOrigin;

        public override void OnEnter()
        {
            ChronosphereProjection chronosphereProjection = Object.Instantiate(ChronoAssets.chronosphereProjection);
            chronosphereProjection.transform.position = originalPoint;
            projectionGameObject = chronosphereProjection.gameObject;
            chronosphereProjection.SetRadiusAndEnable(AimChronosphereBase.BaseRadius);

            //if (isAuthority)
            //{
            //    for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
            //    {
            //        GatherTeleportees(TeamComponent.GetTeamMembers(teamIndex));
            //    }
            //}

            //if (NetworkServer.active)
            //{
            //    RootTeleportees();
            //}

            sqrRadius = BaseRadius * BaseRadius;

            skillLocator.utility.SetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.OnEnter();

            viewRadius *= 0.5f;
        }
        
        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        if (FriendlyFireManager.ShouldDirectHitProceed(body.healthComponent, teamComponent.teamIndex))
                        {
                            teleporteeBodies.Add(body);
                        }
                    }
                }
            }
        }

        private void RootTeleportees()
        {
            for (int i = 0; i < teleporteeBodies.Count; i++)
            {
                teleporteeBodies[i].AddBuff(ChronoBuffs.chronosphereRootDebuff);
            }
        }

        private void UnRootTeleportees()
        {
            for (int i = 0; i < teleporteeBodies.Count; i++)
            {
                teleporteeBodies[i].RemoveBuff(ChronoBuffs.chronosphereRootDebuff);
            }
        }

        public override void UpdateTrajectoryInfo(out TrajectoryInfo dest)
        {
            base.UpdateTrajectoryInfo(out dest);

            Vector3 vector = dest.hitPoint - originalPoint;
            dest.finalRay.origin = originalPoint;
            dest.finalRay.direction = vector.normalized;
            dest.speedOverride = this.projectileBaseSpeed;
            dest.travelTime = this.projectileBaseSpeed / vector.magnitude;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (castSuccessful)
            {
                Util.PlaySound("Play_ChronosphereMove", gameObject);
            }

            //if (NetworkServer.active)
            //{
            //    UnRootTeleportees();
            //}

            Object.Destroy(projectionGameObject);

            skillLocator.utility.UnsetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);
            
            characterBody.aimOriginTransform = origOrigin;
            cameraTargetParams.RemoveParamsOverride(cameraOverride, 0.5f);
        }

        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new PlaceChronosphere2 {
                originalPoint = originalPoint,
                BaseRadius = BaseRadius,
                trajectoryPoint = currentTrajectoryInfo.hitPoint,
                //teamCharacterBodies = teleporteeBodies
            };
        }

        protected override void PlayEnterSounds()
        {
            Util.PlaySound("Play_ChronosphereSelectStart", projectionGameObject);
            Util.PlaySound("Play_ChronosphereSelectLoop", projectionGameObject);
        }
        
        protected override void PlayExitSounds()
        {

            Util.PlaySound("Stop_ChronosphereSelectLoop", projectionGameObject);
            Util.PlaySound("Play_ChronosphereSelectEnd", projectionGameObject);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(originalPoint);
            for (int i = 0; i < teleporteeBodies.Count; i++)
            {
                writer.Write(teleporteeBodies[i].gameObject);
            }
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            originalPoint = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                teleporteeBodies.Add(reader.ReadGameObject().GetComponent<CharacterBody>());
            }
        }
    }
}