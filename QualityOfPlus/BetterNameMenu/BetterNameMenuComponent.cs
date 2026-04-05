using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterNameMenu
{
    class BetterNameMenuComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Name Menu";

        private static ConfigEntry<bool> backButton;
        private static ConfigEntry<bool> deleteButton;
        public static bool BackButton => backButton.Value;
        public static bool DeleteButton => deleteButton.Value;
        public override void Initialize()
        {
            backButton = CreateConfig("Back Button", true, "If true, you will be able to go back to name entry menu from main menu");
            deleteButton = CreateConfig("Delete Button", true, "If true, you will be able to delete names from the name entry menu");
        }
    }
}
