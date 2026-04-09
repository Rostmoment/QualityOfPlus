using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterHUD
{
    [HarmonyPatch]
    internal class BetterCounter
    {

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.UpdateNotebookText))]
        [HarmonyPrefix]
        private static void Icon(HudManager __instance, int textVal)
        {
            if (!BetterHUDComponent.ElevatorsCounter)
                return;

            Image img = __instance.transform.Find("NotebookIcon").GetComponent<Image>();
            if (BaseGameManager.Instance.FoundNotebooks<BaseGameManager.Instance.Ec.notebookTotal || BaseGameManager.Instance is EndlessGameManager)
                Graphics.CopyTexture(BasePlugin.Asset.Get<Texture2D>("NotebooksCounterIconSheet"), img.sprite.texture);
            else
                Graphics.CopyTexture(BasePlugin.Asset.Get<Texture2D>("ElevatorsCounterIconSheet"), img.sprite.texture);
            
        }

        [HarmonyPatch(typeof(EndlessGameManager), nameof(EndlessGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void Text(EndlessGameManager __instance, int count)
        {
            string text = __instance.FoundNotebooks.ToString();

            if (BetterHUDComponent.ExtendedCounterText)
                text += " " + LocalizationManager.Instance.GetLocalizedText("Hud_Notebooks");

            Singleton<CoreGameManager>.Instance.GetHud(0).UpdateNotebookText(0, text, count > 0);
        }

        [HarmonyPatch(typeof(Elevator), nameof(Elevator.SetState))]
        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void Text()
        {
            string text = "";
            if (BaseGameManager.Instance.FoundNotebooks < BaseGameManager.Instance.Ec.notebookTotal || !BetterHUDComponent.ElevatorsCounter)
            {
                text = string.Concat(new string[]
                {
                    BaseGameManager.Instance.FoundNotebooks.ToString(),
                    "/",
                    Mathf.Max(BaseGameManager.Instance.FoundNotebooks, BaseGameManager.Instance.Ec.notebookTotal).ToString(),
                });
                if (BetterHUDComponent.ExtendedCounterText)
                    text += " " + LocalizationManager.Instance.GetLocalizedText("Hud_Notebooks");
            }
            else if (BetterHUDComponent.ElevatorsCounter)
            {
                text = string.Concat(new string[]
                {
                    BaseGameManager.Instance.ec.GetOutOfElevatorsCount().ToString(),
                    "/",
                    (BaseGameManager.Instance.Ec.GetTotalOutOfOrderElevators()+1).ToString(),
                    " (",
                    BaseGameManager.Instance.ec.GetElevatorsCount().ToString(),
                    ")"

                });
                if (BetterHUDComponent.ExtendedCounterText)
                    text += " " + LocalizationManager.Instance.GetLocalizedText("HUD_Elevators");
            }
            CoreGameManager.Instance.GetHud(0).UpdateNotebookText(0, text, !PlayerFileManager.Instance.authenticMode);
        }
    }
}
