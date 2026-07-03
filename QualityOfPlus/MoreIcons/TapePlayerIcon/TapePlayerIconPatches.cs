using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.MoreIcons.TapePlayerIcon
{

    [HarmonyPatch(typeof(BaseGameManager))]
    internal class TapePlayerIconPatches
    {

        [HarmonyPatch(nameof(BaseGameManager.ApplyMap))]
        [HarmonyPostfix]
        private static void AddIcons(Map map)
        {
            if (QOPManager.Instance.GetFeatureIfEnabled<TapePlayerIconFeature>(out TapePlayerIconFeature tape))
            {

                foreach (TapePlayer tapePlayer in UnityEngine.Object.FindObjectsOfType<TapePlayer>())
                {
                    if (tapePlayer != null)
                    {
                        if (tapePlayer.requiredItem == Items.Tape)
                            map.AddIcon(tape.Icon, tapePlayer.transform, Color.white);
                    }
                }
            }
        }
    }
}
