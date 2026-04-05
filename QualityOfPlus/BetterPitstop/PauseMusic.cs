using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace QualityOfPlus.BetterPitstop
{
    [HarmonyPatch(typeof(PitstopGameManager))]
    class PauseMusic
    {

        [HarmonyPatch(nameof(PitstopGameManager.FieldTripTransition))]
        [HarmonyPrefix]
        private static void Pause()
        {
            if (BetterPitstopComponent.PauseLoadMusic)
                MusicManager.Instance.StopMidi();
        }

    }
}