using MTM101BaldAPI.AssetTools;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.MoreIcons.PickupIcons
{
    public class StickerIconFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.STICKER.ICON";

        protected override string EnabledConfigKey => "Sticker Icon";
        protected override string EnabledConfigDescription => "Add custom map icon for sticker pickups";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
            Icon = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 22, "MapIcons", "StickerIcon.png");
        }

        public Sprite Icon { get; private set; }
    }
}
