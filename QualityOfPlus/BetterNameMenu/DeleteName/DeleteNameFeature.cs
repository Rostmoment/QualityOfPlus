using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System.Linq;
using UnityEngine;

namespace QualityOfPlus.BetterNameMenu.DeleteName
{
    public class DeleteNameFeature : QOPToggleableFeature
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
                    crossMarkPointed = AssetLoader.SpriteFromMod(BasePlugin.Instance,Vector2.one / 2f, 1, "CrossPointed.png");

                return crossMarkPointed;
            }
        }

        public override string ID => "QOP.FEATURE.DELETE.NAME";

        protected override string EnabledConfigKey => "Quick Delete Button";
        protected override string EnabledConfigDescription => "Adds buttons to delete saved names in the name menu";

        public override void PostInitialize(QOPCategory category)
        {
            // Doing it on any API call is too late because game loads name menu before API loading screen
            crossMark = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(x => x.name == "YCTP_IndicatorsSheet_1");
            crossMarkPointed = AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1, "CrossPointed.png");
        }
    }
}