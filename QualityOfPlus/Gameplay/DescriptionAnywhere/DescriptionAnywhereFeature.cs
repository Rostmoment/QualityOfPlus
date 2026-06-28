using QualityOfPlus.Interfaces;

namespace QualityOfPlus.Gameplay.DescriptionAnywhere
{
    public class DescriptionAnywhereFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.GAMEPLAY.DESC.ANYWHERE";

        protected override string EnabledConfigKey => "Pickup Description Anywhere";
        protected override string EnabledConfigDescription => "Shows item descriptions everywhere";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}