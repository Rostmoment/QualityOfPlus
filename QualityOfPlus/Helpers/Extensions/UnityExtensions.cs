using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Helpers.Extensions
{
    static class UnityExtensions
    {
        public static bool IsNullOrDestroyed(this object obj)
        {
            try
            {
                if (obj == null) { return true; }
                else if (obj is UnityEngine.Object unityObj && !unityObj) { return true; }
                return false;
            }
            catch { return true; }
        }
    }
}
