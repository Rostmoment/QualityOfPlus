using HarmonyLib;
using MTM101BaldAPI.UI;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace QualityOfPlus.BetterPause.CopySeed
{
    [HarmonyPatch]
    internal class CopySeedPatches
    {
        private static bool copying = false;

        [HarmonyPatch(typeof(PauseReset), nameof(PauseReset.OnEnable))]
        [HarmonyPostfix]
        private static void SeedText(PauseReset __instance)
        {
            CopySeedFeature feature = QOPManager.Instance.GetFeature<CopySeedFeature>();
            if (feature.IsEnabled() && !__instance.seedText.TryGetComponent<StandardMenuButton>(out _))
            {
                copying = false;
                StandardMenuButton button = __instance.seedText.gameObject.ConvertToButton<StandardMenuButton>();
                __instance.seedText.raycastTarget = true;
                button.OnPress.AddListener(() =>
                {
                    if (feature.IsEnabled())
                    {
                        MusicManager.Instance.PlaySoundEffect(feature.CopySound);
                        __instance.StartCoroutine(ChangeTextCoroutine(__instance.seedText));
                        GUIUtility.systemCopyBuffer = CoreGameManager.Instance.Seed().ToString();
                    }
                });
                button.underlineOnHigh = true;
            }
        }

        private static IEnumerator ChangeTextCoroutine(TMP_Text text)
        {
            if (copying)
                yield break;

            copying = true;
            string saved = text.text;
            text.text = LocalizationManager.Instance.GetLocalizedText("QOP_SEED_COPIED");
            yield return new WaitForSecondsRealtime(3);
            text.text = saved;
            copying = false;
        }
    }
}
