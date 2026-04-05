using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using MTM101BaldAPI;
using UnityEngine;

namespace QualityOfPlus.TABSwitch
{
    class TABSwitch : MonoBehaviour
    {
        public static HashSet<TABSwitch> switchers = new HashSet<TABSwitch>();
        private static int currentIndex = 0;
        public static TABSwitch Chosen
        {
            get
            {
                try
                {
                    return switchers.ElementAtOrDefault(currentIndex);
                }
                catch { return null; }
            }
        }

        private StandardMenuButton button;
        private void Start()
        {
            button = GetComponent<StandardMenuButton>();
            if (button == null)
            {
                Destroy(this);
                return;
            }
        }
        private void OnEnable()
        {
            switchers.Add(this);
        }
        public void OnDisable()
        {
            switchers.Remove(this);
        }
        public static void SwitchToPrevious()
        {
            if (switchers.Count == 0) return;
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = switchers.Count - 1;
        }
        public static void SwitchToNext()
        {
            if (switchers.Count == 0) return;
            currentIndex++;
            if (currentIndex >= switchers.Count)
                currentIndex = 0;
        }
        public void UpdateButton()
        {
            if (Chosen == null) return;

            if (Chosen.Equals(this))
            {
                button?.Highlight();
            }
        }
        public void Click()
        {
            button.Press();
        }
    }
}
