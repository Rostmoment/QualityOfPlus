using QualityOfPlus.BetterElevator.BackButtons;
using QualityOfPlus.BetterElevator.ExitTrigger;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator
{
    public class BetterElevatorCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.ELEVATOR";
        public override string Name => "Better Elevator";

        public override void PreInitialize()
        {
            AddFeature<ElevatorExitTriggerFeature>();
            AddFeature<BackElevatorButtonsFeature>();
        }
        public override void PostInitialize()
        {
        }

    }
}
