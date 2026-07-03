using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMenu.DarkMode
{
    public class DarkModeFeature : QOPToggleableFeature, IOnAPIStart, IOnAPIFinal
    {
        public override string ID => "QOP.FEATURE.DARK.MODE";

        protected override string EnabledConfigKey => "Dark Mode";
        protected override string EnabledConfigDescription => "Adds dark mode to menus";
        protected override bool DefaultValue => false;

        #region level studio
        public Sprite LevelStudioToolbox { get; private set; }
        public Sprite EditorDarkMode { get; private set; }
        #endregion

        #region base game
        public Sprite OptionsDarkMode { get; private set; }
        public Sprite NameEntryBackgroundDarkMode { get; private set; }
        public Sprite ExitNotHighlitghedDarkMode { get; private set; }
        public Sprite ExitHighlitghedDarkMode { get; private set; }
        public Sprite MainMenuDarkMode { get; private set; }
        public Sprite WhiteCheckBox { get; private set; }
        #endregion

        public override void PostInitialize(QOPCategory category)
        {
            NameEntryBackgroundDarkMode = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "DarkMode", "NameEntry.png");
            EditorDarkMode = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1f, "DarkMode", "Editor", "Menu.png");
        }
        

        public IEnumerator APIStartAction()
        {
            yield return "Creating dark mode textures...";
            OptionsDarkMode = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "DarkMode", "OptionsMenu.png");
            ExitNotHighlitghedDarkMode = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "DarkMode", "ExitNotHighlitghed.png");
            ExitHighlitghedDarkMode = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "DarkMode", "ExitHighlitghed.png");
            MainMenuDarkMode = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "DarkMode", "MainMenu.png");
            WhiteCheckBox = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "DarkMode", "WhiteCheckBox.png");
        }

        public IEnumerator APIFinalAction()
        {
            yield return "Applying options menu dark mode...";
            CustomOptionsCore.OnMenuInitialize += OptionsMenuDarkMode.ApplyDarkMode;
        }
    }
}
