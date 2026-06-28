using BepInEx.Configuration;
using QualityOfPlus.Interfaces;

namespace QualityOfPlus
{
    public abstract class QOPToggleableFeature : QOPFeature, IToggleableFeature
    {
        private ConfigEntry<bool> enabled;

        protected abstract string EnabledConfigKey { get; }
        protected abstract string EnabledConfigDescription { get; }
        protected virtual bool DefaultValue => true;

        protected virtual void OnPreInitialize(QOPCategory category) { }

        public sealed override void PreInitialize(QOPCategory category)
        {
            OnPreInitialize(category);
            enabled = category.CreateEntry<bool>(EnabledConfigKey, DefaultValue, EnabledConfigDescription);
        }


        public bool IsEnabled() => enabled?.Value ?? DefaultValue;

        public bool TrySetActive(bool value)
        {
            if (enabled == null) return false;
            enabled.Value = value;
            return true;
        }
    }
}