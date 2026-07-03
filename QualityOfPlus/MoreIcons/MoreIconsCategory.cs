using MTM101BaldAPI;
using QualityOfPlus.MoreIcons.PickupIcons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.MoreIcons
{
    public class MoreIconsCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.MOREICONS";

        public override string Name => "More Map Icons";

        public override void PreInitialize()
        {
            AddFeature<YTPIconFeature>();
            AddFeature<StickerIconFeature>();
        }
        public override void PostInitialize()
        {
        }

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
            icon.spriteRenderer.material = new Material(Resources.FindObjectsOfTypeAll<MapIcon>().First(x => x.name == "Icon_Prefab").spriteRenderer.material);
            cache.Add(name, icon);
            return icon;
        }
    }
}
