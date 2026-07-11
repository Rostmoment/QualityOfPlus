using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator.QuickTV
{
    public class QuickElevatorTVFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.QUICK.ELEVATOR.TV";

        protected override string EnabledConfigKey => "Quick Elevator TV";
        protected override string EnabledConfigDescription => "Always skip the elevator result TV";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
