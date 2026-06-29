using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap.QuickMarkers
{
    public class AddRandomMarkerFeature : QOPToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURE.ADD.RANDOM.MARKER";

        protected override string EnabledConfigKey => "Add Random Marker";
        protected override string EnabledConfigDescription => "Allows you to quickly place random map marker by pressing binded key";

        private ConfigEntry<KeyCode> key;

        public override void PostInitialize(QOPCategory category)
        {
            key = category.CreateEntry<KeyCode>("Add Random Marker Keybind", KeyCode.RightShift, "Key to quickly place random marker to map");
        }
        public void OnUpdate()
        {
            if (!IsEnabled())
                return;

            if (Input.GetKeyDown(key.Value))
            {
                Map map = BaseGameManager.Instance?.Ec?.map;
                PlayerManager pm = CoreGameManager.Instance?.GetPlayer(0);
                if (pm == null || map == null)
                    return;

                Vector3 position = pm.transform.position;

                map.AddMarker(BetterMapCategory.WorldToMapScreenPosition(position), UnityEngine.Random.Range(0, 6));
                if (map.environmentMarkersVisible)
                    map.markers.Last().ShowMarker(true);
            }
        }
    }
}
