using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterPause
{
    [HarmonyPatch]
    class PauseNoScreen
    {
        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.CameraShutoffDelay), MethodType.Enumerator)]
        [HarmonyPrefix]
        private static bool ReturnFalse() => false;

        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Update))]
        [HarmonyPostfix]
        private static void PauseWithNoScreen()
        {
            if (BetterPauseComponent.EnablePauseWithoutScreen && Input.GetKeyDown(BetterPauseComponent.PauseWithoutScreen))
            {
                if (CoreGameManager.Instance.disablePause || GlobalCam.Instance.TransitionActive)
                    return;

                if (CoreGameManager.Instance.Paused)
                {
                    CoreGameManager.Instance.Pause(false);
                    return;
                }

                CoreGameManager.Instance.Pause(false);
                if (CoreGameManager.Instance.Paused)
                    CoreGameManager.Instance.GetHud(0).SetTooltip(LocalizationManager.Instance.GetLocalizedText("PausedWithoutScreen"));
            }
        }
        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Pause))]
        [HarmonyPostfix]
        private static void FixMyStupidBug() => CoreGameManager.Instance?.GetHud(0)?.CloseTooltip();
    }
}
