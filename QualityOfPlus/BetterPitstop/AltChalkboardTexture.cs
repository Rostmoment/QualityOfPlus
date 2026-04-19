using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterPitstop
{
    [HarmonyPatch(typeof(PitstopGameManager))]
    class AltChalkboardTexture
    {
        private static void ChangeTexture(PosterObject posterObj)
        {
            posterObj.baseTexture = BasePlugin.Asset.Get<Texture2D>("AltChalkboardPit");
            foreach (PosterObject poster in posterObj.multiPosterArray)
                ChangeTexture(poster);
        }

        [HarmonyPatch(nameof(PitstopGameManager.Initialize))]
        [HarmonyPrefix]
        private static void ChangeTexture(PitstopGameManager __instance)
        {
            if (BetterPitstopComponent.AltChalkboardTexture)
            {
                foreach (PosterObject poster in __instance.levelTypePoster)
                    ChangeTexture(poster);
            }
        }
    }
}
