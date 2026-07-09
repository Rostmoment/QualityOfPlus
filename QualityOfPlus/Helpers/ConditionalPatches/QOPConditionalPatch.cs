using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.ConditionalPatches
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    abstract class QOPConditionalPatch : Attribute
    {
        public abstract bool ShouldPatch();
    }
}
