using HarmonyLib;

namespace QualityOfPlus.BetterPause
{
    [HarmonyPatch]
    internal static class PauseWithoutScreenPatches
    {
        private static PauseWithoutScreenFeature Feature => QOPManager.Instance.GetFeature<PauseWithoutScreenFeature>();

        [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.StopRendering))]
        [HarmonyPrefix]
        private static bool InterceptStopRendering(GameCamera __instance, bool val)
        {
            PauseWithoutScreenFeature feature = Feature;
            if (feature == null)
                return true;

            if (val && feature.PauseNoScreen)
            {
                feature.PauseNoScreen = false;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Pause))]
        [HarmonyPostfix]
        private static void FixMyStupidBug() => CoreGameManager.Instance?.GetHud(0)?.CloseTooltip();
    }
}