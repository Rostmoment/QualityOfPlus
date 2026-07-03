using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.MoreIcons.PickupIcons
{
    [HarmonyPatch(typeof(Pickup))]
    internal class PickupPatches
    {

        [HarmonyPatch(nameof(Pickup.Start))]
        [HarmonyPatch(nameof(Pickup.AssignItem))]
        [HarmonyPostfix]
        private static void AddIcons(Pickup __instance)
        {
            if (__instance?.icon?.spriteRenderer == null)
                return;

            if (QOPManager.Instance.GetFeatureIfEnabled<YTPIconFeature>(out YTPIconFeature ytp) && __instance.item.itemType == Items.Points)
                __instance.icon.spriteRenderer.sprite = ytp.Icon;

            if (QOPManager.Instance.GetFeatureIfEnabled<StickerIconFeature>(out StickerIconFeature sticker) && __instance.item.itemType == Items.StickerPack)
                __instance.icon.spriteRenderer.sprite = sticker.Icon;
        }
    }
}
