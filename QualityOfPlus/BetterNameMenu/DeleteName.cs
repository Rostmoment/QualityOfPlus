using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterNameMenu
{
    [HarmonyPatch(typeof(NameManager))]
    class DeleteName
    {
        private static int savedIndex = -1;
        private static GameObject deleteConfirmation;

        [HarmonyPatch(nameof(NameManager.Awake))]
        [HarmonyPostfix]
        private static void AddButtons(NameManager __instance)
        {

            if (!BasePlugin.Asset.Exists<Sprite>("CrossMark"))
                BasePlugin.Asset.Add<Sprite>("CrossMark", Resources.FindObjectsOfTypeAll<Sprite>().Find(x => x.name == "YCTP_IndicatorsSheet_1"));

            if (!BetterNameMenuComponent.DeleteButton)
                return;


            for (int i = 0; i < __instance.buttons.Length; i++)
            {
                if (__instance.nameList[i].IsNullOrWhiteSpace())
                    continue;

                Image image = UIHelpers.CreateImage(BasePlugin.Asset.Get<Sprite>("CrossMarkPointed"), __instance.buttons[i].transform.parent,
                    __instance.buttons[i].transform.localPosition - new Vector3(60, 0, 0), false);

                image.raycastTarget = true;
                __instance.buttons[i].transform.localPosition += new Vector3(25, 0, 0);

                StandardMenuButton delete = image.gameObject.ConvertToButton<StandardMenuButton>();
                delete.transitionOnPress = true;
                delete.highlightedSprite = BasePlugin.Asset.Get<Sprite>("CrossMark");
                delete.heldSprite = BasePlugin.Asset.Get<Sprite>("CrossMark");
                delete.unhighlightedSprite = BasePlugin.Asset.Get<Sprite>("CrossMarkPointed");
                delete.name = $"DeleteButton_{i}";
                delete.swapOnHigh = true;

                int index = i;
                delete.OnPress.AddListener(() =>
                {
                    CreateConfirm(__instance);

                    deleteConfirmation.SetActive(true);
                    savedIndex = index;
                });
            }

        }

        private static void CreateConfirm(NameManager __instance)
        {
            if (!deleteConfirmation.IsNullOrDestroyed())
                return;

            deleteConfirmation = GameObject.Instantiate(__instance.transform.parent.Find("KeyboardScreen").gameObject, __instance.transform.parent);
            deleteConfirmation.name = "ConfirmDeleteScreen";
            GameObject.Destroy(deleteConfirmation.transform.Find("Name").gameObject);
            GameObject.Destroy(deleteConfirmation.transform.Find("CaptialLetters").gameObject);
            GameObject.Destroy(deleteConfirmation.transform.Find("LowercaseLetters").gameObject);
            GameObject.Destroy(deleteConfirmation.transform.Find("Backspace").gameObject);
            GameObject.Destroy(deleteConfirmation.transform.Find("SubmitDisabled").gameObject);
            GameObject.Destroy(deleteConfirmation.transform.Find("BackButton").gameObject);

            GameObject.Destroy(deleteConfirmation.transform.Find("Instructions").GetComponent<TextLocalizer>());
            GameObject.Destroy(deleteConfirmation.transform.Find("Submit").Find("TMP").GetComponent<TextLocalizer>());

            deleteConfirmation.transform.Find("Instructions").transform.localPosition = new Vector3(0, 46, 0);
            deleteConfirmation.transform.Find("Instructions").GetComponent<TextMeshProUGUI>().fontSize = 34;
            deleteConfirmation.transform.Find("Instructions").GetComponent<TextMeshProUGUI>().text = LocalizationManager.Instance.GetLocalizedText("Opt_ConfirmDelete");
            deleteConfirmation.transform.Find("Instructions").GetComponent<TextMeshProUGUI>().color = Color.white;

            deleteConfirmation.transform.Find("Submit").gameObject.SetActive(true);
            deleteConfirmation.transform.Find("Submit").GetComponent<StandardMenuButton>().OnPress = new UnityEngine.Events.UnityEvent();
            deleteConfirmation.transform.Find("Submit").GetComponent<StandardMenuButton>().OnPress.AddListener(() =>
            {
                if (savedIndex < 0)
                    return;

                __instance.DeleteName(savedIndex);
                AdditiveSceneManager.Instance.LoadScene("MainMenu");
                savedIndex = -1;

            });
            deleteConfirmation.transform.Find("Submit").Find("TMP").GetComponent<TextMeshProUGUI>().text = LocalizationManager.Instance.GetLocalizedText("But_QuitYes");
            deleteConfirmation.transform.Find("Submit").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 48);
            deleteConfirmation.transform.Find("Submit").name = "Yes";

            GameObject noButton = GameObject.Instantiate(deleteConfirmation.transform.Find("Yes").gameObject, deleteConfirmation.transform);
            noButton.name = "No";
            noButton.GetComponent<StandardMenuButton>().transitionOnPress = true;
            noButton.SetActive(true);
            noButton.GetComponent<StandardMenuButton>().OnPress = new UnityEngine.Events.UnityEvent();
            noButton.GetComponent<StandardMenuButton>().OnPress.AddListener(() =>
            {
                savedIndex = -1;
                deleteConfirmation.SetActive(false);
            });
            noButton.transform.Find("TMP").GetComponent<TextMeshProUGUI>().text = LocalizationManager.Instance.GetLocalizedText("But_QuitNo");
            noButton.transform.localPosition = new Vector3(-64, -98, 0);
            noButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 48);

            deleteConfirmation.transform.SetSiblingIndex(__instance.transform.parent.Find("KeyboardScreen").GetSiblingIndex());
        }
        
    }
}
