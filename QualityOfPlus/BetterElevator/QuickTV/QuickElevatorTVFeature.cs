using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator.QuickTV
{
    public class QuickElevatorTVFeature : QOPFeature, IToggleableFeature
    {
        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.QUICK.ELEVATOR.TV";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Quick Elevator TV", false, $"If true, elevator result TV will always be skipped");
        }

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
