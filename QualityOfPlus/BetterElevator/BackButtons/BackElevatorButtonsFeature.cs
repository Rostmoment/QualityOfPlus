using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator.BackButtons
{
    public class BackElevatorButtonsFeature : QOPFeature, IToggleableFeature
    {
        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.BACK.ELEVATOR.BUTTONS";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Old Buttons", false, "If true, the elevator will use the old buttons as before 0.14");
        }
        public override void PostInitialize(QOPCategory category)
        {
            
        }

        public bool ButtonsShouldAppear()
        {
            return this.IsEnabled(); // TODO: add methods for mods to lock/force buttons
        }

    }
}
