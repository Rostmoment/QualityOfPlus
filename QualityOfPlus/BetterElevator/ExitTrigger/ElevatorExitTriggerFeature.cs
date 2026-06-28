using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;

namespace QualityOfPlus.BetterElevator.ExitTrigger
{
    public class ElevatorExitTriggerFeature : QOPToggleableFeature
    {
        private readonly HashSet<Type> typesForFeature = new HashSet<Type>();

        public override string ID => "QOP.FEATURE.ELEVATOR.EXIT.TRIGGER";

        protected override string EnabledConfigKey => "Elevator Exit Trigger";
        protected override string EnabledConfigDescription => "Restore the old elevator exit trigger instead of the green pit stop button";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
            AddTriggerTo<PitstopGameManager>();
        }

        public void AddTriggerTo<T>() where T : BaseGameManager => AddTriggerTo(typeof(T));

        public void AddTriggerTo(Type type) => typesForFeature.Add(type);

        public bool IsTriggerFor<T>() where T : BaseGameManager => IsTriggerFor(typeof(T));

        public bool IsTriggerFor(Type type) => typesForFeature.Contains(type);
    }
}