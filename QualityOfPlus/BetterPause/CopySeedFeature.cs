using BepInEx.Configuration;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterPause
{
    public class CopySeedFeature : QOPFeature, IToggleableFeature, IOnAPIStart
    {
        public SoundObject CopySound { get; private set; }
        public override string ID => "QOP.FEATURE.COPY.SEED";

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Enable Copy Seed Function", true, "If true, you will be able copy seed in pause by clicking on it");
        }
        public override void PostInitialize(QOPCategory category)
        {
        }

        public IEnumerator APIStartAction()
        {
            yield return "Creating copy seed sound...";
            CopySound = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(BasePlugin.Instance, "Audio", "CopySeed.mp3"), "", SoundType.Effect, Color.white, 0);
        }
    }
}
