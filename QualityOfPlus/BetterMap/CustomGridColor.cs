using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap
{
    [HarmonyPatch(typeof(Map))]
    class CustomGridColor
    {
        [HarmonyPatch(nameof(Map.OpenMap))]
        [HarmonyPrefix]
        private static void ChangeColor(Map __instance)
        {
            __instance.gridObject.transform.GetComponentInChildren<SpriteRenderer>().color = BetterMapComponent.CustomGridColor;
        }
    }
}
