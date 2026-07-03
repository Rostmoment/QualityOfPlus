using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode.Patches
{
    [HarmonyPatch]
    internal class WinningScreensDarkMode
    {
        [HarmonyPatch(typeof(TutorialGameManager), nameof(TutorialGameManager.LoadNextLevel))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(TutorialGameManager __instance)
        {
            if (!QOPManager.Instance.GetFeature<DarkModeFeature>().IsEnabled())
                return;

            __instance.resultsScreen.transform.Find("BG").GetComponent<Image>().color = UnityEngine.Color.black;
        }


        [HarmonyPatch(typeof(ChallengeWin), nameof(ChallengeWin.Start))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(ChallengeWin __instance)
        {
            if (!QOPManager.Instance.GetFeature<DarkModeFeature>().IsEnabled())
                return;

            Transform canvas = __instance.transform.Find("Canvas");
            canvas.Find("Image").GetComponent<Image>().sprite = BasePlugin.Asset.Get<Sprite>("ChallengeWinDarkMode");
            canvas.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }
}
