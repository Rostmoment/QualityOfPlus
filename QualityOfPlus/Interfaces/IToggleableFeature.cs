using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Interfaces
{
    /// <summary>
    /// Interface for feature that can be turned on/off at any time
    /// </summary>
    public interface IToggleableFeature
    {
        /// <summary>
        /// Value to return if entry is null
        /// </summary>
        bool ValueIfNull { get; }

        /// <summary>
        /// Entry that stores if feature is enabled or disabled
        /// </summary>
        ConfigEntry<bool> Enabled { get; }
    }

    public static class IToggleableFeatureExtensions
    {
        public static bool IsEnabled(this IToggleableFeature feature)
        {
            if (feature.Enabled == null)
                return feature.ValueIfNull;

            return feature.Enabled.Value;
        }

        public static bool TrySetActive(this IToggleableFeature feature, bool enabled)
        {
            if (feature.Enabled == null)
                return false; 

            feature.Enabled.Value = enabled;
            return true;
        }
    }
}
