using BepInEx.Configuration;
using QualityOfPlus.Helpers.Extensions;
using QualityOfPlus.Interfaces;
using Rewired.Utils.Classes.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.PhotoMode.PanoramaScreenshot
{
    public class PanoramaScreenshotFeature : QOPToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURES.PANORAMA.SCREENSHOT";

        protected override string EnabledConfigKey => "Panorama Screenshot";
        protected override string EnabledConfigDescription => "Allows to make panorama screenshots by pressing bound key";
        protected override bool DefaultValue => false;

        private ConfigEntry<KeyCode> bind;
        private ConfigEntry<bool> usePng;
        private ConfigEntry<int> width;

        public override void PostInitialize(QOPCategory category)
        {
            bind = category.CreateEntry<KeyCode>("Panorama Screenshot Key", KeyCode.Home, "Key to press to make panorama screenshot");
            usePng = category.CreateEntry<bool>("Use PNG For Panorama", false, "If true, PNG file format will be used instead of JPEG");
            width = category.CreateEntry<int>("Width For Panorama", 2048, "Width for panorama, higher value means better quality. Values will auto-round to the next power of two");
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(bind.Value) && IsEnabled() && !CoreGameManager.Instance.IsNullOrDestroyed())
            {
                byte[] bytes = I360Render.Capture(width.Value, !usePng.Value);
                File.WriteAllBytes("360.png", bytes);
            }
        }
    }
}
