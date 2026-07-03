using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode.Patches
{
    [HarmonyPatch]
    internal class NameMenuDarkMode
    {
        [HarmonyPatch(typeof(NameButton), nameof(NameButton.Unhighlight))]
        [HarmonyPatch(typeof(NameButton), nameof(NameButton.UpdateState))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(NameButton __instance)
        {
            if (!QOPManager.Instance.GetFeature<DarkModeFeature>().IsEnabled())
                return;


            __instance.text.color = Color.white;
        }

        [HarmonyPatch(typeof(NameManager), nameof(NameManager.Awake))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(NameManager __instance)
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled(out DarkModeFeature feature))
                return;

            Transform parent = __instance.transform.parent;
            Transform clipboardScreen = parent.Find("ClipboardScreen");
            clipboardScreen.Find("Image").GetComponent<Image>().color = Color.black;
            clipboardScreen.Find("BG").GetComponent<Image>().sprite = feature.NameEntryBackgroundDarkMode;
            clipboardScreen.Find("NewFileButton").GetComponent<Image>().color = Color.black;

            parent.Find("KeyboardScreen").Find("BG").GetComponent<Image>().color = Color.black;
        }
    }
}
