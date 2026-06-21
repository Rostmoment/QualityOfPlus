using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterGameWindow
{
    public class BetterGameWindowCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.GAME.WINDOW";

        public override string Name => "Better Game Window";

        public override void PreInitialize()
        {
            AddFeature<FreeResizeFeature>();
        }
        public override void PostInitialize()
        {
        }
    }
}
