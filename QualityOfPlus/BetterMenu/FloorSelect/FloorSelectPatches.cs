using HarmonyLib;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterMenu.FloorSelect
{
    // Code by TicklerXD
    [HarmonyPatch(typeof(HideSeekMenu))]
    internal class FloorSelectPatches
    {
        [HarmonyPatch(nameof(HideSeekMenu.Awake))]
        [HarmonyPostfix]
        private static void SetActiveButtons(HideSeekMenu __instance)
        {
            if (!QOPManager.Instance.GetFeature<FloorSelectFeature>().IsEnabled())
                return;

            for (int i = 2; i <= 5; i++)
                __instance.transform.Find($"MainNew_{i}").gameObject.SetActive(true);
        }
    }
}
