using HarmonyLib;
using MTM101BaldAPI;
using PlusLevelStudio.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode
{
    [ConditionalPatchMod(Compats.LEVEL_STUDIO_GUID)]
    [HarmonyPatch(typeof(EditorModeSelectionMenu))]
    class LevelEditor
    {
        [ConditionalPatchMod(Compats.LEVEL_STUDIO_GUID)]
        [HarmonyPatch(nameof(EditorModeSelectionMenu.Build))]
        [HarmonyPostfix]
        private static void Patch()
        {
            if (!BetterMenuComponent.DarkMode)
                return;

            SceneManager.GetActiveScene().GetRootGameObjects().
                First(x => x.name == "EditorModeSelection").transform.Find("BG").GetComponent<Image>().sprite = BasePlugin.Asset.Get<Sprite>("DarkModeEditor");
        }
    }
}
