using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterHUD
{
    class BetterHUDComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better HUD";

        private static ConfigEntry<bool> elevatorsCounter;
        private static ConfigEntry<bool> extendedCounterText;

        public static bool ElevatorsCounter => elevatorsCounter.Value;
        public static bool ExtendedCounterText => extendedCounterText.Value;

        public override void Initialize()
        {
            elevatorsCounter = CreateConfig<bool>("Elevators Counter", true, "If true, notebooks counter will be replaced with elevators counter after collecting last notebook");
            extendedCounterText = CreateConfig<bool>("Extended Counter Text", false, "If true, notebooks counter will have word 'notebooks' and elevators counter willl have word 'Elevators'");
        }
    }
}
