using BepInEx;
using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace QualityOfPlus
{
    public abstract class QOPCategory
    {
        /// <summary>
        /// Unique ID of category, must be not empty string, not case sensitive
        /// </summary>
        public abstract string ID { get; }

        /// <summary>
        /// Human readable name for category, used in cofings and options menu
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Called before adding category to categories list
        /// </summary>
        public abstract void PreInitialize();

        /// <summary>
        /// Called after adding category to categories list
        /// </summary>
        public abstract void PostInitialize();


        public PluginInfo PluginInfo { get; private set; }
        private ConfigFile config;

        private List<QOPFeature> features = new List<QOPFeature>();
        public ReadOnlyCollection<QOPFeature> Features => features.AsReadOnly();


        internal void Assign(PluginInfo plugin, ConfigFile config)
        {
            if (this.PluginInfo != null)
                throw new InvalidOperationException($"{GetType().Name} already assigned");

            this.PluginInfo = plugin;
            this.config = config;
        }

        public ConfigEntry<T> CreateEntry<T>(string key, T defaultValue, string description) => config.Bind<T>(Name, key, defaultValue, description);

        public void SetAllToggleables(bool value)
        {
            foreach (QOPFeature feature in features)
            {
                if (feature is IToggleableFeature toggleable)
                    toggleable.TrySetActive(value);
            }
        }

        public void DisableAllToggleables() => SetAllToggleables(false);
        public void EnableAllToggleables() => SetAllToggleables(true);

        
        public T AddFeature<T>() where T : QOPFeature, new()
        {
            T t = new T();
            if (t.ID.IsNullOrWhiteSpace())
                throw new ArgumentException($"ID of feature {typeof(T).FullName} must be not empty string");

            if (GetFeature(t.ID) != null)
                throw new ArgumentException($"Feature with ID {t.ID} already exists in category {Name}! Duplicates are not allowed");

            t.AssignFromCategory(this);

            t.PreInitialize(this);
            features.Add(t);
            t.PostInitialize(this);

            if (t is IUpdatable updatable)
                QOPEvents.Instance.AddUpdatable(updatable);

            return t;
        }


        public QOPFeature GetFeature(string id) => features.FirstOrDefault(x => x.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        public T GetFeature<T>(string id) where T : QOPFeature => features.OfType<T>().FirstOrDefault(x => x.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        public T GetFeature<T>() where T : QOPFeature => features.OfType<T>().FirstOrDefault();
    }
}
