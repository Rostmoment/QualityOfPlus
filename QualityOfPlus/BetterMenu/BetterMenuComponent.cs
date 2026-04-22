using BepInEx.Configuration;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using PlusLevelStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMenu.DarkMode
{
    class BetterMenuComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Menu";

        private static ConfigEntry<bool> darkMode;
        private static ConfigEntry<bool> floorSelect;
        public static bool DarkMode => darkMode.Value;
        public static bool FloorSelect => floorSelect.Value;

        public override void Initialize()
        {
            darkMode = CreateConfig("Enable Dark Mode", false, "Enables dark mode for various menus in the game");
            floorSelect = CreateConfig("Enable Floor Select buttons", false, "Enables floor select buttons that mystman uses for debug");
        }

        public override IEnumerator OnAPIFinal()
        {
            yield return "Applying dark mode...";
            CustomOptionsCore.OnMenuInitialize += OptionsMenuDarkMode.ApplyDarkMode;
        }

    }
}
