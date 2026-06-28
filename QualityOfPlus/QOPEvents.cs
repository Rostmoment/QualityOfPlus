using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus
{
    internal class QOPEvents : Singleton<QOPEvents>
    {
        private List<IUpdatable> updatables = new List<IUpdatable>();
        internal void AddUpdatable(IUpdatable updatable) => updatables.Add(updatable);
        private void Update()
        {
            foreach (IUpdatable updatable in updatables)
                updatable.OnUpdate();
        }
    }
}
