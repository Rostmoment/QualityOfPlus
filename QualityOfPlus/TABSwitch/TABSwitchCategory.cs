using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TABSwitch
{
    public class TABSwitchCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.TAB.SWITCH";
        public override string Name => "TAB Switch";

        public override void PreInitialize()
        {
            AddFeature<TABSwitchFeature>();
        }

        public override void PostInitialize()
        {
        }
    }
}