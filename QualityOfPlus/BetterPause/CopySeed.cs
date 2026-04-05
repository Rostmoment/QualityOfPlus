using HarmonyLib;
using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterPause
{
    [HarmonyPatch]
    internal class CopySeed
    {

        [HarmonyPatch(typeof(PauseReset), nameof(PauseReset.OnEnable))]
        [HarmonyPostfix]
        private static void SeedText(PauseReset __instance)
        {
            if (BetterPauseComponent.EnableCopySeedFunction && !__instance.seedText.TryGetComponent<StandardMenuButton>(out _))
            {
                StandardMenuButton button = __instance.seedText.gameObject.ConvertToButton<StandardMenuButton>();
                __instance.seedText.raycastTarget = true;
                button.OnPress.AddListener(() =>
                {
                    GUIUtility.systemCopyBuffer = CoreGameManager.Instance.Seed().ToString();
                });
                button.underlineOnHigh = true;
            }
        }
    }
}
