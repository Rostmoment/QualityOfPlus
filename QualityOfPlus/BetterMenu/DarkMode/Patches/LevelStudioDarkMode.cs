using HarmonyLib;
using PlusLevelStudio.Editor;
using PlusLevelStudio.Menus;
using QualityOfPlus.ConditionalPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QualityOfPlus.BetterMenu.DarkMode.Patches
{
    [QOPConditionalPatchMod(Compats.LEVEL_STUDIO_GUID)]
    [HarmonyPatch]
    internal class LevelStudioDarkMode
    {
        [HarmonyPatch(typeof(EditorModeSelectionMenu), nameof(EditorModeSelectionMenu.Build))]
        [HarmonyPostfix]

        private static void BlackMenu()
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled(out DarkModeFeature feature))
                return;

            SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "EditorModeSelection").transform.Find("BG").GetComponent<Image>().sprite = feature.EditorDarkMode;
        }

    }
}
