using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using QualityOfPlus.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QualityOfPlus.BetterNameMenu
{
    [HarmonyPatch(typeof(MainMenu))]
    class BackToNameMenu
    {
        private static StandardMenuButton back;
        private static IEnumerator AddBackButton(MainMenu __instance)
        {
            GameObject button;
            while (true)
            {
                yield return null;
                try
                {
                    button = GameObject.Instantiate(SceneManager.GetActiveScene().GetRootGameObjects().Find(x => x.name == "PickMode").transform.Find("BackButton").gameObject);
                    break;
                }
                catch (NullReferenceException)
                {
                }
            }

            button.transform.SetParent(__instance.transform);
            back = button.GetComponent<StandardMenuButton>();
            back.OnPress = new UnityEngine.Events.UnityEvent();
            back.OnPress.AddListener(() =>
            {
                GlobalStateManager.Instance.skipNameEntry = false;
                MusicManager.Instance.StopMidi();
                SceneManager.LoadScene("MainMenu");
            });
            button.AddComponent<MonoBehaviourBuilder>().SetOnStart(x =>
            {
                x.transform.localPosition = new Vector3(-240, 180, 0);
                x.transform.localScale = Vector3.one;
            });
            button.transform.SetSiblingIndex(__instance.transform.Find("Play").GetSiblingIndex());
        }

        [HarmonyPatch(nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void StartCoroutine(MainMenu __instance)
        {
            if (!back.IsNullOrDestroyed() || !BetterNameMenuComponent.BackButton)
                return;

            __instance.StartCoroutine(AddBackButton(__instance));
       
        }
    }
}
