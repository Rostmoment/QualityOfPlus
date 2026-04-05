using BepInEx.Configuration;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using PlusLevelStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.DarkMode
{
    class DarkModeComponent : BaseQOLThing
    {
        protected override string CategoryName => "Dark Mode";

        private static ConfigEntry<bool> darkMode;
        public static bool DarkMode => darkMode.Value;

        public override void Initialize()
        {
            darkMode = CreateConfig("Enable Dark Mode", false, "Enables dark mode for various menus in the game");
        }

        public override IEnumerator OnAPIFinal()
        {
            yield return "Applying dark mode...";
            CustomOptionsCore.OnMenuInitialize += OptionsMenuDarkMode.ApplyDarkMode;
        }

    }
}
