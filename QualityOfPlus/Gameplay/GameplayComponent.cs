using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay
{
    class GameplayComponent : BaseQOLThing
    {
        protected override string CategoryName => "Gameplay";

        private static ConfigEntry<bool> descAnywhere;
        public static bool DescAnywhere => descAnywhere.Value;

        public override void Initialize()
        {
            descAnywhere = CreateConfig("Pickup Description Anywhere", false, "If true, item description will be showed anywhere");
        }
    }
}
