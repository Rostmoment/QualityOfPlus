using BepInEx;
using BepInEx.Configuration;
using Rewired.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus
{
    public class QOPManager
    {
        public static QOPManager Instance { get; } = new QOPManager();
        private QOPManager() { }

        
        private List<QOPCategory> categories = new List<QOPCategory>();
        public ReadOnlyCollection<QOPCategory> Categories => categories.AsReadOnly();


        public T RegisterCategory<T>(PluginInfo plugin, ConfigFile file) where T : QOPCategory, new()
        {
            T t = new T();
            if (t.ID.IsNullOrWhiteSpace())
                throw new ArgumentException($"Category {typeof(T).FullName} ID must be not empty string");

            if (GetCategory(t.ID) != null)
                throw new ArgumentException($"Category with ID {t.ID} already exists! Duplicates are not allowed");

            t.Assign(plugin, file);

            t.PreInitialize();
            categories.Add(t);
            t.PostInitialize();

            return t;
        }


        public QOPCategory GetCategory(string id) => categories.FirstOrDefault(x => x.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        public T GetCategory<T>(string id) where T : QOPCategory => categories.OfType<T>().FirstOrDefault(x => x.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        public T GetCategory<T>() where T : QOPCategory => categories.OfType<T>().FirstOrDefault();

        public IEnumerable<QOPFeature> GetAllFeatures() => categories.SelectMany(c => c.Features);
    }
}
