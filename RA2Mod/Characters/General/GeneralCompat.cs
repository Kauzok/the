﻿using EntityStates;
using RA2Mod.General.States;
using RA2Mod.Modules;
using RoR2;
using RoR2.Skills;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRAPI;

namespace RA2Mod.General
{
    public static class GeneralStates
    {
        public static void Init()
        {
            Content.AddEntityState(typeof(WindDownState));
            Content.AddEntityState(typeof(Rest));
        }
    }
    public static class GeneralCompat
    {
        public delegate void Meme_SurivorCatalog_Init();
        public static event Meme_SurivorCatalog_Init Meme_OnSurvivorCatalog_Init;

        //todo teslamove hooks
        public delegate void Driver_SurvivorCatalog_SetSurvivorDefs(GameObject driverBody);
        public static event Driver_SurvivorCatalog_SetSurvivorDefs FuckWithDriver;

        public static bool TinkersSatchelInstalled;
        public static bool AetheriumInstalled;
        public static bool ScepterInstalled;
        public static bool VREnabled;

        public static bool driverInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rob.Driver");

        public static void Init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.TinkersSatchel"))
            {
                TinkersSatchelInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium"))
            {
                AetheriumInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
            {
                ScepterInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
            if (driverInstalled)
            {
                On.RoR2.SurvivorCatalog.SetSurvivorDefs += SurvivorCatalog_SetSurvivorDefs;
            }
        }

        private static void SurvivorCatalog_SetSurvivorDefs(On.RoR2.SurvivorCatalog.orig_SetSurvivorDefs orig, SurvivorDef[] newSurvivorDefs)
        {
            orig(newSurvivorDefs);

            for (int i = 0; i < newSurvivorDefs.Length; i++)
            {
                if (newSurvivorDefs[i].bodyPrefab.name == "RobDriverBody")
                {
                    FuckWithDriver?.Invoke(newSurvivorDefs[i].bodyPrefab);
                    return;
                }
            }

            Log.Debug("no driver. ra2 compat failed");
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            Meme_OnSurvivorCatalog_Init?.Invoke();
            orig();
        }

        internal static int TryGetScepterCount(Inventory inventory)
        {
            if (!ScepterInstalled)
                return 0;

            return GetScepterCount(inventory);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static int GetScepterCount(Inventory inventory)
        {
            return inventory.GetItemCount(AncientScepter.AncientScepterItem.instance.ItemDef);
        }


        #region vr helpers
        public static Ray GetAimRay(this BaseState state, bool dominant = true)
        {
            if (IsLocalVRPlayer(state.characterBody))
            {
                return GetVrAimRay(dominant);
            }
            return state.GetAimRay();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Ray GetVrAimRay(bool dominant)
        {
            return dominant ? MotionControls.dominantHand.aimRay : MotionControls.nonDominantHand.aimRay;
        }

        public static Ray GetAimRayCamera(BaseState state)
        {
            if (IsLocalVRPlayer(state.characterBody))
            {
                return GetVRAimRayCamera();
            }
            return state.GetAimRay();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Ray GetVRAimRayCamera()
        {
            //todo teslamove no camera.main in fixedupdate
            return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        public static ChildLocator GetModelChildLocator(this BaseState state, bool dominant = true)
        {
            if (IsLocalVRPlayer(state.characterBody))
            {
                return GetVRChildLocator(dominant);
            }
            return state.GetModelChildLocator();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static ChildLocator GetVRChildLocator(bool dominant)
        {
            if (dominant)
            {
                return MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();
            }
            else
            {
                return MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>();
            }
        }

        public static bool IsLocalVRPlayer(CharacterBody body)
        {
            return General.GeneralCompat.VREnabled && body == LocalUserManager.GetFirstLocalUser().cachedBody;
        }
        #endregion vr helpers
    }
}
