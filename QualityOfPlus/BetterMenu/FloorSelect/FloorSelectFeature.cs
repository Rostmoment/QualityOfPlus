using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterMenu.FloorSelect
{
    public class FloorSelectFeature : QOPFeature, IToggleableFeature
    {
        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.FLOOR.SELECT";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Floor Select buttons", false, "Enables floor select buttons that mystman uses for debug");
        }
        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
