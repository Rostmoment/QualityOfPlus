using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.TABSwitch
{
    class TABSwitcherComponent : BaseQOLThing
    {
        protected override string CategoryName => "TAB Switch";

        private static ConfigEntry<bool> enableTABSwitching;
        public static bool Enabled => enableTABSwitching.ValueOrDefault(false);

        public override void VirtualUpdate()
        {
            if (!enableTABSwitching.Value) return;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    TABSwitch.SwitchToPrevious();
                else
                    TABSwitch.SwitchToNext();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                TABSwitch.Chosen?.Click();
            }
            TABSwitch.switchers = new HashSet<TABSwitch>(TABSwitch.switchers.OrderByDescending(x => x.transform.position.y).ThenBy(x => x.transform.position.x));
        }


        public override void Initialize()
        {
            enableTABSwitching = CreateConfig("Enable TAB Switching", false, "If true, you will be able switch between buttons with keyboard\nTAB - next button\nShift+Tab - previous button\nEnter - press button");
        }
    }
}
