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
        private static ConfigEntry<bool> altChalkboardTexture;
        public static bool NoLoadingScreen => noLoadingScreen.Value;
        public static bool PauseLoadMusic => pauseLoadMusic.Value;
        public static bool AltChalkboardTexture => false; // I have no idea why it doesn't work, but it doesn't, so I'm just gonna disable it for now

        public override void Initialize()
        {
            noLoadingScreen = CreateConfig("No Loading Screen", false, "Removes fake loading screen for field trip");
            pauseLoadMusic = CreateConfig("Pause Load Music", true, "Pauses music during loading screen");
           // altChalkboardTexture = CreateConfig("Alt Chalkboard Texture", false, "Changes level type chalkboard texture to a different one");
        }
    }
}
