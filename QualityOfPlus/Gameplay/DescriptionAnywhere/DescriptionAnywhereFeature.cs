using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay.DescriptionAnywhere
{
    public class DescriptionAnywhereFeature : QOPFeature, IToggleableFeature
    {
        public override string ID => "QOP.FEATURE.GAMEPLAY.DESC.ANYWHERE";

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry("Pickup Description Anywhere", false, "If true, item description will be showed anywhere");
        }

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}