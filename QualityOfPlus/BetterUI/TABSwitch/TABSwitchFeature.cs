using QualityOfPlus.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QualityOfPlus.BetterUI.TABSwitch
{
    public class TABSwitchFeature : QOPToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURE.TAB.SWITCH";

        protected override string EnabledConfigKey => "TAB Switching";
        protected override string EnabledConfigDescription =>
            "Allows switching between buttons with keyboard\n" +
            "TAB - next button\n" +
            "Shift+Tab - previous button\n" +
            "Enter - press button";
        protected override bool DefaultValue => false;

        private readonly List<StandardMenuButton> buttons = new List<StandardMenuButton>();
        private int currentIndex = 0;

        private StandardMenuButton Chosen => buttons.ElementAtOrDefault(currentIndex);

        internal void Register(StandardMenuButton button)
        {
            if (!buttons.Contains(button))
                buttons.Add(button);
        }

        internal void Unregister(StandardMenuButton button)
        {
            buttons.Remove(button);
            if (currentIndex >= buttons.Count)
                currentIndex = 0;
        }

        public void OnUpdate()
        {
            if (!IsEnabled()) return;

            buttons.Sort((a, b) =>
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
            if (buttons.Count == 0) return;
            currentIndex = (currentIndex + 1) % buttons.Count;
        }

        private void SwitchToPrevious()
        {
            if (buttons.Count == 0) return;
            currentIndex = (currentIndex - 1 + buttons.Count) % buttons.Count;
        }

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}