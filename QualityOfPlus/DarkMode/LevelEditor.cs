using HarmonyLib;
using MTM101BaldAPI;
using PlusLevelStudio.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QualityOfPlus.DarkMode
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
            if (!DarkModeComponent.DarkMode)
                return;

            SceneManager.GetActiveScene().GetRootGameObjects().
                Find(x => x.name == "EditorModeSelection").transform.Find("BG").GetComponent<Image>().sprite = BasePlugin.Asset.Get<Sprite>("DarkModeEditor");
        }
    }
}
