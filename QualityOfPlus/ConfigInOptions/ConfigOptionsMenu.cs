using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.ConfigInOptions
{
    class ConfigOptionsMenu : CustomOptionsCategory
    {
        public const int MASK_SIZE_X = 364;
        public const int MASK_SIZE_Y = 205;
        public const int CATEGORY_Y = 75;


        private Dictionary<string, List<ConfigEntryBase>> configs = new Dictionary<string, List<ConfigEntryBase>>();
        private List<ConfigCategory> categories = new List<ConfigCategory>();
        private TextMeshProUGUI categoryTitle;
        private int currentCategory = 0;
        private ConfigCategory CurrentActiveCategory => categories[currentCategory];

        public static void Register(OptionsMenu menu, CustomOptionsHandler handler)
        {
            handler.AddCategory<ConfigOptionsMenu>("QOP");
        }

        public override void Build()
        {
            foreach (ConfigDefinition config in BasePlugin.Instance.Config.Keys)
            {
                if (!configs.ContainsKey(config.Section))
                    configs[config.Section] = new List<ConfigEntryBase>();

                configs[config.Section].Add(BasePlugin.Instance.Config[config]);
            }

            foreach (var data in configs)
                CreateCategory(data.Key, data.Value).SetActive(false);

            categories[0].SetActive(true);

            CreateButton(() =>
            {
                ChangeCategory(true);
            }, BasePlugin.Asset.Get<Sprite>("ArrowRightUnhigh"), BasePlugin.Asset.Get<Sprite>("ArrowRightHigh"), "NextCategory", new Vector3(170, CATEGORY_Y));
            CreateButton(() =>
            {
                ChangeCategory(false);
            }, BasePlugin.Asset.Get<Sprite>("ArrowLeftUnhigh"), BasePlugin.Asset.Get<Sprite>("ArrowLeftHigh"), "PreviousCategory", new Vector3(-165, CATEGORY_Y));
            categoryTitle = CreateText("CategoryTitle", "", new Vector3(0, CATEGORY_Y), BaldiFonts.ComicSans18, 
                TextAlignmentOptions.Center, new Vector2(MASK_SIZE_X-50, 20), Color.white);

            ChangeCategory(false);
            ChangeCategory(true);

            StandardMenuButton apply = CreateApplyButton(() =>
            {
                foreach (ConfigCategory category in categories)
                    category.OnApplyButtonPressed();
            });

            CreateText("UseWheel", LocalizationManager.Instance.GetLocalizedText("UseMMB"), new Vector3(-20, -160),
                BaldiFonts.ComicSans24, TextAlignmentOptions.Left, new Vector2(300, 20), Color.red);
        }

        private void ChangeCategory(bool increment)
        {

            if (increment) 
            {
                categories[currentCategory++].SetActive(false);
                if (currentCategory >= categories.Count)
                    currentCategory = 0;
            }
            else
            {
                categories[currentCategory--].SetActive(false);
                if (currentCategory < 0)
                    currentCategory = categories.Count - 1;
            }

            CurrentActiveCategory.SetActive(true);
            categoryTitle.text = categories[currentCategory].Name;
        }

        private ConfigCategory CreateCategory(string category, List<ConfigEntryBase> configs)
        {
            GameObject categoryObject = new GameObject(category);
            categoryObject.transform.SetParent(transform, false);
            categoryObject.transform.position = Vector3.zero;

            Image maskImage = CreateImage(null, $"Mask{category}", new Vector3(4, -45, 0), new Vector2(MASK_SIZE_X, MASK_SIZE_Y));
            maskImage.color = Color.white;
            maskImage.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            maskImage.transform.SetParent(categoryObject.transform, true);

            ConfigCategory result = new ConfigCategory(categoryObject, maskImage);

            foreach (ConfigEntryBase configEntry in configs)
            {
                if (configEntry is ConfigEntry<bool> boolEntry)
                    result.AddToggle(this, boolEntry);
            }

            categories.Add(result);
            return result;
        }

        private void Update()
        {
            float y = Input.mouseScrollDelta.y;
            CurrentActiveCategory.ScrollFor(y);
        }
    }
}
