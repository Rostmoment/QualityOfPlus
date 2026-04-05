using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterPause
{
    class BetterPauseComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Pause";

        private static ConfigEntry<bool> enableRestartButton;
        public static bool EnableRestartButton => enableRestartButton.Value;
        private static ConfigEntry<bool> enableCopySeedFunction;
        public static bool EnableCopySeedFunction => enableCopySeedFunction.Value;
        private static ConfigEntry<KeyCode> pauseWithoutScreen;
        public static KeyCode PauseWithoutScreen => pauseWithoutScreen.Value;
        private static ConfigEntry<bool> enablePauseWithoutScreen;
        public static bool EnablePauseWithoutScreen => enablePauseWithoutScreen.Value;

        public override void Initialize()
        {
            enableCopySeedFunction = CreateConfig("Enable Copy Seed Function", true, "If true, you will be able copy seed in pause with CTRL+C");
            enableRestartButton = CreateConfig("Enable Restart Button", true, "If true, restart button will appear in pause menu");
            pauseWithoutScreen = CreateConfig("Pause Without Screen Key", KeyCode.Backspace, "Key that will be used to pause without pause screen");
            enablePauseWithoutScreen = CreateConfig("Enable Pause Without Screen", true, "If true, you will be able to pause game without pause screen");
        }
    }
}
