using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System.Linq;
using UnityEngine;

namespace QualityOfPlus.BetterMap.QuickMarkers
{
    public class AddMarkerFeature : QOPToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURE.ADD.MARKER";

        protected override string EnabledConfigKey => "Add Marker";
        protected override string EnabledConfigDescription => "Allows you to place a specific map marker by holding the modifier key and pressing 1-6";

        private ConfigEntry<KeyCode> modifier;

        private static readonly KeyCode[] markerKeys = new KeyCode[]
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
            KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6
        };
        public override void PostInitialize(QOPCategory category)
        {
            modifier = category.CreateEntry<KeyCode>("Add Marker Modifier Key", KeyCode.LeftAlt, "Hold this key then press 1-6 to place specific marker");
        }

        public void OnUpdate()
        {
            if (!IsEnabled())
                return;

            if (!Input.GetKey(modifier.Value))
                return;

            Map map = BaseGameManager.Instance?.Ec?.map;
            PlayerManager pm = CoreGameManager.Instance?.GetPlayer(0);
            if (pm == null || map == null || map.markers.Count >= 32)
                return;

            for (int i = 0; i < markerKeys.Length; i++)
            {
                if (!Input.GetKeyDown(markerKeys[i]))
                    continue;

                map.AddMarker(BetterMapCategory.WorldToMapScreenPosition(pm.transform.position), i);
                if (map.environmentMarkersVisible)
                    map.markers.Last().ShowMarker(true);
                break;
            }
        }

    }
}