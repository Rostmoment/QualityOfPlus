using QualityOfPlus.Interfaces;

namespace QualityOfPlus.BetterHUD
{
    public class ExtendedCounterTextFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.HUD.EXTENDED.COUNTER.TEXT";

        protected override string EnabledConfigKey => "Extended Counter Text";
        protected override string EnabledConfigDescription => "Adds the label 'Notebooks' to the notebooks counter and 'Elevators' to the elevators counter";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}