using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode
{
    [HarmonyPatch]
    class NameMenuDarkMode
    {
        [HarmonyPatch(typeof(NameButton), nameof(NameButton.Unhighlight))]
        [HarmonyPatch(typeof(NameButton), nameof(NameButton.UpdateState))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(NameButton __instance)
        {
            if (!BetterMenuComponent.DarkMode) 
                return;


            __instance.text.color = Color.white;
        }

        [HarmonyPatch(typeof(NameManager), nameof(NameManager.Awake))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(NameManager __instance)
        {
            if (!BetterMenuComponent.DarkMode) 
                return;

            Transform parent = __instance.transform.parent;
            Transform clipboardScreen = parent.Find("ClipboardScreen");
            clipboardScreen.Find("Image").GetComponent<Image>().color = Color.black;
            clipboardScreen.Find("BG").GetComponent<Image>().sprite = BasePlugin.Asset.Get<Sprite>("NameEntryDarkModeBG");
            clipboardScreen.Find("NewFileButton").GetComponent<Image>().color = Color.black;

            parent.Find("KeyboardScreen").Find("BG").GetComponent<Image>().color = Color.black;
        }
    }
}
