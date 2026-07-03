using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterPitstop.NoFakeLoad
{
    public class NoFakeLoadFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.BETTER.PITSTOP.NOF.AKE.LOAD";

        protected override string EnabledConfigKey => "No Loading Screen";
        protected override string EnabledConfigDescription => "Removes the fake loading screen when going to field trip";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
