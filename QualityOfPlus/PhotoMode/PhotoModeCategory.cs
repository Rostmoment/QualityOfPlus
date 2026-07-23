using QualityOfPlus.PhotoMode.PanoramaScreenshot;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.PhotoMode
{
    public class PhotoModeCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.PHOTO.MODE";
        public override string Name => "Photo Mode";

        public override void PreInitialize()
        {
            AddFeature<PanoramaScreenshotFeature>();
        }
        public override void PostInitialize()
        {
        }

    }
}
