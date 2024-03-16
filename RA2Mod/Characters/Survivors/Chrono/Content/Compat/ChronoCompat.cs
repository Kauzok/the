﻿using RA2Mod.General;
using RTAutoSprintEx;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoCompat
    {
        public static bool AutoSprintInstalled;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void Init()
        {
            if (AutoSprintInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                RA2Plugin.instance.StartCoroutine(AutoSprintCompat());
            }
            if (GeneralCompat.driverInstalled)
            {
                new DriverCompat().Init();
            }
        }

        private static IEnumerator AutoSprintCompat()
        {
            //let awake happen
            yield return null;
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.ChronoCharacterMain");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.PhaseState");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.AimChronosphere1");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.AimChronosphere2");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.PlaceChronosphere2");
        }
    }
}
