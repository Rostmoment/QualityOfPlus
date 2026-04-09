using HarmonyLib;
using UnityEngine;

namespace QualityOfPlus.BetterPause
{
    [HarmonyPatch]
    class PauseNoScreen
    {
        internal static bool pauseNoScreen = false;

        [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.StopRendering))]
        [HarmonyPrefix]
        private static bool InterceptStopRendering(GameCamera __instance, bool val)
        {
            if (val && pauseNoScreen)
            {
                pauseNoScreen = false;
                return false; 
            }
            return true;
        }

        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Update))]
        [HarmonyPostfix]
        private static void PauseWithNoScreen()
        {
            if (!BetterPauseComponent.EnablePauseWithoutScreen || !Input.GetKeyDown(BetterPauseComponent.PauseWithoutScreen))
                return;

            if (CoreGameManager.Instance.disablePause || GlobalCam.Instance.TransitionActive)
                return;

            if (CoreGameManager.Instance.Paused)
            {
                CoreGameManager.Instance.Pause(false);
                return;
            }

            pauseNoScreen = true;
            CoreGameManager.Instance.Pause(false);

            if (CoreGameManager.Instance.Paused)
                CoreGameManager.Instance.GetHud(0).SetTooltip(LocalizationManager.Instance.GetLocalizedText("PausedWithoutScreen"));
        }

        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Pause))]
        [HarmonyPostfix]
        private static void FixMyStupidBug() => CoreGameManager.Instance?.GetHud(0)?.CloseTooltip();
    }
}