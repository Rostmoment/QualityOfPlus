using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace QualityOfPlus.DarkMode
{
    [HarmonyPatch(typeof(MainMenu))]
    class MainMenuDarkMode
    {
        private static bool IsWhite(Color c)
        {
            float luminance = c.r * 0.299f + c.g * 0.587f + c.b * 0.114f;

            bool nearGrey =
                Mathf.Abs(c.r - c.g) < 0.1f &&
                Mathf.Abs(c.r - c.b) < 0.1f &&
                Mathf.Abs(c.g - c.b) < 0.1f;

            return luminance > 0.7f && nearGrey;
        }
        private static GameObject[] gameObjects = new GameObject[] { };

        [HarmonyPatch(nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void ApplyDarkMode(MainMenu __instance)
        {
            if (!DarkModeComponent.DarkMode) 
                return;

            __instance.transform.Find("Image").GetComponent<Image>().sprite = BasePlugin.Asset.Get<Sprite>("MainMenuDarkMode");
            StandardMenuButton button = __instance.transform.Find("Exit").GetComponent<StandardMenuButton>();
            button.unhighlightedSprite = BasePlugin.Asset.Get<Sprite>("ExitNotHighlitghedDarkMode");
            button.highlightedSprite = BasePlugin.Asset.Get<Sprite>("ExitHighlitghedDarkMode");
            button.Highlight();
            button.UnHighilight();

            __instance.transform.Find("Version").GetComponent<TextMeshProUGUI>().color = Color.white;
            __instance.transform.Find("ChangelogButton").GetComponent<Image>().color = Color.black;

            /*
            button = __instance.transform.Find("Play").GetComponent<StandardMenuButton>();
            button.unhighlightedSprite = BasePlugin.Asset.Get<Sprite>("PlayNotHighlitghedDarkMode");
            button.highlightedSprite = BasePlugin.Asset.Get<Sprite>("PlayHighlitghedDarkMode");
            button.Highlight();
            button.UnHighilight();

            button = __instance.transform.Find("Options").GetComponent<StandardMenuButton>();
            button.unhighlightedSprite = BasePlugin.Asset.Get<Sprite>("PlayNotHighlitghedDarkMode");
            button.highlightedSprite = BasePlugin.Asset.Get<Sprite>("PlayHighlitghedDarkMode");
            button.Highlight();
            button.UnHighilight();*/


            foreach (StandardMenuButton btn in __instance.GetComponentsInChildren<StandardMenuButton>())
            {
                if (btn.unhighlightedSprite == null || btn.highlightedSprite == null || btn.name == "Exit")
                    continue;

                Texture2D unTex = btn.unhighlightedSprite.texture.MakeReadableCopy(true);
                Texture2D hiTex = btn.highlightedSprite.texture.MakeReadableCopy(true);

                Color[] unhighlighted = unTex.GetPixels();
                Color[] highlighted = hiTex.GetPixels();

                for (int i = 0; i < unhighlighted.Length; i++)
                {
                    Color c = unhighlighted[i];
                    if (IsWhite(c))
                        c = Color.black;

                    unhighlighted[i] = c;
                }

                for (int i = 0; i < highlighted.Length; i++)
                {
                    Color c = highlighted[i];
                    if (IsWhite(c))
                        c = Color.black;

                    highlighted[i] = c;
                }

                Texture2D newUn = new Texture2D(unTex.width, unTex.height, TextureFormat.RGBA32, false);
                Texture2D newHi = new Texture2D(hiTex.width, hiTex.height, TextureFormat.RGBA32, false);

                newUn.SetPixels(unhighlighted);
                newUn.Apply();
                newHi.SetPixels(highlighted);
                newHi.Apply();

                btn.unhighlightedSprite = AssetLoader.SpriteFromTexture2D(newUn, 1);
                btn.highlightedSprite = AssetLoader.SpriteFromTexture2D(newHi, 1);

                btn.Highlight();
                btn.UnHighilight();
            }


            gameObjects = new GameObject[] { };
            __instance.StartCoroutine(General(__instance));
        }

        private static IEnumerator General(MainMenu __instance)
        {
            Coroutine[] coroutines = new Coroutine[]
            {
                __instance.StartCoroutine(ChangePickMenu()),
                __instance.StartCoroutine(ChangeMenu("About")),
                __instance.StartCoroutine(ChangeMenu("HideSeekMenu")),
                __instance.StartCoroutine(ChangeMenu("PickChallenge")),
                __instance.StartCoroutine(ChangeMenu("PickFieldTrip")),
                __instance.StartCoroutine(ChangeMenu("PickEndlessMap")),
                __instance.StartCoroutine(ChangeEndlessMenu()),
            };
            while (coroutines.Any(x => x != null))
            {
                yield return null;
                gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            }
        }
        private static IEnumerator ChangeMenu(string menu)
        {
            GameObject target = gameObjects.Find(x => x.name == menu);
            while (true)
            {
                yield return null;
                target = gameObjects.Find(x => x.name == menu);
                if (target != null)
                    break;
            }

            target.transform.Find("BG").GetComponent<Image>().color = Color.black;

            foreach (TextMeshProUGUI text in target.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (text.transform.parent.name != "Tooltip" && text.color == Color.black)
                    text.color = Color.white;
            }

            foreach (Transform t in target.transform)
                ChangeMenuToggleColor(t);

            yield break;
        }
        private static IEnumerator ChangePickMenu()
        {
            GameObject target = gameObjects.Find(x => x.name == "PickMode");
            while (true)
            {
                yield return null;
                target = gameObjects.Find(x => x.name == "PickMode");
                if (target != null)
                    break;
            }

            target.transform.Find("TutorialPrompt").Find("BG").GetComponent<Image>().color = Color.black;
            target.transform.Find("BG").GetComponent<Image>().color = Color.black;

            foreach (TextMeshProUGUI text in target.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (text.color == Color.black)
                    text.color = Color.white;
            }

            foreach (Transform t in target.transform)
                ChangeMenuToggleColor(t);

            yield break;

        }

        private static IEnumerator ChangeEndlessMenu()
        {
            GameObject target = gameObjects.Find(x => x.name == "EndlessMapOverview");
            while (true)
            {
                yield return null;
                target = gameObjects.Find(x => x.name == "EndlessMapOverview");
                if (target != null)
                    break;
            }

            target.transform.Find("BG").GetComponent<Image>().color = Color.black;

            foreach (TextMeshProUGUI text in target.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (text.color == Color.black)
                    text.color = Color.white;
            }

            foreach (HighScoreListing listing in target.GetComponentsInChildren<HighScoreListing>())
                listing.transform.Find("Button").GetComponent<Image>().color = Color.black;

            foreach (Transform t in target.transform)
                ChangeMenuToggleColor(t);

            yield break;
        }

        public static void ChangeMenuToggleColor(Transform transform)
        {
            if (transform.TryGetComponent<MenuToggle>(out MenuToggle menuToggle))
            {
                Transform box = menuToggle.transform.Find("Box");
                if (box != null && box.TryGetComponent<Image>(out Image img))
                    img.sprite = BasePlugin.Asset.Get<Sprite>("WhiteCheckbox");
                transform.Find("ToggleText").GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            foreach (Transform child in transform)
                ChangeMenuToggleColor(child);
        }
    }
}
