using HarmonyLib;
using QualityOfPlus.BetterMenu.DarkMode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QualityOfPlus.BetterMenu
{
    [HarmonyPatch(typeof(MainMenu))]
    class FloorSelect
    {
        [HarmonyPatch(nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void ShowButtons(MainMenu __instance)
        {
            if (!BetterMenuComponent.FloorSelect)
                return;

            __instance.StartCoroutine(Coroutine());
        }
        private static IEnumerator Coroutine()
        {
            GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            GameObject target = gameObjects.FirstOrDefault(x => x.name == "HideSeekMenu");
            while (true)
            {
                yield return null;
                target = gameObjects.FirstOrDefault(x => x.name == "HideSeekMenu");
                if (target != null)
                    break;

                gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            }

            for (int i = 2; i <= 5; i++)
                target.transform.Find($"MainNew_{i}").gameObject.SetActive(true);
        }
    }
}
