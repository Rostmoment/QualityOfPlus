using HarmonyLib;
using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.Gameplay.GameplayHistory
{
    [HarmonyPatch]
    internal class GameplayHistoryPatches
    {
        [HarmonyPatch(typeof(HideSeekMenu), nameof(HideSeekMenu.Awake))]
        [HarmonyPostfix]
        private static void AddButton(HideSeekMenu __instance)
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled<GameplayHistoryFeature>(out GameplayHistoryFeature feature))
                return;

            StandardMenuButton button = GameObject.Instantiate(__instance.transform.Find("BackButton").GetComponent<StandardMenuButton>());
            button.eventOnHigh = true;
            button.InitializeAllEvents();

            button.highlightedSprite = feature.Highlighted;
            button.unhighlightedSprite = feature.Unhighlighted;
            button.image.sprite = feature.Unhighlighted;

            button.OnHighlight.AddListener(() =>
            {
                __instance.GetComponent<TooltipController>().UpdateTooltip("QOP_ABOUT_HISTORY");
            });
            button.OffHighlight.AddListener(() =>
            {
                __instance.GetComponent<TooltipController>().CloseTooltip();
            });

            button.transform.SetParent(__instance.transform, false);
            button.transform.localPosition = new Vector3(100, 128, 0);

            button.transform.SetSiblingIndex(__instance.transform.Find("BackButton").GetSiblingIndex());
        }


        [HarmonyPatch(typeof(GameLoader), nameof(GameLoader.LoadLevel))]
        [HarmonyPrefix]
        private static void SaveData(SceneObject sceneObject)
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled<GameplayHistoryFeature>(out GameplayHistoryFeature feature))
                return;

            GameplayHistoryStorage.AddEntry(GameplayHistoryEntry.CreateNow(CoreGameManager.Instance.Seed(), sceneObject));
        }
    }
}
