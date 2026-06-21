using BepInEx.Configuration;
using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterNameMenu.DeleteName
{
    public class DeleteNameFeature : QOPFeature, IToggleableFeature
    {
        private Sprite crossMark;
        internal Sprite CrossMark
        {
            get
            {
                if (crossMark == null)
                    crossMark = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "YCTP_IndicatorsSheet_1");
                return crossMark;
            }
        }
        private Sprite crossMarkPointed;
        internal Sprite CrossMarkPointed
        {
            get
            {
                if (crossMarkPointed == null)
                    crossMarkPointed = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "CrossPointed.png");
                return crossMarkPointed;
            }
        }

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.DELETE.NAME";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Quick Delete Button", true, "If true, you will be able to delete saved names from name entry menu");
        }

        public override void PostInitialize(QOPCategory category)
        {
            // Doing it on any API call is too late because game loads name menu before API loading screen
            crossMark = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(x => x.name == "YCTP_IndicatorsSheet_1");
            crossMarkPointed = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "CrossPointed.png");
        }
    }
}