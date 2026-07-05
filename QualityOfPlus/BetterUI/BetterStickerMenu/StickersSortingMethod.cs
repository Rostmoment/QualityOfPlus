using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterUI.BetterStickerMenu
{
    internal class StickersSortingMethod
    {
        public string Name { get; private set; }
        public Func<ExtendedInventorySticker[], ExtendedInventorySticker[]> Sorting { get; private set; }

        public StickersSortingMethod(string name, Func<ExtendedInventorySticker[], ExtendedInventorySticker[]> sorting)
        {
            Name = name;
            Sorting = sorting;
        }
    }
}
