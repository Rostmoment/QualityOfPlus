using HarmonyLib;
using MTM101BaldAPI.UI;
using QualityOfPlus.Extensions;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace QualityOfPlus.BetterPause.RestartButton
{
    [HarmonyPatch]
    internal class RestartButtonPatches
    {
        private static GameObject restart;

        private static void AddRestart(PauseReset pause)
        {
            RestartButtonFeature feature = QOPManager.Instance.GetFeature<RestartButtonFeature>();
            if (pause.IsNullOrDestroyed() || !restart.IsNullOrDestroyed() || !feature.IsEnabled())
                return;

            Transform screen = pause.transform.Find("PauseScreen");
            GameObject options = screen.Find("Main").Find("OptionsButton").gameObject;
            if (options == null)
                return;

            restart = GameObject.Instantiate(options);
            GameObject.Destroy(restart.GetComponent<TextLocalizer>());
            restart.GetComponent<StandardMenuButton>().text.text = LocalizationManager.Instance.GetLocalizedText("QOP_RESTART");
            restart.transform.SetParent(screen.Find("Main"));
            restart.name = "RestartButton";

            GameObject confirm = GameObject.Instantiate(screen.Find("QuitConfirm").gameObject);
            StandardMenuButton button = restart.GetComponent<StandardMenuButton>();
            restart.GetComponent<TextLocalizer>().GetLocalizedText("QOP_RESTART");
            button.InitializeAllEvents();
            button.OnPress.AddListener(() =>
            {
                screen.Find("Main").gameObject.SetActive(false);
                confirm.SetActive(true);
            });

            restart.transform.localPosition = new Vector3(100, -64, 0);
            options.transform.localPosition = new Vector3(-100, -64, 0);


            confirm.name = "RestartConfirm";
            confirm.transform.SetParent(screen);
            confirm.transform.SetSiblingIndex(screen.Find("QuitConfirm").GetSiblingIndex());
            GameObject.Destroy(confirm.transform.Find("Text").GetComponent<TextLocalizer>());
            confirm.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = LocalizationManager.Instance.GetLocalizedText("QOP_RESTART_CONFIRM");
            confirm.transform.Find("Text").GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta += new Vector2(100, 0);


            button = confirm.transform.Find("NoButton").GetComponent<StandardMenuButton>();
            button.OnPress = new UnityEngine.Events.UnityEvent();
            button.OnPress.AddListener(() => {
                confirm.SetActive(false);
                screen.Find("Main").gameObject.SetActive(true);
            });

            button = confirm.transform.Find("YesButton").GetComponent<StandardMenuButton>();
            button.OnPress = new UnityEngine.Events.UnityEvent();
            button.OnPress.AddListener(() =>
            {
                BaseGameManager instance = BaseGameManager.Instance;
                CoreGameManager.Instance.Pause(true);
                feature.GetAction(instance.GetType())?.Invoke(instance);
            });

            confirm.transform.localPosition = Vector3.zero;
            pause.close = pause.close.AddToArray(confirm);
        }

        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Start))]
        [HarmonyPostfix]
        private static void ApplyChanges(CoreGameManager __instance)
        {
            PauseReset pause = __instance.pauseScreen.GetComponent<PauseReset>();
            if (pause != null)
                AddRestart(pause);
        }

    }
}
