using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterElevator
{
    [HarmonyPatch(typeof(PitstopGameManager))]
    class PitstopTrigger
    {
        [HarmonyPatch(nameof(PitstopGameManager.Initialize))]
        [HarmonyPostfix]
        private static void ReplaceGreenButton(PitstopGameManager __instance)
        {
            if (!BetterElevatorComponent.PitstopTrigger)
                return;

            Elevator elevator = __instance.ec.Elevators[0];

            elevator.button.gameObject.SetActive(false);
            elevator.insideCollider.gameObject.AddComponent<ElevatorExitTrigger>().SetElevator(elevator);
        }
    }
}
