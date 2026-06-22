using HarmonyLib;
using QualityOfPlus.Interfaces;

namespace QualityOfPlus.BetterUI.TABSwitch
{
    [HarmonyPatch(typeof(StandardMenuButton))]
    internal static class TABSwitchPatches
    {
        private static TABSwitchFeature Feature =>
            QOPManager.Instance.GetFeature<TABSwitchFeature>();

        [HarmonyPatch(nameof(StandardMenuButton.OnEnable))]
        [HarmonyPostfix]
        private static void Register(StandardMenuButton __instance)
        {
            if (Feature?.IsEnabled() == true)
                Feature.Register(__instance);
        }

        [HarmonyPatch(nameof(StandardMenuButton.OnDisable))]
        [HarmonyPostfix]
        private static void Unregister(StandardMenuButton __instance)
        {
            Feature?.Unregister(__instance);
        }
    }
}