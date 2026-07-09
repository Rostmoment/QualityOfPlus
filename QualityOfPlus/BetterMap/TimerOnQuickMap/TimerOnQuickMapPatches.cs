using BepInEx.Bootstrap;
using HarmonyLib;
using QualityOfPlus.ConditionalPatches;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace QualityOfPlus.BetterMap.TimerOnQuickMap
{
    // TODO: remake this code
    [HarmonyPatch(typeof(Map))]
    [QOPConditionalPatchNoMod(Compats.NO_TIMER_GUID)]
    internal class TimerOnQuickMapPatches
    {
        private static TextMeshProUGUI text;

        [HarmonyPatch(nameof(Map.Update))]
        [HarmonyPostfix]
        private static void AddTimer(Map __instance)
        {
            if (!QOPManager.Instance.GetFeature<TimerOnQuickMapFeature>().IsEnabled())
                return;

            HudManager hud = CoreGameManager.Instance.GetHud(0);
            if (hud?.itemTitle == null)
                return;

            if (text.IsNullOrDestroyed())
            {
                GameObject gameObject = GameObject.Instantiate(hud.transform.Find("Notebook Text").gameObject);
                gameObject.transform.SetParent(hud.transform, false);
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.SetSiblingIndex(hud.transform.Find("Notebook Text").GetSiblingIndex());
                gameObject.name = "Timer Text";
                text = gameObject.GetComponent<TextMeshProUGUI>();
                text.text = "00:00";
                text.rectTransform.anchoredPosition = hud.transform.Find("Notebook Text").GetComponent<RectTransform>().anchoredPosition - Vector2.up * 25;
            }
            if (!CoreGameManager.Instance.GetCamera(0).QuickMapAvailable)
            {
                text.color = Color.clear;
                return;
            }
            try
            {
                text.color = hud.itemTitle.color;
                text.text = __instance.clock.displayTime.Insert(2, ":");
            }
            catch (NullReferenceException)
            {
            }

        }
    }
}
