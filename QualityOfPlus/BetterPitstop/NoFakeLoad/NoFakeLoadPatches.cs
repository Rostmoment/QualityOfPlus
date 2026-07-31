using HarmonyLib;
using QualityOfPlus.Helpers.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterPitstop.NoFakeLoad
{
    [HarmonyPatch(typeof(PitstopGameManager))]
    internal class NoFakeLoadPatches
    {
        [HarmonyPatch(nameof(PitstopGameManager.FieldTripTransition))]
        [HarmonyPrefix]
        private static bool ChangeTransition(PitstopGameManager __instance, ref IEnumerator __result, bool entering, bool teleport)
        {
            if (QOPManager.Instance.GetFeatureIfEnabled<NoFakeLoadFeature>(out _))
            {
                __result = InstaTransition(__instance, entering, teleport, __instance.currentFieldTrip, __instance.fieldTripExitSpawnPoint);
                return false;
            }
            return true;
        }

        private static IEnumerator InstaTransition(PitstopGameManager __instance, bool entering, bool teleport, FieldTripObject currentFieldTrip, Vector3 fieldTripExitSpawnPoint)
        {
            yield return null;

            if (teleport)
            {
                if (entering)
                {
                    Singleton<CoreGameManager>.Instance.GetPlayer(0).Teleport(currentFieldTrip.spawnPoint);
                    Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.rotation = currentFieldTrip.spawnDirection.ToRotation();
                    Shader.SetGlobalTexture("_Skybox", currentFieldTrip.skybox);
                }
                else
                {
                    Singleton<CoreGameManager>.Instance.GetPlayer(0).Teleport(fieldTripExitSpawnPoint);
                    Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.rotation = Direction.East.ToRotation();
                    Shader.SetGlobalTexture("_Skybox", Singleton<CoreGameManager>.Instance.sceneObject.skybox);
                }
            }

            if (!__instance.tripScreen.IsNullOrDestroyed())
                GameObject.Destroy(__instance.tripScreen.gameObject);
        }
    }
}
