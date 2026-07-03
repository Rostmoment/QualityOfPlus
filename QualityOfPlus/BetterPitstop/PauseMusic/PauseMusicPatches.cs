using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterPitstop.PauseMusic
{
    [HarmonyPatch(typeof(PitstopGameManager))]
    internal class PauseMusicPatches
    {
        [HarmonyPatch(nameof(PitstopGameManager.FieldTripTransition))]
        [HarmonyPrefix]
        private static void StopMusic()
        {
            if (QOPManager.Instance.GetFeatureIfEnabled<PauseMusicFeature>(out _))
                MusicManager.Instance.StopMidi();
        }
    }
}
