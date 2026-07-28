using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay.NowPlaysMusic
{
    [HarmonyPatch(typeof(MusicManager))]
    internal class NowPlaysMusicPatches
    {
        [HarmonyPatch(nameof(MusicManager.PlayMidi), new Type[] { typeof(string), typeof(float), typeof(bool) })]
        [HarmonyPrefix]
        private static void ShowNotification(string song)
        {
            NowPlaysMusicFeature feature = QOPManager.Instance.GetFeature<NowPlaysMusicFeature>();
            if (feature.IsEnabled())
                feature.ShowMusic(song);
        }
    }
}
