using HarmonyLib;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterElevator.QuickTV
{
    [HarmonyPatch(typeof(ElevatorScreen))]
    internal class QuickElevatorTVPatches 
    {
        [HarmonyPatch(nameof(ElevatorScreen.ShowResults))]
        [HarmonyPrefix]
        private static bool SpeedUp(ElevatorScreen __instance)
        {
            return !QOPManager.Instance.GetFeature<QuickElevatorTVFeature>().IsEnabled();
        }
    }
}
