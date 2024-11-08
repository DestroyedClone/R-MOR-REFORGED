﻿using UnityEngine;
using RoR2;
using UnityEngine.AddressableAssets;
using R2API;
using System;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using UnityEngine.Networking;
using RoR2.Audio;
using RMORMod.Modules;

namespace RMORMod.Content.RMORSurvivor
{
    public class Buffs
    {
        private static NetworkSoundEventDef platingSound = LegacyResourcesAPI.Load<NetworkSoundEventDef>("NetworkSoundEventDefs/nseArmorPlateBlock");
        public static BuffDef RMORPassive;

        public static void Init()
        {
            if (!RMORPassive)
            {
                RMORPassive = Modules.Buffs.CreateBuffDef(
                       "SWARM_ASSAULT",
                       true,
                       false,
                       false,
                       new Color(255f / 255f, 0f / 255f, 84f / 255f),
                       Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSwarmAssault.png")
                       );
                RecalculateStatsAPI.GetStatCoefficients += RMORPassiveHook;
            }
        }
        private static void RMORPassiveHook(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(Buffs.RMORPassive))
            {
                args.attackSpeedMultAdd += sender.GetBuffCount(RMORPassive) * 0.05f;
            }
        }
    }
}
