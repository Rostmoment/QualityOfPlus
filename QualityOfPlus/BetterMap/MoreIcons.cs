using HarmonyLib;
using Mono.Cecil;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterMap
{
    [HarmonyPatch]
    class MoreIcons
    {
        #region create 
        private static Dictionary<string, MapIcon> cache = new Dictionary<string, MapIcon>();
        public static T CreateMapIcon<T>(string name, Sprite spr = null) where T : MapIcon
        {
            if (cache.ContainsKey(name)) 
                return (T)cache[name];
            T icon = new GameObject(name).AddComponent<T>();
            icon.spriteRenderer = icon.gameObject.AddComponent<SpriteRenderer>();
            icon.name = name;
            if (spr != null)
                icon.spriteRenderer.sprite = spr;
            icon.gameObject.ConvertToPrefab(true);
            icon.gameObject.layer = LayerMask.NameToLayer("Map");
            icon.spriteRenderer.material = new Material(Resources.FindObjectsOfTypeAll<MapIcon>().Find(x => x.name == "Icon_Prefab").spriteRenderer.material);
            cache.Add(name, icon);
            return icon;
        }
        #endregion

        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.ApplyMap))]
        [HarmonyPostfix]
        private static void CustomIcons(Map map)
        {
            if (BetterMapComponent.TapeIcon)
            {
                foreach (TapePlayer tape in UnityEngine.Object.FindObjectsOfType<TapePlayer>())
                {
                    if (tape != null)
                        if (tape.requiredItem == Items.Tape)
                            map.AddIcon(CreateMapIcon<MapIcon>("TapePlayerIcon", BasePlugin.Asset.Get<Sprite>("TapePlayerIcon")), tape.transform, Color.white);
                }
            }
        }
        
        [HarmonyPatch(typeof(Pickup), "Start")]
        [HarmonyPatch(typeof(Pickup), "AssignItem")]
        [HarmonyPostfix]
        private static void YRPIcon(Pickup __instance)
        {
            if (__instance?.icon?.spriteRenderer == null || !BetterMapComponent.YTPIcon)
                return;

            if (__instance.item.itemType == Items.Points) 
                __instance.icon.spriteRenderer.sprite = BasePlugin.Asset.Get<Sprite>("YTPMapIcon");
        }
    }
}
