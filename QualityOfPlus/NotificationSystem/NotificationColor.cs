using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.NotificationSystem
{
    internal enum NotificationColor
    {
        Blue,
        Green,
        Yellow,
        Red,
        Purple,
        Orange,
        Cyan,
        Pink
    }

    internal static class NotificationColorExtensions
    {
        private static Dictionary<NotificationColor, Color> spriteColors = new Dictionary<NotificationColor, Color>()
        {
            [NotificationColor.Blue]  = new Color(0.2f, 0.6f, 1.0f, 1f) ,  
            [NotificationColor.Green] = new Color(0.2f, 0.9f, 0.4f, 1f),   
            [NotificationColor.Yellow] = new Color(1.0f, 0.85f, 0.1f, 1f),   
            [NotificationColor.Red] = new Color(1.0f, 0.25f, 0.25f, 1f),    
            [NotificationColor.Purple] = new Color(0.7f, 0.2f, 0.9f, 1f),  
            [NotificationColor.Orange] = new Color(1.0f, 0.55f, 0.0f, 1f),
            [NotificationColor.Cyan] = new Color(0.0f, 0.9f, 0.9f, 1f),   
            [NotificationColor.Pink] = new Color(1.0f, 0.3f, 0.7f, 1f)    
        };

        private static Dictionary<NotificationColor, Color> outineColors = new Dictionary<NotificationColor, Color>()
        {
            [NotificationColor.Blue] = new Color(0.05f, 0.25f, 0.5f, 1f),  
            [NotificationColor.Green] = new Color(0.0f, 0.45f, 0.15f, 1f),
            [NotificationColor.Yellow] = new Color(0.6f, 0.45f, 0.0f, 1f),  
            [NotificationColor.Red] = new Color(0.5f, 0.05f, 0.05f, 1f),    
            [NotificationColor.Purple] = new Color(0.35f, 0.05f, 0.5f, 1f), 
            [NotificationColor.Orange] = new Color(0.6f, 0.25f, 0.0f, 1f),  
            [NotificationColor.Cyan] = new Color(0.0f, 0.45f, 0.45f, 1f),   
            [NotificationColor.Pink] = new Color(0.6f, 0.1f, 0.35f, 1f)     
        };

        public static Color GetColorForSprite(this NotificationColor color) => spriteColors[color];
        public static Color GetColorForOutline(this NotificationColor color) => outineColors[color];
    }
}