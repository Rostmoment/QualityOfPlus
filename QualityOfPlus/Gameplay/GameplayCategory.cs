using QualityOfPlus.Gameplay.DescriptionAnywhere;
using QualityOfPlus.Gameplay.NowPlaysMusic;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay
{
    public class GameplayCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.GAMEPLAY";

        public override string Name => "Gameplay";

        public override void PreInitialize()
        {
            AddFeature<DescriptionAnywhereFeature>();
            AddFeature<NowPlaysMusicFeature>();
        }

        public override void PostInitialize()
        {
        }
    }
}