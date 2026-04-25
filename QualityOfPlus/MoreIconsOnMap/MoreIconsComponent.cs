using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.MoreIconsOnMap
{
    class MoreIconsComponent : BaseQOLThing
    {
        protected override string CategoryName => "More Map Icons";

        private static ConfigEntry<bool> ytp;
        private static ConfigEntry<bool> tapePlayer;
        private static ConfigEntry<bool> sticker;

        public static bool YTP => ytp.Value;
        public static bool TapePlayer => tapePlayer.Value;
        public static bool Sticker => sticker.Value;

        public override void Initialize()
        {
            ytp = CreateConfig("YTP Icon", false, "Enable YTP icon");
            tapePlayer = CreateConfig("Tape Player Icon", false, "Enable tape player icon");
            sticker = CreateConfig("Sticker Icon", false, "Enable sticker icon");
        }
    }
}
