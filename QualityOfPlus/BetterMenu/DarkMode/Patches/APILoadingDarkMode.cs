using HarmonyLib;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode.Patches
{
    [HarmonyPatch(typeof(ModLoadingScreenManager))]
    internal class APILoadingDarkMode
    {
        [HarmonyPatch(nameof(ModLoadingScreenManager.Start))]
        [HarmonyPostfix]
        private static void FixLoadMenuScreen(ModLoadingScreenManager __instance)
        {
            if (!QOPManager.Instance.GetFeature<DarkModeFeature>().IsEnabled())
                return;

            __instance.transform.GetComponent<Image>().color = Color.black;
            __instance.modLoadText.color = Color.white;
            __instance.modIdText.color = Color.white;
            __instance.apiLoadText.color = Color.white;
            __instance.transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }
}
