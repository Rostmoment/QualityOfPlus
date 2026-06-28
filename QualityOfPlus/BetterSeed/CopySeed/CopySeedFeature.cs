using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System.Collections;
using UnityEngine;

namespace QualityOfPlus.BetterSeed.CopySeed
{
    public class CopySeedFeature : QOPToggleableFeature, IOnAPIStart
    {
        public override string ID => "QOP.FEATURE.COPY.SEED";

        protected override string EnabledConfigKey => "Copy Seed";
        protected override string EnabledConfigDescription => "Allows copying the seed by clicking it in the pause menu or elevator menu";

        public SoundObject CopySound { get; private set; }

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