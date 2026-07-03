using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap.CustomGridColor
{
    [HarmonyPatch(typeof(Map))]
    internal class CustomGridColorPatches
    {
        [HarmonyPatch(nameof(Map.OpenMap))]
        [HarmonyPrefix]
        private static void ChangeColor(Map __instance)
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled(out CustomGridColorFeature feature))
                return;

            __instance.gridObject.transform.GetComponentInChildren<SpriteRenderer>().color = feature.Color;
        }
    }
}
