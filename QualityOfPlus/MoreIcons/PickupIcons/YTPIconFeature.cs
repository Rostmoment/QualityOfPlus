using MTM101BaldAPI.AssetTools;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.MoreIcons.PickupIcons
{
    public class YTPIconFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.YTPICON";

        protected override string EnabledConfigKey => "YTP Icon";
        protected override string EnabledConfigDescription => "Add custom map icon for YTP pickups";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
            Icon = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 70, "MapIcons", "YTPIcon.png");
        }

        public Sprite Icon { get; private set; }
    }
}
