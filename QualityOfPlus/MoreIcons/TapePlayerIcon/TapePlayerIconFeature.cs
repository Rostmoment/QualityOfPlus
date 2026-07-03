using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.MoreIcons.TapePlayerIcon
{
    public class TapePlayerIconFeature : QOPToggleableFeature, IOnAPIStart
    {
        public override string ID => "QOP.FEATURE.TAPE.PLAYER.ICON";

        protected override string EnabledConfigKey => "Tape Player Icon";
        protected override string EnabledConfigDescription => "Add custom icon for tape player";

        public override void PostInitialize(QOPCategory category)
        {
        }

        public IEnumerator APIStartAction()
        {
            yield return "Creating icon for tape player";
            Icon = MoreIconsCategory.CreateMapIcon<MapIcon>("TapePlayerIcon", AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 22, "MapIcons", "TapePlayerIcon.png"));
        }

        public MapIcon Icon { get; private set; }
    }
}
