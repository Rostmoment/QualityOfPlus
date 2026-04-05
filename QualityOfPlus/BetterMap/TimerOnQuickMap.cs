using BepInEx.Bootstrap;
using HarmonyLib;
using Rewired.Utils.Classes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

namespace QualityOfPlus.BetterMap
{
    [HarmonyPatch(typeof(Map))]
    class TimerOnQuickMap
    {
        private static GameObject timerText;
        private static TextMeshProUGUI text;

        [HarmonyPatch(nameof(Map.Update))]
        [HarmonyPostfix]
        private static void AddTimer(Map __instance)
        {
            if (!BetterMapComponent.TimerOnQuickMap)
                return;

            HudManager hud = CoreGameManager.Instance.GetHud(0);
            if (hud?.itemTitle == null)
                return;

            if (timerText.IsNullOrDestroyed())
            {
                timerText = GameObject.Instantiate(hud.transform.Find("Notebook Text").gameObject);
                timerText.transform.SetParent(hud.transform, false);
                timerText.transform.localScale = Vector3.one;
                timerText.transform.SetSiblingIndex(hud.transform.Find("Notebook Text").GetSiblingIndex());
                timerText.name = "Timer Text";
                text = timerText.GetComponent<TextMeshProUGUI>();
                text.text = "00:00";
                text.rectTransform.anchoredPosition = hud.transform.Find("Notebook Text").GetComponent<RectTransform>().anchoredPosition - Vector2.up * 25;
            }
            if (Chainloader.PluginInfos.ContainsKey("rost.moment.baldiplus.notimer") || !Singleton<CoreGameManager>.Instance.GetCamera(0).QuickMapAvailable)
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
