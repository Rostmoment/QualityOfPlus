using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QualityOfPlus.BetterUI.TABSwitch
{
    public class TABSwitchFeature : QOPFeature, IToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURE.TAB.SWITCH";

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        private List<StandardMenuButton> Buttons { get; } = new List<StandardMenuButton>();
        private int currentIndex = 0;

        private StandardMenuButton Chosen =>
            Buttons.Count > 0 ? Buttons.ElementAtOrDefault(currentIndex) : null;

        internal void Register(StandardMenuButton button)
        {
            if (!Buttons.Contains(button))
                Buttons.Add(button);
        }

        internal void Unregister(StandardMenuButton button)
        {
            Buttons.Remove(button);
            if (currentIndex >= Buttons.Count)
                currentIndex = 0;
        }

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>(
                "TAB Switching", false,
                "If true, you will be able to switch between buttons with keyboard\n" +
                "TAB - next button\n" +
                "Shift+Tab - previous button\n" +
                "Enter - press button");
        }

        public override void PostInitialize(QOPCategory category) { }

        public void Update()
        {
            if (!this.IsEnabled()) return;

            Buttons.Sort((a, b) =>
            {
                int y = b.transform.position.y.CompareTo(a.transform.position.y);
                return y != 0 ? y : a.transform.position.x.CompareTo(b.transform.position.x);
            });

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    SwitchToPrevious();
                else
                    SwitchToNext();
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                Chosen?.Press();

            Chosen?.Highlight();
        }

        private void SwitchToNext()
        {
            if (Buttons.Count == 0) return;
            currentIndex = (currentIndex + 1) % Buttons.Count;
        }

        private void SwitchToPrevious()
        {
            if (Buttons.Count == 0) return;
            currentIndex = (currentIndex - 1 + Buttons.Count) % Buttons.Count;
        }
    }
}