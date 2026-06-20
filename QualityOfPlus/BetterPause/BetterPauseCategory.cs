using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterPause
{
    public class BetterPauseCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.PAUSE";

        public override string Name => "Better Pause";

        public override void PostInitialize()
        {
            AddFeature<PauseWithoutScreenFeature>();
            AddFeature<CopySeedFeature>();
        }

        public override void PreInitialize()
        {
        }
    }
}
