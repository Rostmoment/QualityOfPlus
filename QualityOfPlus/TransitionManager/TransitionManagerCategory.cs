using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TransitionManager
{
    public class TransitionManagerCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.TRANSITION.MANAGER";
        public override string Name => "Transition Manager";


        public override void PreInitialize()
        {
            AddFeature<TransitionManagerFeature>();
        }
        public override void PostInitialize()
        {
        }
    }
}
