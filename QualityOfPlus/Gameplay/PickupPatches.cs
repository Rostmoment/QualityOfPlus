using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay
{
    [HarmonyPatch(typeof(Pickup))]
    class PickupPatches
    {
        [HarmonyPatch(nameof(Pickup.Start))]
        [HarmonyPatch(nameof(Pickup.AssignItem))]
        [HarmonyPostfix]
        private static void ShowDesc(Pickup __instance)
        {
            if (GameplayComponent.DescAnywhere)
                __instance.showDescription = true;
        }
    }
}
