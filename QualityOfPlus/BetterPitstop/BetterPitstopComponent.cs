using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterPitstop
{
    class BetterPitstopComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Pitstop";

        private static ConfigEntry<bool> noLoadingScreen;
        private static ConfigEntry<bool> pauseLoadMusic;

        public static bool NoLoadingScreen => noLoadingScreen.Value;
        public static bool PauseLoadMusic => pauseLoadMusic.Value;

        public override void Initialize()
        {
            noLoadingScreen = CreateConfig("No Loading Screen", false, "Removes fake loading screen for field trip");
            pauseLoadMusic = CreateConfig("Pause Load Music", true, "Pauses music during loading screen");
        }
    }
}
