using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TransitionManager
{
    class TransitionManagerComponent : BaseQOLThing
    {
        protected override string CategoryName => "Transition Manager";

        private static ConfigEntry<float> multiplier;
        private static ConfigEntry<float> addend;
        private static ConfigEntry<CustomUiTransition> transition;

        public static float Multiplier => multiplier.ValueOrDefault(1);
        public static float Addend => addend.ValueOrDefault(0);
        public static CustomUiTransition Transition => transition.ValueOrDefault(CustomUiTransition.SameAsDefault);

        public override void Initialize()
        {
            multiplier = CreateConfig("Multiplier", 1f, "Multiplier for transition duration");
            addend = CreateConfig("Addend", 0f, "Addend for transition duration");
            transition = CreateConfig("Transition", CustomUiTransition.SameAsDefault, "What transition should be used instead of default\nMay look ugly or slow in some cases");
        }
    }
}
