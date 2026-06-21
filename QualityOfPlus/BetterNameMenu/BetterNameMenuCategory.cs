using QualityOfPlus.BetterNameMenu.BackToNameMenu;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterNameMenu
{
    public class BetterNameMenuCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.NAME.MENU";
        public override string Name => "Better Name Menu";

        public override void PreInitialize()
        {
            AddFeature<BackToNameMenuFeature>();
        }
        public override void PostInitialize()
        {

        }
    }
}
