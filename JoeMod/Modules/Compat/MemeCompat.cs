﻿using UnityEngine;
using Modules.Survivors;

namespace Modules {
    public class MemeCompat {
        public static void init() {
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig) {

            GameObject skele = Assets.LoadAsset<GameObject>("TeslaTrooper_meme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(TeslaTrooperSurvivor.instance.bodyPrefab, skele, true);
            //skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;
            orig();
        }
    }
}