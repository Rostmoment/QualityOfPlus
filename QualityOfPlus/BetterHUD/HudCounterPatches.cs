using HarmonyLib;
using QualityOfPlus.Helpers.Extensions;
using QualityOfPlus.Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterHUD
{
    [HarmonyPatch]
    internal static class HudCounterPatches
    {
        private static ElevatorsCounterFeature ElevatorsCounter =>
            QOPManager.Instance.GetFeature<ElevatorsCounterFeature>();

        private static ExtendedCounterTextFeature ExtendedText =>
            QOPManager.Instance.GetFeature<ExtendedCounterTextFeature>();

        // ── helpers ──────────────────────────────────────────────────────────────
        private static bool AllNotebooksCollected =>
            BaseGameManager.Instance.FoundNotebooks >= BaseGameManager.Instance.Ec.notebookTotal;

        private static bool ShouldShowElevators =>
            AllNotebooksCollected && ElevatorsCounter.IsEnabled() && !(BaseGameManager.Instance is EndlessGameManager);

        private static string NotebooksText()
        {
            string text = $"{BaseGameManager.Instance.FoundNotebooks}/{Mathf.Max(BaseGameManager.Instance.FoundNotebooks, BaseGameManager.Instance.Ec.notebookTotal)}";
            if (BaseGameManager.Instance is EndlessGameManager)
                text = BaseGameManager.Instance.FoundNotebooks.ToString();

            if (ExtendedText.IsEnabled())
                text += " " + LocalizationManager.Instance.GetLocalizedText("Hud_Notebooks");

            return text;
        }

        private static string ElevatorsText()
        {
            int outOfOrder = BaseGameManager.Instance.Ec.GetOutOfElevatorsCount();
            int totalOutOfOrder = BaseGameManager.Instance.Ec.GetTotalOutOfOrderElevators() + 1;
            int active = BaseGameManager.Instance.Ec.GetElevatorsCount();
            string text = $"{outOfOrder}/{totalOutOfOrder} ({active})";
            if (ExtendedText.IsEnabled())
                text += " " + LocalizationManager.Instance.GetLocalizedText("QOP_HUD_ELEVATORS");
            return text;
        }

        /// Updates the notebook icon texture when notebooks/elevators state changes.
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.UpdateNotebookText))]
        [HarmonyPrefix]
        private static void UpdateIcon(HudManager __instance)
        {
            ElevatorsCounterFeature feature = ElevatorsCounter;
            if (!feature.IsEnabled())
                return;

            Image icon = __instance.transform.Find("NotebookIcon").GetComponent<Image>();
            Graphics.CopyTexture(feature.CounterIcon(ShouldShowElevators), icon.sprite.texture);
        }

        /// Handles the standard game manager notebook collection (floor levels).
        [HarmonyPatch(typeof(Elevator), nameof(Elevator.SetState))]
        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void UpdateCounterText()
        {
            string text = ShouldShowElevators ? ElevatorsText() : NotebooksText();
            CoreGameManager.Instance.GetHud(0).UpdateNotebookText(0, text, !PlayerFileManager.Instance.authenticMode);
        }

        /// Handles the endless game manager notebook collection (no elevators).
        [HarmonyPatch(typeof(EndlessGameManager), nameof(EndlessGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void UpdateEndlessCounterText(EndlessGameManager __instance, int count)
        {
            string text = __instance.FoundNotebooks.ToString();
            if (ExtendedText.IsEnabled())
                text += " " + LocalizationManager.Instance.GetLocalizedText("Hud_Notebooks");

            CoreGameManager.Instance.GetHud(0).UpdateNotebookText(0, text, count > 0);
        }
    }
}