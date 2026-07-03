using QualityOfPlus.BetterPitstop.NoFakeLoad;
using QualityOfPlus.BetterPitstop.PauseMusic;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterPitstop
{
    public class BetterPitstopCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.PITSTOP";

        public override string Name => "Better Pitstop";

        public override void PreInitialize()
        {
            AddFeature<NoFakeLoadFeature>();
            AddFeature<PauseMusicFeature>();
        }

        public override void PostInitialize()
        {
        }
    }
}
