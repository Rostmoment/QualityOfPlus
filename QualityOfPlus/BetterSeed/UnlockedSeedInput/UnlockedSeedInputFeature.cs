using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterSeed.UnlockedSeedInput
{
    public class UnlockedSeedInputFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.UNLOCK.SEED.INPUT";

        protected override string EnabledConfigKey => "Unlocked Seed Input";
        protected override string EnabledConfigDescription => "Unlocks seed input even on new save files";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
