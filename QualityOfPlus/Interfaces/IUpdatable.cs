using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Interfaces
{
    /// <summary>
    /// Implement this on your <see cref="QOPFeature"/> to receive a per-frame Update call
    /// from QOP's single internal <see cref="QOPEvents"/> MonoBehaviour.<br/>
    /// QOP calls <see cref="Update"/> automatically every frame — you don't need your own MonoBehaviour for this.
    /// </summary>
    public interface IUpdatable
    {
        void Update();
    }
}
