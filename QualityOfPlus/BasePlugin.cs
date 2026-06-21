using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.DiscordSocialSDK;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using System.Collections;
using UnityEngine;
using MTM101BaldAPI;
using MTM101BaldAPI.OptionsAPI;
using System.IO;
using System.Linq;
using QualityOfPlus.ConditionalPatches;
using QualityOfPlus.Interfaces;
using System.Collections.Generic;
using QualityOfPlus.BetterPause;
using QualityOfPlus.BetterGameWindow;
using QualityOfPlus.BetterNameMenu;
using QualityOfPlus.Gameplay;

namespace QualityOfPlus
{
    public class MyPluginInfo
    {
        public const string NAME = "Quality Of Plus";
        public const string GUID = "rost.moment.baldiplus.qop";
        public const string VERSION = "1.9.2.2";
    }

    [BepInDependency(MTM101BaldiDevAPI.ModGUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(Compats.LEVEL_STUDIO_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(DiscordSocialSDKPlugin.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.GUID, MyPluginInfo.NAME, MyPluginInfo.VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {

        public new static ManualLogSource Logger { get; private set; }
        public static AssetManager Asset { get; private set; }
        public static Harmony Harmony { get; private set; }
        public static BasePlugin Instance { get; private set; }

        private void Awake()
        {
            Asset = new AssetManager();
            Logger = base.Logger;
            Instance = this;
            Harmony = new Harmony(MyPluginInfo.GUID);
            Harmony.PatchAllConditionalFixed();

            AssetLoader.LoadLocalizationFolder(Path.Combine(AssetLoader.GetModPath(this), "Languages"), Language.English);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIStart(), LoadingEventOrder.Start);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIPre(), LoadingEventOrder.Pre);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIPost(), LoadingEventOrder.Post);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIFinal(), LoadingEventOrder.Final);


            GameObject qopObject = new GameObject("Quality Of Plus");
            qopObject.AddComponent<QOPEvents>();

            QOPManager.Instance.RegisterCategory<BetterPauseCategory>(Info, Config);
            QOPManager.Instance.RegisterCategory<BetterGameWindowCategory>(Info, Config);
            QOPManager.Instance.RegisterCategory<BetterNameMenuCategory>(Info, Config);
            QOPManager.Instance.RegisterCategory<GameplayCategory>(Info, Config);


            #region adding assets for name menu because API loads them on name menu, but I need them earlier
            BasePlugin.Asset.Add<Sprite>("NameEntryDarkModeBG", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "NameEntry.png"));
            BasePlugin.Asset.Add<Sprite>("DarkModeEditor", AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1f, "DarkMode", "Editor.png"));

            BasePlugin.Asset.Add<Sprite>("CrossMarkPointed", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "CrossPointed.png"));
            #endregion
        }

        private IEnumerator APIStart()
        {
            IOnAPIStart[] starts = QOPManager.Instance.GetAllFeatures().OfType<IOnAPIStart>().ToArray();
            if (starts.Length == 0)
            {
                yield return 1;
                yield return $"Calling {nameof(APIStart)}";
            }
            else
            {
                yield return starts.Length;

                foreach (IOnAPIStart start in starts)
                {
                    IEnumerator inner = start.APIStartAction();
                    while (inner.MoveNext())
                        yield return inner.Current;
                }
            }
        }
        private IEnumerator APIPre()
        {
            IOnAPIPre[] pres = QOPManager.Instance.GetAllFeatures().OfType<IOnAPIPre>().ToArray();
            if (pres.Length == 0)
            {
                yield return 1;
                yield return $"Calling {nameof(APIPost)}";
            }
            else
            {
                yield return pres.Length;

                foreach (IOnAPIPre start in pres)
                {
                    IEnumerator inner = start.APIPreAction();
                    while (inner.MoveNext())
                        yield return inner.Current;
                }
            }
        }
        private IEnumerator APIPost()
        {
            IOnAPIPost[] posts = QOPManager.Instance.GetAllFeatures().OfType<IOnAPIPost>().ToArray();
            if (posts.Length == 0)
            {
                yield return 1;
                yield return $"Calling {nameof(APIPost)}";
            }
            else
            {
                yield return posts.Length;

                foreach (IOnAPIPost start in posts)
                {
                    IEnumerator inner = start.APIPostAction();
                    while (inner.MoveNext())
                        yield return inner.Current;
                }
            }
        }
        private IEnumerator APIFinal()
        {
            IOnAPIFinal[] finals = QOPManager.Instance.GetAllFeatures().OfType<IOnAPIFinal>().ToArray();
            if (finals.Length == 0)
            {
                yield return 1;
                yield return $"Calling {nameof(APIFinal)}";
            }
            else
            {
                yield return finals.Length;

                foreach (IOnAPIFinal start in finals)
                {
                    IEnumerator inner = start.APIFinalAction();
                    while (inner.MoveNext())
                        yield return inner.Current;
                }
            }
        }
    }
}