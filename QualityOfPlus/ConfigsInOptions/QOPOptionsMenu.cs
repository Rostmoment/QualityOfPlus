using BepInEx.Configuration;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.ConfigsInOptions
{
    internal class QOPOptionsMenu : CustomOptionsCategory
    {
        public const int MASK_SIZE_X = 364;
        private const int MASK_SIZE_Y = 205;
        private const int CATEGORY_Y = 75;


        private List<QOPOptionsCategory> categories = new List<QOPOptionsCategory>();
        private TextMeshProUGUI categoryTitle;
        private int currentCategory = 0;
        private QOPOptionsCategory CurrentActiveCategory => categories[currentCategory];

        public static void Register(OptionsMenu menu, CustomOptionsHandler handler)
        {
            handler.AddCategory<QOPOptionsMenu>("QOP");
        }

        public override void Build()
        {
            foreach (QOPCategory category in QOPManager.Instance.Categories)
                CreateCategory(category).SetActive(false);

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
                TextAlignmentOptions.Center, new Vector2(MASK_SIZE_X - 50, 20), Color.white);

            ChangeCategory(false);
            ChangeCategory(true);

            CreateApplyButton(() =>
            {
                foreach (QOPOptionsCategory category in categories)
                    category.OnApplyButtonPressed();
            });

            CreateText("UseWheel", LocalizationManager.Instance.GetLocalizedText("UseMMB"), new Vector3(-20, -160), BaldiFonts.ComicSans24, TextAlignmentOptions.Left, new Vector2(300, 20), Color.red);
        }

        private QOPOptionsCategory CreateCategory(QOPCategory qop)
        {
            GameObject categoryObject = new GameObject(qop.ID);
            categoryObject.transform.SetParent(transform, false);
            categoryObject.transform.position = Vector3.zero;

            Image maskImage = CreateImage(null, $"MASK.{qop.ID}", new Vector3(4, -45, 0), new Vector2(MASK_SIZE_X, MASK_SIZE_Y));
            maskImage.color = Color.white;
            maskImage.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            maskImage.transform.SetParent(categoryObject.transform, true);

            QOPOptionsCategory category = new QOPOptionsCategory(categoryObject, maskImage, qop, this);
            category.Build();

            if (category.ObjectCount > 0)
                categories.Add(category);

            return category;
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


        private void Update()
        {
            float y = Input.mouseScrollDelta.y;
            CurrentActiveCategory.ScrollFor(y);
        }
    }
}
