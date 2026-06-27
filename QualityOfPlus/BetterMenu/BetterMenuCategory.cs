using QualityOfPlus.BetterMenu.FloorSelect;
using QualityOfPlus.BetterMenu.UnlockedSeedInput;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterMenu
{
    public class BetterMenuCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.MENU";
        public override string Name => "Better Menu";


        public override void PreInitialize()
        {
            AddFeature<FloorSelectFeature>();
            AddFeature<UnlockedSeedInputFeature>();
        }
        public override void PostInitialize()
        {
        }
    }
}
