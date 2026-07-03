using HarmonyLib;
using MTM101BaldAPI.UI;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace QualityOfPlus.BetterSeed.CopySeed
{
    [HarmonyPatch]
    internal class CopySeedPatches
    {
        private static bool copying = false;

        [HarmonyPatch(typeof(PauseReset), nameof(PauseReset.OnEnable))]
        [HarmonyPostfix]
        private static void SeedTextInPause(PauseReset __instance)
        {
            AddAction(__instance.seedText);
        }
        [HarmonyPatch(typeof(ElevatorScreen), nameof(ElevatorScreen.AwakeFunction))]
        [HarmonyPostfix]
        private static void SeedTextInPause(ElevatorScreen __instance)
        {
            AddAction(__instance.seedText);
        }

        private static void AddAction(TMP_Text text)
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled(out CopySeedFeature feature) || text.TryGetComponent<StandardMenuButton>(out _))
                return;

            copying = false;

            StandardMenuButton button = text.gameObject.ConvertToButton<StandardMenuButton>();
            text.raycastTarget = true;
            button.OnPress.AddListener(() =>
            {
                if (feature.IsEnabled())
                {
                    MusicManager.Instance.PlaySoundEffect(feature.CopySound);
                    text.StartCoroutine(ChangeTextCoroutine(text));
                    GUIUtility.systemCopyBuffer = CoreGameManager.Instance.Seed().ToString();
                }
            });
            button.underlineOnHigh = true;
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
