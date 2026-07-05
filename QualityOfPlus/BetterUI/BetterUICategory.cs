using QualityOfPlus.BetterUI.BetterStickerMenu;
using QualityOfPlus.BetterUI.TABSwitch;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterUI
{
    public class BetterUICategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.UI";
        public override string Name => "Better UI";

        public override void PreInitialize()
        {
            AddFeature<TABSwitchFeature>();
            AddFeature<BetterStickersMenuFeature>();
        }

        public override void PostInitialize()
        {
        }
    }
}