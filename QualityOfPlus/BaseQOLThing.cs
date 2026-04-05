using BepInEx.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus
{
    abstract class BaseQOLThing : MonoBehaviour
    {
        protected abstract string CategoryName { get; }
        protected ConfigEntry<T> CreateConfig<T>(string name, T defaultValue, string description)
        {
            return BasePlugin.Instance.Config.Bind(CategoryName, name, defaultValue, description);
        }

        private void Update() => VirtualUpdate();
        public virtual void VirtualUpdate()
        {
        }

        private void OnEnable() => VirtualOnEnable();
        public virtual void VirtualOnEnable()
        {
        }

        private void OnDisable() => VirtualOnDisable();
        public virtual void VirtualOnDisable()
        {
        }

        public virtual void Initialize()
        {

        }

        public virtual IEnumerator OnAPIFinal()
        {
            yield return $"Calling {this.GetType()} OnAPIFinal";
        }
        public virtual IEnumerator OnAPIPost()
        {
            yield return $"Calling {this.GetType()} OnAPIPost";
        }
        public virtual IEnumerator OnAPIPre()
        {
            yield return $"Calling {this.GetType()} OnAPIPre";
        }
        public virtual IEnumerator OnAPIStart()
        {
            yield return $"Calling {this.GetType()} OnAPIStart";
        }
    }
}
