﻿using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;
using System.Linq;
using RoR2.Projectile;
using ThreeEyedGames;
using ModdedEntityStates.Desolator;
using RA2Mod.General.Components;

namespace Modules {
    internal static class Assets
    {
        public static AssetBundle teslaAssetBundle;
        public static AssetBundle desolatorAssetBundle;

        public static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");

        #region tesla
        public static GameObject TeslaCoil;
        public static GameObject TeslaCoilBlueprint;

        public static GameObject TeslaIndicatorPrefab;

        public static GameObject TeslaIndicatorPrefabDash;

        public static GameObject TeslaLoaderZapConeProjectile;
        public static GameObject TeslaZapConeEffect;

        public static GameObject TeslaLightningOrbEffectRed;
        public static GameObject TeslaMageLightningOrbEffectRed;
        public static GameObject TeslaMageLightningOrbEffectRedThick;

        public static Material ChainLightningMaterial;
        #endregion

        #region deso 
        public static GameObject DesolatorTracerRebar;
        public static GameObject DesolatorTracerSnipe;

        public static TeamAreaIndicator DesolatorTeamAreaIndicatorPrefab;

        public static GameObject IrradiatedImpactEffect;

        public static GameObject DesolatorIrradiatorProjectile;
        public static GameObject DesolatorIrradiatorProjectileScepter;

        public static GameObject DesolatorDeployProjectile;
        public static GameObject DesolatorDeployProjectileScepter;
        public static GameObject DesolatorDeployProjectileEmote;

        public static GameObject DesolatorCrocoLeapProjectile;

        public static GameObject DesolatorAuraPrefab;

        #endregion

        public static void Initialize()
        {
            LoadAssetBundles();

            PopulateTeslaAss();
            if (TeslaTrooperPlugin.Desolator) {
                PopulateDesolatorAss();
            }
        }

        public static void LoadAssetBundles()
        {
            if (teslaAssetBundle == null)
            {
                teslaAssetBundle = AssetBundle.LoadFromFile(Files.GetPathToFile("AssetBundles", "teslatrooper"));
            }

            if (TeslaTrooperPlugin.Desolator) {
                desolatorAssetBundle = AssetBundle.LoadFromFile(Files.GetPathToFile("AssetBundles", "desolator"));
            }
        }

        private static void PopulateTeslaAss() {

            TeslaCoilBlueprint = teslaAssetBundle.LoadAsset<GameObject>("TeslaCoilBlueprint");

            TeslaIndicatorPrefab = CreateTeslaTrackingIndicator();

            TeslaIndicatorPrefabDash = CreateTeslaDashTrackingIndicator();

            ChainLightningMaterial = GetChainLightningMaterial();
            TeslaLightningOrbEffectRed =
                CloneLightningOrbEffect("Prefabs/Effects/OrbEffects/LightningOrbEffect",
                                        "NodLightningOrbEffect",
                                        new Color(255f / 255f, 2f / 255f, 1f / 255f),
                                        Color.white,
                                        1.2f);
            TeslaMageLightningOrbEffectRed =
                CloneLightningOrbEffect("Prefabs/Effects/OrbEffects/MageLightningOrbEffect",
                                        "NodMageLightningOrbEffect",
                                        Color.white,
                                        new Color(255f / 255f, 2f / 255f, 1f / 255f));
            TeslaMageLightningOrbEffectRedThick =
                CloneLightningOrbEffect("Prefabs/Effects/OrbEffects/MageLightningOrbEffect",
                                        "NodMageThickLightningOrbEffect",
                                        Color.white,
                                        new Color(255f / 255f, 2f / 255f, 1f / 255f),
                                        2);
            TeslaMageLightningOrbEffectRedThick.GetComponentInChildren<AnimateShaderAlpha>().timeMax = 0.5f;

            TeslaLoaderZapConeProjectile = CreateZapConeProjectile();
        }

        private static void PopulateDesolatorAss() {

            DesolatorTracerRebar = CreateDesolatorTracerRebar();
            DesolatorTracerSnipe = CreateDesolatorTracerSnipe();

            DesolatorTeamAreaIndicatorPrefab = CreateDesolatorTeamAreaIndicator();

            DesolatorIrradiatorProjectile = CreateIrradiatorProjectile();

            DesolatorIrradiatorProjectileScepter = CreateIrradiatorProjectileScepter();

            IrradiatedImpactEffect = DesolatorIrradiatorProjectile.GetComponent<ProjectileDotZone>().impactEffect;

            DesolatorCrocoLeapProjectile = CreateDesolatorCrocoLeapProjectile();
            CreateEffectFromObject(IrradiatedImpactEffect, "", false);

            DesolatorAuraPrefab = CreateDesolatorAura();

            DesolatorDeployProjectile = CreateDesolatorDeployProjectile();
            DesolatorDeployProjectileScepter = CreateDesolatorDeployProjectileScepter();
            DesolatorDeployProjectileEmote= CreateDesolatorDeployProjectileEmote();
        }

        #region tesla stuff
        private static GameObject CreateZapConeProjectile() {
            GameObject zapConeProjectile = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LoaderZapCone"), "TeslaLoaderZapCone");
            
            ProjectileProximityBeamController beamController = zapConeProjectile.GetComponent<ProjectileProximityBeamController>();
            beamController.attackFireCount = ModdedEntityStates.TeslaTrooper.ZapPunch.OrbCasts;
            beamController.attackRange = ModdedEntityStates.TeslaTrooper.ZapPunch.OrbDistance;
            beamController.maxAngleFilter = 50;
            beamController.procCoefficient = ModdedEntityStates.TeslaTrooper.ZapPunch.ProcCoefficient;
            beamController.damageCoefficient = 1;
            beamController.lightningType = RoR2.Orbs.LightningOrb.LightningType.MageLightning;
            //beamController.inheritDamageType = true;

            //DamageAPI.AddModdedDamageType(zapConeProjectile.GetComponent<ProjectileDamage>(), Modules.DamageTypes.conductive);
            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = zapConeProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.Conductive);


            UnityEngine.Object.DestroyImmediate(zapConeProjectile.transform.Find("Effect").GetComponent<ShakeEmitter>());

            TeslaZapConeEffect = CreateTeslaZapConeEffect(zapConeProjectile);

            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/Distortion, 3D").gameObject);
            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/RadialMesh").gameObject);
            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/Flash").gameObject);

            Content.AddProjectilePrefab(zapConeProjectile);

            return zapConeProjectile;
        }

        private static GameObject CreateTeslaZapConeEffect(GameObject zapConeProjectile) {
            GameObject zapConeEffect = PrefabAPI.InstantiateClone(zapConeProjectile.transform.Find("Effect").gameObject, "TeslaPunchConeEffect", false);
            zapConeEffect.SetActive(true);

            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Sparks, Single").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Lines").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Point Light").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Impact Shockwave").gameObject);

            ParticleSystem shockwaveParticle = zapConeEffect.transform.Find("RadialMesh").GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = shockwaveParticle.main;
            mainModule.startSpeed = 0.5f;
            zapConeEffect.transform.Find("RadialMesh").localScale = new Vector3( 0.5f, 0.5f, 1);

            shockwaveParticle.GetComponent<ParticleSystemRenderer>().material.color = Color.cyan;

            CreateEffectFromObject(zapConeEffect, "", false);

            return zapConeEffect;
        }

        private static GameObject CreateTeslaTrackingIndicator() {

            GameObject indicatorPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"), "TeslaIndicator", false);

            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("TextMeshPro").gameObject);
            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("Holder/Brackets").gameObject);

            indicatorPrefab.transform.localScale = Vector3.one * .15f;
            indicatorPrefab.transform.localPosition = Vector3.zero;
            indicatorPrefab.transform.Find("Holder").rotation = Quaternion.identity;
            indicatorPrefab.transform.Find("Holder/Brackets").rotation = Quaternion.identity;

            TeslaIndicatorView indicatorViewComponent = indicatorPrefab.AddComponent<TeslaIndicatorView>();

            SpriteRenderer spriteRenderer = indicatorPrefab.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicator1Close");
            spriteRenderer.color = Color.cyan;
            spriteRenderer.transform.localRotation = Quaternion.identity;
            spriteRenderer.transform.localPosition = Vector3.zero;

            indicatorViewComponent.indicatorRenderer = spriteRenderer;

            SpriteRenderer towerIndicator = UnityEngine.Object.Instantiate(spriteRenderer, spriteRenderer.transform.parent);
            towerIndicator.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorTower2RedWide");
            towerIndicator.color = Color.red;

            indicatorViewComponent.towerIndicator = towerIndicator.gameObject;

            return indicatorPrefab;
        }

        private static GameObject CreateTeslaDashTrackingIndicator() {

            GameObject indicatorPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"), "TeslaIndicator", false);

            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("TextMeshPro").gameObject);
            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("Holder/Brackets").gameObject);

            indicatorPrefab.transform.localScale = Vector3.one * .15f;
            indicatorPrefab.transform.localPosition = Vector3.zero;
            indicatorPrefab.transform.Find("Holder").rotation = Quaternion.identity;
            indicatorPrefab.transform.Find("Holder/Brackets").rotation = Quaternion.identity;

            TeslaIndicatorView indicatorViewComponent = indicatorPrefab.AddComponent<TeslaIndicatorView>();

            SpriteRenderer spriteRenderer = indicatorPrefab.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorDash3");
            spriteRenderer.color = Color.white;
            spriteRenderer.transform.localRotation = Quaternion.identity;
            spriteRenderer.transform.localPosition = Vector3.zero;

            indicatorViewComponent.indicatorRenderer = spriteRenderer;

            //SpriteRenderer towerIndicator = UnityEngine.Object.Instantiate(spriteRenderer, spriteRenderer.transform.parent);
            //towerIndicator.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorDash");
            //towerIndicator.color = Color.white;

            //indicatorViewComponent.towerIndicator = towerIndicator.gameObject;

            return indicatorPrefab;
        }

        private static Material GetChainLightningMaterial() {
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect").GetComponentInChildren<LineRenderer>().material;
        }
        #endregion tesla stuff

        #region desolator stuff

        private static GameObject CreateDesolatorTracerRebar() {
            GameObject tracer = CloneTracer("TracerToolbotRebar", "TracerDeslotorRebar", Color.green, 3);

            UnityEngine.Object.Destroy(tracer.transform.Find("StickEffect").gameObject);

            return tracer;
        }

        private static GameObject CreateDesolatorTracerSnipe() {
            GameObject tracer = CloneTracer("TracerHuntressSnipe", "TracerDeslotorHuntressSnipe", Color.green, 3);

            UnityEngine.Object.Destroy(tracer.transform.Find("TracerHead").gameObject);

            return tracer;
        }

        private static TeamAreaIndicator CreateDesolatorTeamAreaIndicator() {
            GameObject impvoidspike = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ImpVoidspikeProjectile");

            TeamAreaIndicator teamAreaIndicator = PrefabAPI.InstantiateClone(impvoidspike.transform.Find("ImpactEffect/TeamAreaIndicator, FullSphere").gameObject, "DesolatorTeamAreaIndicator", false).GetComponent<TeamAreaIndicator>();

            teamAreaIndicator.teamMaterialPairs[1].sharedMaterial = new Material(teamAreaIndicator.teamMaterialPairs[1].sharedMaterial);
            teamAreaIndicator.teamMaterialPairs[1].sharedMaterial.SetColor("_TintColor", Color.green);

            return teamAreaIndicator;
        }

        private static GameObject CreateDesolatorDeployProjectile() {

            GameObject DeployProjectile = PrefabAPI.InstantiateClone(LoadAsset<GameObject>("DeployProjectile"), "DeployProjectile", true);

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = DeployProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, DeployProjectile.transform);
            areaIndicator.teamFilter = DeployProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * DeployIrradiate.Range;

            DeployProjectile.transform.Find("Hitboxes").localScale = Vector3.one * DeployIrradiate.Range;

            DeployProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            Content.AddProjectilePrefab(DeployProjectile);

            return DeployProjectile;
        }


        private static GameObject CreateDesolatorDeployProjectileEmote() {

            GameObject DeployProjectile = PrefabAPI.InstantiateClone(LoadAsset<GameObject>("DeployProjectileEmote"), "DeployProjectileEmote", true);

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = DeployProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.DesolatorDot);

            //TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, DeployProjectile.transform);
            //areaIndicator.teamFilter = DeployProjectile.GetComponent<TeamFilter>();
            //areaIndicator.transform.localScale = Vector3.one * DeployIrradiate.Range;

            DeployProjectile.transform.Find("Hitboxes").localScale = Vector3.one * EmoteRadiationProjectile.Range;

            DeployProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = EmoteRadiationProjectile.Range;

            Content.AddProjectilePrefab(DeployProjectile);

            return DeployProjectile;
        }

        private static GameObject CreateDesolatorDeployProjectileScepter() {

            GameObject DeployProjectile = PrefabAPI.InstantiateClone(LoadAsset<GameObject>("DeployProjectile"), "DeployProjectileScepter", true);

            DeployProjectile.GetComponent<ProjectileDotZone>().resetFrequency = 1.5f;

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = DeployProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, DeployProjectile.transform);
            areaIndicator.teamFilter = DeployProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ScepterDeployIrradiate.ScepterRange;

            DeployProjectile.transform.Find("Hitboxes").localScale = Vector3.one * ScepterDeployIrradiate.ScepterRange;

            DeployProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            Content.AddProjectilePrefab(DeployProjectile);

            return DeployProjectile;
        }

        private static GameObject CreateIrradiatorProjectile() {

            GameObject irradiatorProjectile = PrefabAPI.InstantiateClone(LoadAsset<GameObject>("IrradiatorProjectile"), "IrradiatorProjectile", true);

            Renderer ghostRenderer = irradiatorProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<Renderer>();
            ghostRenderer.material = ghostRenderer.material.SetHotpooMaterial();

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = irradiatorProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, irradiatorProjectile.transform);
            areaIndicator.teamFilter = irradiatorProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectile.transform.Find("Hitboxes").localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            Content.AddProjectilePrefab(irradiatorProjectile);

            return irradiatorProjectile;
        }

        private static GameObject CreateIrradiatorProjectileScepter() {
            GameObject irradiatorProjectileScepter = PrefabAPI.InstantiateClone(LoadAsset<GameObject>("IrradiatorProjectileScepter"), "IrradiatorProjectileScepter", true);

            Renderer ghostRenderer = irradiatorProjectileScepter.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<Renderer>();
            ghostRenderer.material = ghostRenderer.material.SetHotpooMaterial();

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = irradiatorProjectileScepter.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, irradiatorProjectileScepter.transform);
            areaIndicator.teamFilter = irradiatorProjectileScepter.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectileScepter.transform.Find("Hitboxes").localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectileScepter.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            ProjectileImpactExplosion impactExplosion = irradiatorProjectileScepter.GetComponent<ProjectileImpactExplosion>();
            impactExplosion.blastRadius = ThrowIrradiator.Range;
            impactExplosion.blastDamageCoefficient = ScepterThrowIrradiator.explosionDamageCoefficient;
            impactExplosion.explosionEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXGreaterWisp");

            Content.AddProjectilePrefab(irradiatorProjectileScepter);

            return irradiatorProjectileScepter;
        }
        
        private static GameObject CreateDesolatorCrocoLeapProjectile() {

            GameObject leapAcidProjectile = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CrocoLeapAcid"), "DesolatorLeapAcid");

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = leapAcidProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.DesolatorDot);

            ProjectileDotZone projectileDotZone = leapAcidProjectile.GetComponent<ProjectileDotZone>();
            projectileDotZone.impactEffect = IrradiatedImpactEffect;
            projectileDotZone.lifetime = AimBigRadBeam.DotZoneLifetime;
            projectileDotZone.damageCoefficient = 1;
            projectileDotZone.overlapProcCoefficient = 0.5f;
            projectileDotZone.resetFrequency = 2F;

            leapAcidProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            
            Transform transformFindFX = leapAcidProjectile.transform.Find("FX");
            transformFindFX.transform.localScale = Vector3.one * ModdedEntityStates.Desolator.AimBigRadBeam.BaseAttackRadius * 1.8f;
            UnityEngine.Object.Destroy(transformFindFX.GetComponent<AlignToNormal>());

            Transform transformFindDecal = leapAcidProjectile.transform.Find("FX/Decal");
            float scale = AimBigRadBeam.BaseAttackRadius * 0.10f;
            transformFindDecal.localScale = new Vector3(scale, 0.85f, scale);
            transformFindDecal.GetComponent<Decal>().Material.SetTexture("_MainTexture", LoadAsset<Texture2D>("texDesolatorDecal"));

            AlignToNormal alignComponent = transformFindDecal.gameObject.AddComponent<AlignToNormal>();
            alignComponent.maxDistance = ModdedEntityStates.Desolator.AimBigRadBeam.BaseAttackRadius;
            alignComponent.offsetDistance = 1.5f;

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, leapAcidProjectile.transform);
            areaIndicator.teamFilter = leapAcidProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ModdedEntityStates.Desolator.AimBigRadBeam.BaseAttackRadius;
            areaIndicator.transform.parent = transformFindFX;

            leapAcidProjectile.transform.Find("FX/Hitbox (1)").localScale = Vector3.one;

            Content.AddProjectilePrefab(leapAcidProjectile);

            return leapAcidProjectile;
        }

        private static GameObject CreateDesolatorAura() {
            GameObject aura = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/IcicleAura"), "DesolatorAura", true);

            UnityEngine.Object.Destroy(aura.GetComponent<IcicleAuraController>());
            aura.AddComponent<DesolatorAuraController>();

            UnityEngine.Object.Destroy(aura.transform.Find("Particles/Chunks").gameObject);
            UnityEngine.Object.Destroy(aura.transform.Find("Particles/SpinningSharpChunks").gameObject);
            UnityEngine.Object.Destroy(aura.transform.Find("Particles/Ring, Procced").gameObject);
            UnityEngine.Object.Destroy(aura.GetComponent<AkGameObj>());
            
            BuffWard buffWard = aura.GetComponent<BuffWard>();
            buffWard.buffDef = LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Weak");
            buffWard.radius = RadiationAura.Radius;

            greenify(aura.transform.Find("Particles/Ring, Outer").GetComponent<ParticleSystem>());
            greenify(aura.transform.Find("Particles/Ring, Core").GetComponent<ParticleSystem>());

            aura.transform.Find("Particles/Area").GetComponent<ParticleSystemRenderer>().material = DesolatorTeamAreaIndicatorPrefab.teamMaterialPairs[1].sharedMaterial;
            
            return aura;
        }

        private static void greenify(ParticleSystem particleSystem) {
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = Color.green;
        }

        #endregion desolator stuff


        public static T LoadAsset<T>(string assString) where T : UnityEngine.Object
        {
            T loadedAss = teslaAssetBundle.LoadAsset<T>(assString);
            
            if(loadedAss == null) {
                 loadedAss = desolatorAssetBundle.LoadAsset<T>(assString);
            }

            if (loadedAss == null) {
                Debug.LogError($"Null asset: {assString}.\nAttempt to load asset '{assString}' from assetbundles returned null");
            }

            return loadedAss;
        }

        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = Modules.Assets.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Debug.LogError("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        public static void ConvertAllRenderersToHopooShader(GameObject objectToConvert) {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }
        }

        public static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        private static GameObject CloneLightningOrbEffect(string path, string name, Color beamColor, Color? lineColor = null, float width = 1) {
            GameObject newEffect = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>(path), name, false);

            foreach (LineRenderer rend in newEffect.GetComponentsInChildren<LineRenderer>()) {
                if (rend) {
                    Material mat = UnityEngine.Object.Instantiate<Material>(rend.material);
                    mat.SetColor("_TintColor", beamColor);
                    rend.material = mat;

                    if (lineColor != null) {
                        rend.startColor = lineColor.Value;
                        rend.endColor = lineColor.Value;
                    }
                    rend.widthMultiplier = width;
                }
            }
            AddNewEffectDef(newEffect);

            return newEffect;
        }

        private static GameObject CloneTracer(string originalTracerName, string newTracerName, Color? color = null, float widthMultiplierMultiplier = 1, float? speed = null, float? length = null)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            newTracer.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = speed.HasValue? speed.Value : newTracer.GetComponent<Tracer>().speed;
            newTracer.GetComponent<Tracer>().length = length.HasValue ? length.Value : newTracer.GetComponent<Tracer>().length;

            if (color.HasValue || widthMultiplierMultiplier != 1) {
                foreach (var lineREnderer in newTracer.GetComponentsInChildren<LineRenderer>()) {
                    if (color.HasValue) {
                        lineREnderer.startColor = color.Value;
                        lineREnderer.endColor = color.Value;
                    }
                    if(widthMultiplierMultiplier != 1){
                        lineREnderer.widthMultiplier *= widthMultiplierMultiplier;
                    }
                }

                foreach (ParticleSystem particles in newTracer.GetComponentsInChildren<ParticleSystem>()) {
                    ParticleSystem.MainModule mainModule = particles.main;
                    mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant * widthMultiplierMultiplier);
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(color.Value);

                    ParticleSystem.TrailModule trailModule = particles.trails;
                    if (trailModule.enabled) {

                        //Gradient gradient = trailModule.colorOverLifetime.gradientMin;
                        //gradient.colorKeys[0].color = Color.green;
                        Gradient gradient = new Gradient();
                        GradientColorKey[] colorKey = new GradientColorKey[2];
                        colorKey[0].color = Color.green;
                        colorKey[0].time = 0.0f;
                        colorKey[1].color = Color.green;
                        colorKey[1].time = 1.0f;

                        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
                        alphaKey[0].alpha = 1.0f;
                        alphaKey[0].time = 0.0f;
                        alphaKey[1].alpha = 1.0f;
                        alphaKey[1].time = 1.0f;

                        gradient.SetKeys(colorKey, alphaKey);

                        trailModule.colorOverLifetime = new ParticleSystem.MinMaxGradient(gradient);
                    }
                }
            }
            
            if (color.HasValue) {
                foreach(var rend in newTracer.GetComponentsInChildren<ParticleSystemRenderer>()) {
                    rend.material.SetColor("_MainColor", color.Value);
                    rend.material.SetColor("_Color", color.Value);
                    rend.material.SetColor("_TintColor", color.Value);
                }
            }

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        public static Texture LoadCharacterIcon(string icon) {
            return Modules.Assets.LoadAsset<Texture>(icon);
        }

        private static GameObject LoadEffect(string resourceName) => LoadEffect(resourceName, "", false);
        private static GameObject LoadEffect(string resourceName, string soundName) => LoadEffect(resourceName, soundName, false);
        private static GameObject LoadEffect(string resourceName, bool parentToTransform) => LoadEffect(resourceName, "", parentToTransform);
        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform) {
            //bool assetExists = false;
            //for (int i = 0; i < assetNames.Length; i++) {
            //    if (assetNames[i].Contains(resourceName.ToLowerInvariant())) {
            //        assetExists = true;
            //        break;
            //    }
            //}

            //if (!assetExists) {
            //    Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
            //    return null;
            //}

            GameObject newEffect = LoadAsset<GameObject>(resourceName);
            CreateEffectFromObject(newEffect, soundName, parentToTransform);

            return newEffect;
        }

        private static void CreateEffectFromObject(GameObject newEffect, string soundName, bool parentToTransform) {
            newEffect.AddComponent<DestroyOnTimer>().duration = 6;
            newEffect.AddComponent<NetworkIdentity>();
            if (!newEffect.GetComponent<VFXAttributes>()) {
                newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            }

            EffectComponent effect = newEffect.GetComponent<EffectComponent>();
            if (!effect) {
                effect = newEffect.AddComponent<EffectComponent>();
                effect.applyScale = true;
                effect.effectIndex = EffectIndex.Invalid;
                effect.parentToReferencedTransform = parentToTransform;
                effect.positionAtReferencedTransform = true;
                effect.soundName = soundName;
            }
            
            AddNewEffectDef(newEffect, soundName);
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName = "")
        {
            EffectDef newEffectDef = new EffectDef(effectPrefab);
            //newEffectDef.prefab = effectPrefab;
            //newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            //newEffectDef.prefabName = effectPrefab.name;
            //newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            //newEffectDef.spawnSoundEventName = soundName;

            Modules.Content.AddEffectDef(newEffectDef);
        }

        /// <summary>
        /// search for crosshair prefabs here. plug in the character or crosshair name
        /// </summary>
        /// <para>https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html</para>
        public static GameObject LoadCrosshair(string crosshairName) {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        #region materials(old)
        private const string obsolete = "use `Materials.CreateMaterial` instead, or use the extension `Material.SetHotpooMaterial` directly on a material";

        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName) => Assets.CreateMaterial(materialName, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission) => Assets.CreateMaterial(materialName, emission, Color.white);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor) => Assets.CreateMaterial(materialName, emission, emissionColor, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            return Materials.CreateHotpooMaterial(materialName)
                            .MakeUnique()
                            .SetEmission(emission, emissionColor)
                            .SetNormal(normalStrength);
        }
        #endregion materials(old)

        #region materials new (simple)(example)
        //public static Material CreateHotpooMaterial(string materialName) {

        //    //Material mat = UnityEngine.Object.Instantiate<Material>(Assets.commandoMat);
        //    Material tempMat = Assets.LoadAsset<Material>(materialName);

        //    if (!tempMat) {
        //        Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
        //        return new Material(Assets.hotpoo);
        //    }

        //    return new Material(tempMat).SetHotpooMaterial;
        //}

        //public static Material SetHotpooMaterial(this Material tempMat) {

        //    float? bumpScale = null;
        //    Color? emissionColor = null;

        //    //grab values before the shader changes
        //    if (tempMat.IsKeywordEnabled("_NORMALMAP")) {
        //        bumpScale = tempMat.GetFloat("_BumpScale");
        //    }
        //    if (tempMat.IsKeywordEnabled("_EMISSION")) {
        //        emissionColor = tempMat.GetColor("_EmissionColor");
        //    }

        //    tempMat.shader = Assets.hotpoo;

        //    tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
        //    tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
        //    tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));

        //    if (bumpScale != null) {
        //        tempMat.SetFloat("_NormalStrength", (float)bumpScale);
        //    }
        //    if (emissionColor != null) {
        //        tempMat.SetColor("_EmColor", (Color)emissionColor);
        //        tempMat.SetFloat("_EmPower", 1);
        //    }

        //    return tempMat;
        //}
        #endregion materials new (simple)(example)
    }
}