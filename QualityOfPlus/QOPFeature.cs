using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus
{
    public abstract class QOPFeature
    {
        /// <summary>
        /// Unique ID of feature, must be not empty string, not case sensitive
        /// </summary>
        public abstract string ID { get; }

        public QOPCategory Category { get; private set; }

        internal void AssignFromCategory(QOPCategory category)
        {
            Category = category;
        }

        /// <summary>
        /// Called before adding feature to features list
        /// </summary>
        public abstract void PreInitialize(QOPCategory category);

        /// <summary>
        /// Called after adding feature to features list
        /// </summary>
        public abstract void PostInitialize(QOPCategory category);
    }
}
