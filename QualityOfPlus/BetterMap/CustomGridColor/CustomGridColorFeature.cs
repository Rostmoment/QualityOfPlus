using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap.CustomGridColor
{
    public class CustomGridColorFeature : QOPFeature, IToggleableFeature
    {
        public override string ID => "QOP.FEATURE.CUSTOM.MAP.GRID.COLOR";

        private bool enabled;
        public bool IsEnabled() => enabled;
        public bool TrySetActive(bool value)
        {
            enabled = value;
            return true;
        }

        private ConfigEntry<Color> configEntry;
        public Color Color => configEntry.Value;

        public override void PreInitialize(QOPCategory category)
        {
            configEntry = category.CreateEntry<Color>("", new Color(0, 0.3922f, 0, 1), "Custom color for map grid\nDefault value is exactly in game default color");
            TrySetActive(true);
        }

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
