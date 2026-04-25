using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap
{
    class BetterMapComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Map";

        private static ConfigEntry<KeyCode> addMarker;
        private static ConfigEntry<bool> addMarkerEnable;
        private static ConfigEntry<KeyCode> removeMarker;
        private static ConfigEntry<bool> removeMarkerEnable;
        private static ConfigEntry<KeyCode> addRandomMarker;
        private static ConfigEntry<bool> addRandomMarkerEnable;

        private static ConfigEntry<bool> roomIconsOnQuickMap;
        private static ConfigEntry<bool> timerOnQuickMap;

        private static ConfigEntry<Color> customGridColor;

        public static KeyCode AddMarker => addMarker.Value;
        public static bool AddMarkerEnable => addMarkerEnable.Value;
        public static KeyCode RemoveMarker => removeMarker.Value;
        public static bool RemoveMarkerEnable => removeMarkerEnable.Value;
        public static KeyCode AddRandomMarker => addRandomMarker.Value;
        public static bool AddRandomMarkerEnable => addRandomMarkerEnable.Value;

        public static bool RoomIconsOnQuickMap => roomIconsOnQuickMap.Value;
        public static bool TimerOnQuickMap => timerOnQuickMap.Value;

        public static Color CustomGridColor => customGridColor.Value;

        public override void Initialize()
        {
            addMarker = CreateConfig("Add Marker", KeyCode.LeftControl, "Key to quickly place marker to map");
            addMarkerEnable = CreateConfig("Enable Quick Map Marker", true, "If true, you can place marker to map with key+1-6 number");

            removeMarker = CreateConfig("Remove Marker", KeyCode.RightShift, "Key to quickly remove marker from map");
            removeMarkerEnable = CreateConfig("Enable Quick Map Marker Remove", true, "If true, you can remove nearest marker from map with key");

            addRandomMarker = CreateConfig("Add Random Marker", KeyCode.RightControl, "Key to quickly place random marker to map");
            addRandomMarkerEnable = CreateConfig("Enable Add Random Marker", true, "If true, you can place random marker with key");

            roomIconsOnQuickMap = CreateConfig("Room Icons On Quick Map", true, "If true, you will be able too see room icons even on quick map");

            timerOnQuickMap = CreateConfig("Show Timer On Quick Map", true, "If true, there will be text on quick map that shows timer before lights out event");

            customGridColor = CreateConfig("Custom Grid Color", new Color(0, 0.3922f, 0, 1), "Custom color for map grid\nIn game color by default");
        }
    }
}
