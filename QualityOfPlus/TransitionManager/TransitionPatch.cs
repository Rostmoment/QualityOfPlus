using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TransitionManager
{
    [HarmonyPatch(typeof(GlobalCam))]
    class TransitionPatch
    {
        [HarmonyPatch(nameof(GlobalCam.Transition))]
        [HarmonyPatch(nameof(GlobalCam.FadeIn))]
        [HarmonyPrefix]
        private static void ReplaceValues(ref UiTransition type, ref float duration)
        {
            switch (TransitionManagerComponent.Transition)
            {
                case CustomUiTransition.SwipeLeft:
                    type = UiTransition.SwipeLeft;
                    break;
                case CustomUiTransition.SwipeRight:
                    type = UiTransition.SwipeRight;
                    break;
                case CustomUiTransition.Dither:
                    type = UiTransition.Dither;
                    break;
                default:
                    break;
            }

            duration = duration * TransitionManagerComponent.Multiplier + TransitionManagerComponent.Addend;
        }
    }
}
