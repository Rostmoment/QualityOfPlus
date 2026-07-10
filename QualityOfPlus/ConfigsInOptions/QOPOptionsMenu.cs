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

        private StandardMenuButton scrollButton;
        private const float SCROLL_SIZE_X = 25;
        private const float SCROLL_SIZE_Y = 200;
        private const float SCROLL_MAX = 73;
        private const float SCROLL_MIN = -73;

        private float offset;

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

            AddScroller();
        }

        private void AddScroller()
        {
            Image scrollBg = CreateImage(null, "ScrollBG", new Vector3(165, -45, 0), new Vector2(SCROLL_SIZE_X, SCROLL_SIZE_Y));
            scrollBg.color = Color.black;
            scrollBg.gameObject.AddComponent<Outline>().effectColor = Color.white;

            GameObject scrollButtonObject = new GameObject("Button");
            scrollButtonObject.transform.SetParent(scrollBg.transform);
            scrollButtonObject.transform.localPosition = Vector3.zero;
            scrollButtonObject.transform.localScale = Vector3.one;

            Image scrollButtonImage = scrollButtonObject.AddComponent<Image>();
            scrollButtonImage.rectTransform.sizeDelta = new Vector2(SCROLL_SIZE_X - 4, 45);
            scrollButtonImage.color = new Color(0.277f, 0.757f, 0.101f);
            scrollButtonObject.AddComponent<Outline>().effectColor = Color.white;
            scrollButtonImage.raycastTarget = true;

            scrollButton = scrollButtonObject.ConvertToButton<StandardMenuButton>();
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

            ResetScroll();
        }

        private void ResetScroll()
        {
            offset = 0f;

            if (scrollButton != null)
            {
                Vector3 resetPos = scrollButton.transform.localPosition;
                resetPos.y = SCROLL_MAX;
                scrollButton.transform.localPosition = resetPos;
            }

            CurrentActiveCategory.ApplyScroll(0f);
        }

        private float GetMaxScrollOffset()
        {
            return Mathf.Max(0f, CurrentActiveCategory.TotalUnits - MASK_SIZE_Y);
        }

        private void Update()
        {
            float maxOffset = GetMaxScrollOffset();
            float trackRange = SCROLL_MAX - SCROLL_MIN;

            if (maxOffset <= 0f)
            {
                Vector3 resetPos = scrollButton.transform.localPosition;
                resetPos.y = SCROLL_MAX;
                scrollButton.transform.localPosition = resetPos;

                offset = 0f;
                CurrentActiveCategory.ApplyScroll(0f);
                return;
            }

            if (scrollButton.held)
            {
                Vector3 local = scrollButton.transform.localPosition;
                local.y += CursorController.Instance.movementThisFrame.y;
                local.y = Mathf.Clamp(local.y, SCROLL_MIN, SCROLL_MAX);
                scrollButton.transform.localPosition = local;
            }

            float t = (SCROLL_MAX - scrollButton.transform.localPosition.y) / trackRange;
            offset = t * maxOffset;

            CurrentActiveCategory.ApplyScroll(offset);
        }
    }
}