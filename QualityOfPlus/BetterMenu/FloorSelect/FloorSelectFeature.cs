using QualityOfPlus.Interfaces;

namespace QualityOfPlus.BetterMenu.FloorSelect
{
    public class FloorSelectFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.FLOOR.SELECT";

        protected override string EnabledConfigKey => "Floor Select Buttons";
        protected override string EnabledConfigDescription => "Enables floor select buttons that mystman uses for debug";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}