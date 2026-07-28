using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.NotificationSystem
{
    internal struct NotificationData
    {
        private string title;
        private string content;
        private NotificationColor color;

        public NotificationData(string title, string content, NotificationColor color)
        {
            this.title = title;
            this.content = content;
            this.color = color;
        }

        public string Title => LocalizationManager.Instance.GetLocalizedText(title);
        public string Content => LocalizationManager.Instance.GetLocalizedText(content);
        public Color SpriteColor => color.GetColorForSprite();
        public Color OutlineColor => color.GetColorForOutline();
    }
}
