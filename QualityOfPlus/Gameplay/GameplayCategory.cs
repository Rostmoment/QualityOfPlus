using QualityOfPlus.Gameplay.DescriptionAnywhere;
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
        }

        public override void PostInitialize()
        {
        }
    }
}