using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator
{
    class BetterElevatorComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Elevator";

        private static ConfigEntry<bool> oldButtons;
        private static ConfigEntry<bool> pitstopTrigger;

        public static bool OldButtons => oldButtons.Value;
        public static bool PitstopTrigger => pitstopTrigger.Value;

        public override void Initialize()
        {
            
            oldButtons = CreateConfig<bool>("Old Buttons", false, "If true, the elevator will use the old buttons as before 0.14");
            pitstopTrigger = CreateConfig<bool>("Pitstop Trigger", false, "If true, the elevator in pitstop will have exit trigger instead of green button");
        }
    }
}
