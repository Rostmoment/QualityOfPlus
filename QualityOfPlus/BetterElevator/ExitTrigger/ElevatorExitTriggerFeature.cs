using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterElevator.ExitTrigger
{
    public class ElevatorExitTriggerFeature : QOPFeature, IToggleableFeature
    {
        private HashSet<Type> typesForFeature = new HashSet<Type>();

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.ELEVATOR.EXIT.TRIGGER";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Elevator Exit Trigger", false, "If true, old elevator trigger will be used for exit instead of green button in pitstop");
        }
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
