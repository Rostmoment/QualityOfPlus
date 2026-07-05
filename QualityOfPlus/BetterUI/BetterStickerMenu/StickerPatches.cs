using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterUI.BetterStickerMenu
{
    [HarmonyPatch(typeof(InventorySticker))]
    internal class StickerPatches
    {
        [HarmonyPatch(nameof(InventorySticker.Initialize))]
        [HarmonyPrefix]
        private static void InitializePatch(InventorySticker __instance)
        {
            if (__instance.TryGetComponent<ExtendedInventorySticker>(out ExtendedInventorySticker extendedSticker))
                extendedSticker.Initialize();
        }

        [HarmonyPatch(nameof(InventorySticker.SetValue))]
        [HarmonyPrefix]
        private static bool SetValuePatch(InventorySticker __instance, int value)
        {
            if (__instance.TryGetComponent<ExtendedInventorySticker>(out ExtendedInventorySticker extendedSticker))
            {
                extendedSticker.SetValue(value);
                return false;
            }
            return true;
        }

    }
}
