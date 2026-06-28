using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TransitionManager
{
    public enum CustomUiTransition
    {
        SameAsDefault,
        SwipeLeft,
        SwipeRight,
        Dither
    }
    public class TransitionManagerFeature : QOPFeature, IToggleableFeature
    {
        public bool Enabled { get; private set; }

        private ConfigEntry<float> multiplier;
        public float Multiplier => multiplier.Value;

        private ConfigEntry<float> addend; 
        public float Addend => addend.Value;

        private ConfigEntry<CustomUiTransition> transition; 
        public CustomUiTransition Transition => transition.Value;

        public override string ID => "QOP.FEATURE.TRANSITION.MANAGER";

        public override void PreInitialize(QOPCategory category)
        {
            multiplier = category.CreateEntry<float>("Multiplier", 1, "Multiplier for transition duration");
            addend = category.CreateEntry<float>("Addend", 0f, "Addend for transition duration");
            transition = category.CreateEntry<CustomUiTransition>("Transition", CustomUiTransition.SameAsDefault, "What transition should be used instead of default\nMay look ugly or slow in some cases");
            TrySetActive(true);
        }
        public override void PostInitialize(QOPCategory category)
        {
        }

        public bool IsEnabled() => Enabled;

        public bool TrySetActive(bool value)
        {
            Enabled = value;
            return true;
        }
    }
}
