using QualityOfPlus.BetterMap.CustomGridColor;
using QualityOfPlus.BetterMap.QuickMarkers;
using QualityOfPlus.BetterMap.TimerOnQuickMap;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap
{
    public class BetterMapCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.BETTER.MAP";
        public override string Name => "Better Map";


        public override void PreInitialize()
        {
            AddFeature<AddRandomMarkerFeature>();
            AddFeature<AddMarkerFeature>();
            AddFeature<RemoveMarkerFeature>();
            AddFeature<CustomGridColorFeature>();
            AddFeature<TimerOnQuickMapFeature>();
        }
        public override void PostInitialize()
        {
        }
        public static Vector3 WorldToMapScreenPosition(Vector3 worldPosition) => new Vector3(worldPosition.x / 10f - 0.5f, worldPosition.z / 10f - 0.5f, 0f);
    }
}
