using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Interfaces
{
    /// <summary>
    /// Marks a <see cref="QOPFeature"/> as toggleable at runtime.<br/>
    /// Use <see cref="IsEnabled"/> to check state and <see cref="TrySetActive"/> to change it.<br/>
    /// Implement via <see cref="QOPToggleableFeature"/> — it handles the config boilerplate for you.
    /// </summary>
    public interface IToggleableFeature
    {
        bool IsEnabled();
        bool TrySetActive(bool value);
    }
}
