using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System.Linq;
using UnityEngine;

namespace QualityOfPlus.BetterMap.QuickMarkers
{
    public class RemoveMarkerFeature : QOPToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURE.REMOVE.MARKER";

        protected override string EnabledConfigKey => "Remove Marker";
        protected override string EnabledConfigDescription => "Allows you to remove the nearest map marker by pressing the binded key";

        private ConfigEntry<KeyCode> key;

        public override void PostInitialize(QOPCategory category)
        {
            key = category.CreateEntry<KeyCode>("Remove Marker Keybind", KeyCode.Delete, "Key to remove the nearest marker from the map");
        }

        public void OnUpdate()
        {
            if (!IsEnabled())
                return;

            if (!Input.GetKeyDown(key.Value))
                return;

            Map map = BaseGameManager.Instance?.Ec?.map;
            PlayerManager pm = CoreGameManager.Instance?.GetPlayer(0);
            if (pm == null || map == null || map.markers.Count == 0)
                return;

            MapMarker nearest = map.markers
                .OrderBy(m => Vector3.Distance(pm.transform.position, m.environmentMarker.transform.position))
                .First();

            nearest.ShowMarker(false);
            map.DestroyMarker(nearest);
        }
    }
}