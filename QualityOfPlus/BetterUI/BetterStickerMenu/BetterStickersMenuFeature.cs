using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityOfPlus.BetterUI.BetterStickerMenu
{
    public class BetterStickersMenuFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.BETTER.STICKERS.MENU";

        protected override string EnabledConfigKey => "Better Stickers Menu";
        protected override string EnabledConfigDescription => "Changes original sticker menu to new one with better UI and some new features";

        internal List<StickersSortingMethod> sortingMethods = new List<StickersSortingMethod>();

        public override void PostInitialize(QOPCategory category)
        {
            AddSortingMethod(new StickersSortingMethod("Don't Sort", stickers => stickers));
            AddSortingMethod(new StickersSortingMethod("Quantity", stickers => stickers.OrderBy(x => x.Value).ToArray()));
            AddSortingMethod(new StickersSortingMethod("Quantity Descending", stickers => stickers.OrderByDescending(x => x.Value).ToArray()));
        }

        internal void AddSortingMethod(StickersSortingMethod sortingMethod)
        {
            sortingMethods.Add(sortingMethod);
        }
    }
}
