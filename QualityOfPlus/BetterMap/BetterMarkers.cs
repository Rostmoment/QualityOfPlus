using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap
{
    class BetterMarkers
    {
        private static KeyCode[] keyCodes = new KeyCode[]
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6
        };

        private static Vector3 WorldToMapScreenPosition(Vector3 worldPosition) =>
            new Vector3(worldPosition.x / 10f - 0.5f, worldPosition.z / 10f - 0.5f, 0f);

        [HarmonyPatch(nameof(Map.Update))]
        [HarmonyPrefix]
        private static void QuickMarkers(Map __instance)
        {
            PlayerManager pm = CoreGameManager.Instance?.GetPlayer(0);
            if (pm == null)
                return;
            Vector3 position = pm.transform.position;

            if (BetterMapComponent.RemoveMarkerEnable && Input.GetKeyDown(BetterMapComponent.RemoveMarker))
            {
                if (pm != null)
                {
                    MapMarker nearestMarker = null;
                    foreach (MapMarker mapMarker in __instance.markers)
                    {
                        if (nearestMarker == null || Vector3.Distance(pm.transform.position, mapMarker.environmentMarker.transform.position) < Vector3.Distance(pm.transform.position, nearestMarker.environmentMarker.transform.position))
                            nearestMarker = mapMarker;
                    }
                    if (nearestMarker != null)
                    {
                        nearestMarker.ShowMarker(false);
                        __instance.DestroyMarker(nearestMarker);
                    }
                }
            }

            if (BetterMapComponent.AddMarkerEnable && Input.GetKey(BetterMapComponent.AddMarker))
            {
                if (__instance.markers.Count >= 32)
                    return;

                if (pm != null)
                {
                    int id = -1;
                    for (int i = 0; i < keyCodes.Length; i++)
                    {
                        if (Input.GetKeyDown(keyCodes[i]))
                        {
                            id = i;
                            break;
                        }
                    }

                    if (id != -1)
                    {
                        __instance.AddMarker(WorldToMapScreenPosition(position), id);
                        if (__instance.environmentMarkersVisible)
                            __instance.markers.Last().ShowMarker(true);
                    }
                }
            }

            if (BetterMapComponent.AddRandomMarkerEnable && Input.GetKeyDown(BetterMapComponent.AddRandomMarker))
            {
                if (__instance.markers.Count >= 32)
                    return;

                __instance.AddMarker(WorldToMapScreenPosition(position), UnityEngine.Random.Range(0, 6));
                if (__instance.environmentMarkersVisible)
                    __instance.markers.Last().ShowMarker(true);
            }
        }
    }
}
