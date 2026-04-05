using HarmonyLib;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.TABSwitch
{
    [HarmonyPatch(typeof(StandardMenuButton))]
    class ApplyTABSwitcher
    {
        [HarmonyPatch("Update")]
        private static void Postfix(StandardMenuButton __instance)
        {
            if (TABSwitcherComponent.Enabled)
                __instance.gameObject.GetOrAddComponent<TABSwitch>().UpdateButton();
        }
    }
}
