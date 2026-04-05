using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.UI;
using QualityOfPlus.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace QualityOfPlus.BetterPause
{
    [HarmonyPatch]
    class RestartButton
    {
        private static GameObject restart;


        private static void AddResetButton(PauseReset pause)
        {
            if ((restart.IsNullOrDestroyed() || !pause.main.Contains(restart)) && BetterPauseComponent.EnableRestartButton)
            {
                Transform screen = pause.transform.Find("PauseScreen");
                GameObject options = screen.Find("Main").Find("OptionsButton").gameObject;
                if (options == null)
                    return;

                restart = GameObject.Instantiate(options);
                restart.AddComponent<MonoBehaviourBuilder>().SetOnUpdate(x => x.gameObject.GetComponent<StandardMenuButton>().text.text = LocalizationManager.Instance.GetLocalizedText("Restart"));
                restart.transform.SetParent(screen.Find("Main"));
                restart.name = "RestartButton";

                GameObject confirm = GameObject.Instantiate(screen.Find("QuitConfirm").gameObject);
                StandardMenuButton button = restart.GetComponent<StandardMenuButton>();
                restart.GetComponent<TextLocalizer>().GetLocalizedText("Restart");
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

                confirm.transform.Find("Text").gameObject.AddComponent<MonoBehaviourBuilder>().SetOnStart(x => {
                    TextMeshProUGUI text = x.GetComponent<TextMeshProUGUI>();
                    text.text = LocalizationManager.Instance.GetLocalizedText("RestartConfirm");
                    text.rectTransform.sizeDelta += new Vector2(100, 0);
                });


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
                    CoreGameManager.Instance.Pause(true);
                    Transform t = CoreGameManager.Instance.GetPlayer(0).transform;
                    if (BaseGameManager.Instance is TutorialGameManager)
                        CoreGameManager.Instance.Quit();
                    else if (BaseGameManager.Instance is PitstopGameManager pit)
                        pit.LoadNextLevel();
                    else
                        CoreGameManager.Instance.EndGame(t, t);
                });

                pause.close = pause.close.AddToArray(confirm);
            }
        }


        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Start))]
        [HarmonyPostfix]
        private static void ApplyChanges(CoreGameManager __instance)
        {
            PauseReset pause = __instance.pauseScreen.GetComponent<PauseReset>();
            if (pause != null)
                AddResetButton(pause);
        }

        [HarmonyPatch(typeof(PauseReset), nameof(PauseReset.OnEnable))]
        [HarmonyPostfix]
        private static void UpdatePosition(PauseReset __instance)
        {
            Transform confirm = __instance.transform.Find("PauseScreen").Find("RestartConfirm");
            if (confirm != null)
                confirm.localPosition = Vector3.zero;
            
        }
    }
}
