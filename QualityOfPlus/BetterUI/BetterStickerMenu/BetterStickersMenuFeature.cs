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
            AddSortingMethod("Last Gotten", stickers => stickers);
            AddSortingMethod("First Gotten", stickers => stickers.Reverse().ToArray());
            AddSortingMethod("Quantity", stickers => stickers.OrderBy(x => x.Value).ToArray());
            AddSortingMethod("Quantity Descending", stickers => stickers.OrderByDescending(x => x.Value).ToArray());
            AddSortingMethod("Alphabetical", stickers => stickers.OrderBy(x => NormalizeTitle(x.Data.GetLocalizedStickerTitle(null)), StringComparer.OrdinalIgnoreCase).ToArray());
            AddSortingMethod("Alphabetical Reversed", stickers => stickers.OrderByDescending(x => NormalizeTitle(x.Data.GetLocalizedStickerTitle(null)), StringComparer.OrdinalIgnoreCase).ToArray());
        }
        private static string NormalizeTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            StringBuilder builder = new StringBuilder(title.Length);
            foreach (char c in title)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    builder.Append(c);
            }
            return builder.ToString();
        }

        internal void AddSortingMethod(string name, Func<ExtendedInventorySticker[], ExtendedInventorySticker[]> sorter)
        {
            AddSortingMethod(new StickersSortingMethod(name, sorter));
        }
        internal void AddSortingMethod(StickersSortingMethod sortingMethod)
        {
            sortingMethods.Add(sortingMethod);
        }

        public override bool IsEnabled()
        {
            return base.IsEnabled() && !Compats.BetterStickerUIInstalled;
        }
    }
}