using HarmonyLib;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterMenu.UnlockedSeedInput
{
    [HarmonyPatch(typeof(HideSeekMenu))]
    internal class UnlockedSeedInputPatches
    {
        [HarmonyPatch(nameof(HideSeekMenu.Awake))]
        [HarmonyPostfix]
        private static void UnlockSeed(HideSeekMenu __instance)
        {
            if (!QOPManager.Instance.GetFeature<UnlockedSeedInputFeature>().IsEnabled())
                return;

            __instance.transform.Find("SeedInput").gameObject.SetActive(true);
        }
    }
}
