using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Interfaces
{
    /// <summary>
    /// Interface for feature that can be toggled on/off in QOP options menu
    /// </summary>
    public interface IOptionsToggleableFeature : IToggleableFeature
    {
        string OptionToggleDescription { get; }
        string OptionToggleText { get; }
    }
}
