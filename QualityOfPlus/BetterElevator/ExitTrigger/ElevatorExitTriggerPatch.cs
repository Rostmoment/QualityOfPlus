using HarmonyLib;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator.ExitTrigger
{
    [HarmonyPatch(typeof(Elevator))]
    internal class ElevatorExitTriggerPatch
    {
        [HarmonyPatch(nameof(Elevator.Initialize))]
        [HarmonyPostfix]
        private static void ReplaceGreenButton(Elevator __instance)
        {
            ElevatorExitTriggerFeature feature = QOPManager.Instance.GetFeature<ElevatorExitTriggerFeature>();
            if (!feature.IsEnabled() || !feature.IsTriggerFor(BaseGameManager.Instance.GetType()))
                return;

            __instance.button.gameObject.SetActive(false);
            __instance.insideCollider.gameObject.AddComponent<ElevatorExitTriggerComponent>().SetElevator(__instance);
        }
    }
}
