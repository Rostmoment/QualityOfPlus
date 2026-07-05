using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterUI.BetterStickerMenu
{
    internal class NewStickerMenu : MonoBehaviour
    {
        private const float SCROLLER_BUTTON_MAX = 99;
        private const float SCROLLER_BUTTON_MIN = -99;
        private const float SCROLL_HEIGHT = 250;
        private const float SCROLL_BAR_WIDTH = 20;

        private const float X_POSITION = -30f;
        private const float STARTING_Y = 76f;
        private const float SPACING = 76f;
        private const float UNITS_PER_STICKER = SPACING;

        private float offset;

        private TextMeshProUGUI sortingText;
        private int sortingIndex = 0;
        private StickersSortingMethod CurrentSorting => QOPManager.Instance.GetFeature<BetterStickersMenuFeature>().sortingMethods[0];

        private StickerScreenController screen;

        private Image mask;
        private StandardMenuButton scrollerButton;



        private void Awake()
        {
            screen = gameObject.GetComponent<StickerScreenController>();
            offset = 0;
            sortingIndex = -1;

            SetupMask();
            SetupScroller();// TODO: add sorting options
            //ChangeSortingIndex();
        }

        private void SetupMask()
        {
            GameObject maskObject = new GameObject("Mask");
            maskObject.transform.SetParent(screen.transform);
            maskObject.transform.SetSiblingIndex(2);

            mask = maskObject.AddComponent<Image>();
            mask.rectTransform.sizeDelta = new Vector2(160, SCROLL_HEIGHT);
            mask.transform.localPosition = new Vector3(-150, -10, 0);
            mask.gameObject.AddComponent<Mask>().showMaskGraphic = false;
        }
        private void SetupScroller()
        {
            GameObject scroller = new GameObject("Scroller");
            scroller.transform.SetParent(screen.transform);
            scroller.transform.SetSiblingIndex(3);

            Image barImage = scroller.AddComponent<Image>();
            barImage.color = Color.black;
            barImage.rectTransform.sizeDelta = new Vector2(SCROLL_BAR_WIDTH, SCROLL_HEIGHT);

            scroller.AddComponent<Outline>().effectColor = Color.white;
            scroller.transform.localPosition = new Vector3(-50, -10, 0);

            GameObject button = new GameObject("Button");
            button.transform.SetParent(scroller.transform);
            
            Image buttonImage = button.AddComponent<Image>();
            buttonImage.color = new Color(0.4f, 0.4f, 0.4f);
            buttonImage.rectTransform.sizeDelta = new Vector2(16, 50);
            buttonImage.raycastTarget = true;

            button.AddComponent<Outline>().effectColor = Color.white;
            button.transform.localPosition = new Vector3(0, SCROLLER_BUTTON_MAX, 0);

            scrollerButton = button.ConvertToButton<StandardMenuButton>();
        }/*
        private void SetupSorting()
        {
            GameObject mainObject = new GameObject("Sorting");
            mainObject.transform.SetParent(screen.transform);
            mainObject.transform.SetSiblingIndex(3);
            mainObject.transform.localPosition =

            GameObject bg = new GameObject("BG");
            bg.transform.SetParent(mainObject.transform);
            Image image = bg.AddComponent<Image>();
            image.color = Color.black;
            bg.AddComponent<Outline>().effectColor = Color.white;

            sortingText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans12, "", mainObject.transform, Vector3.zero);
            sortingText.transform.localPosition = new Vector3(30, -18, 0);
            sortingText.raycastTarget = true;
            StandardMenuButton button = sortingText.gameObject.ConvertToButton<StandardMenuButton>();
            button.OnPress.AddListener(ChangeSortingIndex);
            button.underlineOnHigh = true;
        }*/
        private void ChangeSortingIndex()
        {
            BetterStickersMenuFeature feature = QOPManager.Instance.GetFeature<BetterStickersMenuFeature>();
            sortingIndex++;
            if (sortingIndex > 0)
                sortingIndex = feature.sortingMethods.Count - 1;
            if (feature.sortingMethods.Count >= sortingIndex)
                sortingIndex = 0;

            sortingText.text = $"Sort by: {CurrentSorting.Name}";
        }

        private void Update()
        {
            float maxOffset = GetMaxScrollOffset();
            float trackRange = SCROLLER_BUTTON_MAX - SCROLLER_BUTTON_MIN;

            if (maxOffset <= 0f)
            {
                Vector3 resetPos = scrollerButton.transform.localPosition;
                resetPos.y = SCROLLER_BUTTON_MAX;
                scrollerButton.transform.localPosition = resetPos;
                offset = 0f;
                return;
            }

            if (scrollerButton.held)
            {
                Vector3 local = scrollerButton.transform.localPosition;
                local.y += CursorController.Instance.movementThisFrame.y;
                local.y = Mathf.Clamp(local.y, SCROLLER_BUTTON_MIN, SCROLLER_BUTTON_MAX);
                scrollerButton.transform.localPosition = local;
            }
            float t = (SCROLLER_BUTTON_MAX - scrollerButton.transform.localPosition.y) / trackRange;
            offset = t * maxOffset;
        }

        private float GetMaxScrollOffset()
        {
            int stickerCount = screen.inventoryStickers.Count;
            float contentHeight = stickerCount * UNITS_PER_STICKER;
            return Mathf.Max(0f, contentHeight - SCROLL_HEIGHT);
        }

        public void SortStickers()
        {
            ExtendedInventorySticker[] stickers = screen.inventoryStickers.Select(x => x.GetComponent<ExtendedInventorySticker>()).ToArray();
            ExtendedInventorySticker[] sorted = CurrentSorting.Sorting(stickers);

            float y = STARTING_Y + offset;
            foreach (ExtendedInventorySticker sticker in sorted)
            {
                AddToMask(sticker);
                sticker.transform.localPosition = new Vector3(X_POSITION, y, 0);
                sticker.sticker.hotSpot.rectTransform.sizeDelta = new Vector2(128, 64);
                y -= SPACING;
            }

            if (screen.holdingSticker)
            {
                RemoveFromMask(screen.inventoryStickers[screen.heldStickerInstantiationId]);
                screen.inventoryStickers[screen.heldStickerInstantiationId].SetPosition(CursorController.Instance.LocalPositionAroundCenter - screen.heldStickerBaseTransform.localPosition);
            }
        }

        #region mask contoller
        public void AddToMask(ExtendedInventorySticker sticker)
        {
            sticker.transform.SetParent(mask.transform);
        }
        public void RemoveFromMask(ExtendedInventorySticker sticker)
        {
            sticker.transform.SetParent(screen.inventoryStickersTransform);
        }
        public void AddToMask(InventorySticker sticker)
        {
            sticker.transform.SetParent(mask.transform);
        }
        public void RemoveFromMask(InventorySticker sticker)
        {
            sticker.transform.SetParent(screen.inventoryStickersTransform);
        }
        #endregion
    }
}
