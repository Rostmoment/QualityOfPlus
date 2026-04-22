using MTM101BaldAPI.OptionsAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode
{
    class OptionsMenuDarkMode
    {

        private static void ChangeTransformColor(Transform transform)
        {
            foreach (TextMeshProUGUI tmp in transform.GetComponents<TextMeshProUGUI>())
            {
                if (tmp.color == Color.black)
                    tmp.color = Color.white;
            }

            foreach (TextMeshProUGUI tmp in transform.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (tmp.color == Color.black)
                    tmp.color = Color.white;
            }

            foreach (Transform t in transform)
                ChangeTransformColor(t);
        }
        private static void ChangeColor(Transform transform)
        {

            transform.Find("BG").GetComponent<UnityEngine.UI.Image>().color = Color.black;
        }



        public static void ApplyDarkMode(OptionsMenu menu, CustomOptionsHandler handler)
        {
            if (!BetterMenuComponent.DarkMode)
                return;


            Transform b = menu.transform.Find("Base");
            b.Find("White").GetComponent<Image>().color = Color.black; // White is not white
            b.Find("BG").GetComponent<Image>().sprite = BasePlugin.Asset.Get<Sprite>("OptionsMenuDarkMode");

            ChangeTransformColor(menu.transform);

            ChangeColor(menu.transform.Find("Data").Find("Confirm"));
            ChangeColor(menu.transform.Find("Data").Find("Continue"));
            ChangeColor(menu.transform.Find("Data").Find("Deleting"));

            foreach (Transform child in menu.transform)
                MainMenuDarkMode.ChangeMenuToggleColor(child);

            menu.transform.Find("TooltipBase").Find("Tooltip").Find("Tmp").GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }
}
