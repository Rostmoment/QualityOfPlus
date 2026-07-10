using HarmonyLib;
using QualityOfPlus.Helpers.Extensions;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace QualityOfPlus.BetterNameMenu.BackToNameMenu
{
    [HarmonyPatch(typeof(MainMenu))]
    internal class BackToNameMenuPatches
    {
        private static StandardMenuButton back;

        private static IEnumerator AddBackButton(MainMenu __instance)
        {
            GameObject button = null;
            while (button == null)
            {
                yield return null;
                GameObject prefab = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.name == "PickMode").transform.Find("BackButton").gameObject;
                if (prefab == null)
                    continue;

                button = GameObject.Instantiate(prefab);
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
            button.transform.localPosition = new Vector3(-240, 180, 0);
            button.transform.localScale = Vector3.one;
            button.transform.SetSiblingIndex(__instance.transform.Find("Play").GetSiblingIndex());
        }


        [HarmonyPatch(nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void StartCoroutine(MainMenu __instance)
        {
            if (!back.IsNullOrDestroyed() || !QOPManager.Instance.GetFeature<BackToNameMenuFeature>().IsEnabled())
                return;

            __instance.StartCoroutine(AddBackButton(__instance));

        }
    }
}
