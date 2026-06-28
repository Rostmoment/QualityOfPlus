using BepInEx;
using BepInEx.DiscordSocialSDK;
using BepInEx.Logging;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.Registers;
using QualityOfPlus.BetterElevator;
using QualityOfPlus.BetterGameWindow;
using QualityOfPlus.BetterHUD;
using QualityOfPlus.BetterMenu;
using QualityOfPlus.BetterNameMenu;
using QualityOfPlus.BetterPause;
using QualityOfPlus.BetterSeed;
using QualityOfPlus.BetterUI;
using QualityOfPlus.ConditionalPatches;
using QualityOfPlus.Gameplay;
using QualityOfPlus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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

            RegisterCategory<BetterPauseCategory>();
            RegisterCategory<BetterGameWindowCategory>();
            RegisterCategory<BetterNameMenuCategory>();
            RegisterCategory<GameplayCategory>();
            RegisterCategory<BetterUICategory>();
            RegisterCategory<BetterElevatorCategory>();
            RegisterCategory<BetterHUDCategory>();
            RegisterCategory<BetterMenuCategory>();
            RegisterCategory<BetterSeedCategory>();

        }

        private void RegisterCategory<T>() where T : QOPCategory, new() => 
            QOPManager.Instance.RegisterCategory<T>(Info, Config);

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