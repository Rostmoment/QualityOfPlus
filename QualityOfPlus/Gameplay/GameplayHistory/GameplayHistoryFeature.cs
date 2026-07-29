using BepInEx.Configuration;
using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.Gameplay.GameplayHistory
{
    public class GameplayHistoryFeature : QOPToggleableFeature, IOnAPIStart
    {
        public override string ID => "QOP.FEATURE.GAMEPLAY.HISTORY";

        protected override string EnabledConfigKey => "Gameplay History";
        protected override string EnabledConfigDescription => "Save history of played seeds";

        public Sprite Highlighted { get; private set; }
        public Sprite Unhighlighted { get; private set; }

        public override void PostInitialize(QOPCategory category)
        {
        }
        public IEnumerator APIStartAction()
        {
            yield return "Creating gameplay history button sprites...";
            Sprite[] sprites = AssetLoader.SpritesFromSpritesheet(2, 1, 1, Vector2.one / 2f, AssetLoader.TextureFromMod(BasePlugin.Instance, "History.png"));
            Highlighted = sprites[0];
            Unhighlighted = sprites[1];
        }
    }
}
