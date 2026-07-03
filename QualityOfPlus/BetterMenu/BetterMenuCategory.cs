using QualityOfPlus.BetterMenu.DarkMode;
using QualityOfPlus.BetterMenu.FloorSelect;
using QualityOfPlus.BetterSeed.UnlockedSeedInput;
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
            AddFeature<DarkModeFeature>();
        }
        public override void PostInitialize()
        {
        }
    }
}
