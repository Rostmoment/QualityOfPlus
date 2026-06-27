using BepInEx.Configuration;
using QualityOfPlus.Interfaces;

namespace QualityOfPlus.BetterHUD
{
    public class ExtendedCounterTextFeature : QOPFeature, IToggleableFeature
    {
        public override string ID => "QOP.FEATURE.HUD.EXTENDED.COUNTER.TEXT";

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Extended Counter Text", false, "Adds the label 'Notebooks' to the notebooks counter and 'Elevators' to the elevators counter");
        }

        public override void PostInitialize(QOPCategory category) { }
    }
}