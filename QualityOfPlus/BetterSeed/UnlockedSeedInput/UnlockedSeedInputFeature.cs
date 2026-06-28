using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterSeed.UnlockedSeedInput
{
    public class UnlockedSeedInputFeature : QOPFeature, IToggleableFeature
    {
        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.UNLOCK.SEED.INPUT";


        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Unlock Seed Input", true, "Unlocks seed input even on new save files");
        }
        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
