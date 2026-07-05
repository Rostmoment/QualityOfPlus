using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.BetterUI.BetterStickerMenu
{
    internal class ExtendedInventorySticker : MonoBehaviour
    {
        private bool initializedPrefab = false;

        [SerializeField]
        private Image bgImage;

        public InventorySticker sticker;

        public ExtendedStickerData Data => StickerMetaStorage.Instance.Get(sticker.Sticker).value;
        public int Value {  get; private set; }

        public void InitializeExtendedPrefab(InventorySticker sticker)
        {
            if (initializedPrefab)
                return;
            
            this.sticker = sticker;

            GameObject bg = new GameObject("BG");
            bg.transform.SetParent(transform);
            bg.transform.SetSiblingIndex(1);

            bgImage = bg.AddComponent<Image>();
            bgImage.raycastTarget = false;
            bgImage.rectTransform.sizeDelta = new Vector2(45, 25);
            bgImage.color = new Color(0.1f, 0.1f, 0.1f);

            bg.AddComponent<Outline>().effectColor = Color.white;
            sticker.totalTmp.color = Color.white;
            sticker.totalTmp.alignment = TextAlignmentOptions.Center;
            sticker.totalTmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
            sticker.totalTmp.verticalAlignment = VerticalAlignmentOptions.Middle;

            transform.Find("Sprite").localPosition = new Vector3(10, 0, 0);
            initializedPrefab = true;
        }

        public void Initialize()
        {
            sticker.totalTmp.transform.localPosition = new Vector3(50, 0, 0);
            bgImage.transform.localPosition = new Vector3(75, 0, 0);
        }

        public void SetValue(int value)
        {
            Value = value;
            bgImage.gameObject.SetActive(value > 1);
            if (value <= 1)
                sticker.totalTmp.text = "";
            else
                sticker.totalTmp.text = $"×{value}";

        }
    }
}
