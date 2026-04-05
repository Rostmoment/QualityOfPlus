using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using UnityEngine;

namespace QualityOfPlus.BetterMap
{
    [HarmonyPatch(typeof(Shader))]
    class RoomIconsOnQuickMap
    {
        private static readonly Dictionary<RoomController, Texture2D> cachedTextures = new Dictionary<RoomController, Texture2D>();

        [HarmonyPatch(nameof(Shader.DisableKeyword))]
        [HarmonyPostfix]
        private static void AddRoomIconsToQuickMap(string keyword)
        {
            Shader.SetGlobalColor("_KEYMAPROOMICONCOLOR", BetterMapComponent.RoomIconsOnQuickMap ? Color.white : Color.clear);
            if (keyword == "_KEYMAPSHOWBACKGROUND" && BetterMapComponent.RoomIconsOnQuickMap)
                Shader.EnableKeyword("_KEYMAPSHOWBACKGROUND");
        }
    }
}
