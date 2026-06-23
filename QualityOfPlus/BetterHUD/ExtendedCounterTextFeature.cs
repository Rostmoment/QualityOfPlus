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
            Enabled = category.CreateEntry<bool>(
                "Extended Counter Text", false,
                "If true, notebooks counter will include the word 'Notebooks' and elevators counter will include 'Elevators'");
        }

        public override void PostInitialize(QOPCategory category) { }
    }
}