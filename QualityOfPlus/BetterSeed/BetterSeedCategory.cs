using QualityOfPlus.BetterSeed.CopySeed;
using QualityOfPlus.BetterSeed.UnlockedSeedInput;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterSeed
{
    public class BetterSeedCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.SEED";
        public override string Name => "Better Seed";


        public override void PreInitialize()
        {
            AddFeature<CopySeedFeature>();
            AddFeature<UnlockedSeedInputFeature>();
        }
        public override void PostInitialize()
        {
        }
    }
}
