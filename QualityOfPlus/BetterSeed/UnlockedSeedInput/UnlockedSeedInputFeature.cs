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

        protected override string EnabledConfigKey => "Unlocks seed input even on new save files";
        protected override string EnabledConfigDescription => throw new NotImplementedException();

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
