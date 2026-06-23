namespace QualityOfPlus.BetterHUD
{
    public class BetterHUDCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.HUD";
        public override string Name => "Better HUD";

        public override void PreInitialize()
        {
            AddFeature<ElevatorsCounterFeature>();
            AddFeature<ExtendedCounterTextFeature>();
        }

        public override void PostInitialize() { }
    }
}