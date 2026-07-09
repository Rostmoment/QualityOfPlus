using HarmonyLib;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterUI.BetterStickerMenu
{
    [HarmonyPatch(typeof(StickerScreenController))]
    internal class StickerMenuPatches
    {
        [HarmonyPatch(nameof(StickerScreenController.InitializeStickers))]
        [HarmonyPrefix]
        private static void ReplaceComponent(StickerScreenController __instance)
        {
            BasePlugin.Logger.LogInfo("StickerMenuPatches.ReplaceComponent called");
            if (BaseGameManager.Instance.IsNullOrDestroyed())
                return; // Preventing changing prefabs because it will be complicated to revert back to original if feature is disabled

            BasePlugin.Logger.LogInfo("Sticker menu is not prefab");

            if (!QOPManager.Instance.GetFeatureIfEnabled<BetterStickersMenuFeature>(out _))
                return;

            BasePlugin.Logger.LogInfo("BetterStickersMenuFeature is enabled, replacing component");

            __instance.gameObject.GetOrAddComponent<NewStickerMenu>();
            __instance.inventoryStickerPrefab.gameObject.GetOrAddComponent<ExtendedInventorySticker>().InitializeExtendedPrefab(__instance.inventoryStickerPrefab);
        }

        [HarmonyPatch(nameof(StickerScreenController.UpdateStickerInventoryPositions))]
        [HarmonyPostfix]
        private static void ReplacePositions(StickerScreenController __instance)
        {
            if (__instance.transform.TryGetComponent<NewStickerMenu>(out NewStickerMenu newStickerMenu))
                newStickerMenu.SortStickers();
        }
    }
}
