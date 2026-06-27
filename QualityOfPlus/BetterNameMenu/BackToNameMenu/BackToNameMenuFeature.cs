using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterNameMenu.BackToNameMenu
{
    public class BackToNameMenuFeature : QOPFeature, IToggleableFeature
    {
        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.BACK.TO.NAME.MENU";

        public override void PostInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Back Button", true, "Adds an button to return to the name entry menu from the main menu");
        }

        public override void PreInitialize(QOPCategory category)
        {
        }

        internal void OnBackButtonPressed()
        {

        }
    }
}
