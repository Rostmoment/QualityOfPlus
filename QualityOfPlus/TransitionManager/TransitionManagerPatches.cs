using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TransitionManager
{
    [HarmonyPatch(typeof(GlobalCam))]
    internal class TransitionManagerPatches
    {
        [HarmonyPatch(nameof(GlobalCam.Transition))]
        [HarmonyPatch(nameof(GlobalCam.FadeIn))]
        [HarmonyPrefix]
        private static void ReplaceValues(ref UiTransition type, ref float duration)
        {
            TransitionManagerFeature feature = QOPManager.Instance.GetFeature<TransitionManagerFeature>();
            if (!feature.IsEnabled())
                return;

            switch (feature.Transition)
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

            duration = duration * feature.Multiplier + feature.Addend;
        }
    }
}
