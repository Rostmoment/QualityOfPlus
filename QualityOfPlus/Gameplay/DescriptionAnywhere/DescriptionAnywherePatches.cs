using HarmonyLib;
using QualityOfPlus.Interfaces;

namespace QualityOfPlus.Gameplay.DescriptionAnywhere
{
    [HarmonyPatch(typeof(Pickup))]
    internal static class DescriptionAnywherePatches
    {
        private static DescriptionAnywhereFeature Feature => QOPManager.Instance.GetFeature<DescriptionAnywhereFeature>();

        [HarmonyPatch(nameof(Pickup.Start))]
        [HarmonyPatch(nameof(Pickup.AssignItem))]
        [HarmonyPostfix]
        private static void ShowDesc(Pickup __instance)
        {
            if (Feature?.IsEnabled() == true)
                __instance.showDescription = true;
        }
    }
}