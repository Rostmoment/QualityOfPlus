using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace QualityOfPlus.BetterPitstop
{
    [HarmonyPatch(typeof(PitstopGameManager))]
    class NoLoadScreen
    {

        [HarmonyPatch(nameof(PitstopGameManager.FieldTripTransition))]
        [HarmonyPrefix]
        private static bool RemoveTransition(PitstopGameManager __instance, bool entering, bool teleport)
        {
            if (!BetterPitstopComponent.NoLoadingScreen) 
                return true;

            if (entering || BetterPitstopComponent.PauseLoadMusic)
                MusicManager.Instance.StopMidi();

            if (teleport)
            {
                if (entering)
                {
                    CoreGameManager.Instance.GetPlayer(0).Teleport(__instance.currentFieldTrip.spawnPoint);
                    CoreGameManager.Instance.GetPlayer(0).transform.rotation = __instance.currentFieldTrip.spawnDirection.ToRotation();
                    Shader.SetGlobalTexture("_Skybox", __instance.currentFieldTrip.skybox);
                }
                else
                {
                    CoreGameManager.Instance.GetPlayer(0).Teleport(__instance.fieldTripExitSpawnPoint);
                    CoreGameManager.Instance.GetPlayer(0).transform.rotation = Direction.East.ToRotation();
                    Shader.SetGlobalTexture("_Skybox", CoreGameManager.Instance.sceneObject.skybox);
                }
            }

            if (!entering)
                MusicManager.Instance.PlayMidi("Elevator", loop: true);

            return false;
        }

    }
}