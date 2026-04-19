using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.DiscordSocialSDK;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using QualityOfPlus.BetterHUD;
using QualityOfPlus.BetterMap;
using QualityOfPlus.BetterNameMenu;
using QualityOfPlus.BetterPause;
using QualityOfPlus.DarkMode;
using QualityOfPlus.GameWindow;
using QualityOfPlus.TABSwitch;
using System.Collections;
using UnityEngine;
using MTM101BaldAPI;
using QualityOfPlus.BetterElevator;
using QualityOfPlus.BetterPitstop;
using MTM101BaldAPI.OptionsAPI;
using QualityOfPlus.ConfigInOptions;
using System.IO;

namespace QualityOfPlus
{
    public class MyPluginInfo
    {
        public const string NAME = "Quality Of Plus";
        public const string GUID = "rost.moment.baldiplus.qop";
        public const string VERSION = "1.9";
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
        private static GameObject qolObject;

        private void Awake()
        {
            Harmony = new Harmony(MyPluginInfo.GUID);
            Harmony.PatchAllConditionals();
            Asset = new AssetManager();
            Logger = base.Logger;
            Instance = this;

            AssetLoader.LoadLocalizationFolder(Path.Combine(AssetLoader.GetModPath(this), "Languages"), Language.English);
            LoadingEvents.RegisterOnAssetsLoaded(Info, LoadAssets(), LoadingEventOrder.Pre);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIStart(), LoadingEventOrder.Start);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIPre(), LoadingEventOrder.Pre);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIPost(), LoadingEventOrder.Post);
            LoadingEvents.RegisterOnAssetsLoaded(Info, APIFinal(), LoadingEventOrder.Final);

            qolObject = new GameObject("QualityOfPlus");
            qolObject.AddComponent<TABSwitcherComponent>().Initialize();
            qolObject.AddComponent<BetterMapComponent>().Initialize();
            qolObject.AddComponent<BetterElevatorComponent>().Initialize();
            qolObject.AddComponent<BetterPauseComponent>().Initialize();
            qolObject.AddComponent<BetterNameMenuComponent>().Initialize();
            qolObject.AddComponent<BetterHUDComponent>().Initialize();
            qolObject.AddComponent<BetterPitstopComponent>().Initialize();
            qolObject.AddComponent<BetterGameWindowComponent>().Initialize();
            qolObject.AddComponent<DarkModeComponent>().Initialize();

            CustomOptionsCore.OnMenuInitialize += ConfigOptionsMenu.Register;


            if (Compats.DiscordSDKInstalled)
                qolObject.AddComponent<DiscordSocialSDK.DiscordSocialSDKComponent>().Initialize();
            else
                MTM101BaldiDevAPI.AddWarningScreen("<color=red>Discord Social SDK for BepInEx is not installed!</color>\nDiscord features of Quality Of Plus will be disabled.\nPlease install Discord Social SDK to enable them.", false);


            DontDestroyOnLoad(qolObject);

            #region adding assets for name menu because API loads them on name menu, but I need them earlier

            BasePlugin.Asset.Add<Sprite>("NameEntryDarkModeBG", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "NameEntry.png"));
            BasePlugin.Asset.Add<Sprite>("DarkModeEditor", AssetLoader.SpriteFromMod(BasePlugin.Instance, Vector2.one / 2f, 1f, "DarkMode", "Editor.png"));

            BasePlugin.Asset.Add<Sprite>("CrossMarkPointed", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "CrossPointed.png"));
            #endregion
        }

        private IEnumerator APIStart()
        {
            BaseQOLThing[] components = qolObject.GetComponents<BaseQOLThing>();
            yield return components.Length;

            foreach (BaseQOLThing component in components)
            {
                IEnumerator inner = component.OnAPIStart();
                while (inner.MoveNext())
                    yield return inner.Current;
            }
        }
        private IEnumerator APIPre()
        {
            BaseQOLThing[] components = qolObject.GetComponents<BaseQOLThing>();
            yield return components.Length;

            foreach (BaseQOLThing component in components)
            {
                IEnumerator inner = component.OnAPIPre();
                while (inner.MoveNext())
                    yield return inner.Current;
            }
        }
        private IEnumerator APIPost()
        {
            BaseQOLThing[] components = qolObject.GetComponents<BaseQOLThing>();
            yield return components.Length;

            foreach (BaseQOLThing component in components)
            {
                IEnumerator inner = component.OnAPIPost();
                while (inner.MoveNext())
                    yield return inner.Current;
            }
        }
        private IEnumerator APIFinal()
        {
            BaseQOLThing[] components = qolObject.GetComponents<BaseQOLThing>();
            yield return components.Length;

            foreach (BaseQOLThing component in components)
            {
                IEnumerator inner = component.OnAPIFinal();
                while (inner.MoveNext())
                    yield return inner.Current;
                
            }
        }

        private IEnumerator LoadAssets()
        {
            AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
            Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();

            yield return 1;

            yield return "Loading textures...";

            if (!BasePlugin.Asset.Exists<Sprite>("CrossMark"))
                BasePlugin.Asset.Add<Sprite>("CrossMark", Resources.FindObjectsOfTypeAll<Sprite>().Find(x => x.name == "YCTP_IndicatorsSheet_1"));


            BasePlugin.Asset.Add<Texture2D>("AltChalkboardPit", AssetLoader.TextureFromMod(this, "AltChalkboardPit.png"));
            BasePlugin.Asset.Add<Texture2D>("ElevatorsCounterIconSheet", AssetLoader.TextureFromMod(this, "ElevatorIconSheet.png"));
            BasePlugin.Asset.Add<Texture2D>("NotebooksCounterIconSheet", textures.Find(x => x.name == "NotebookIcon_Sheet").MakeReadableCopy(true));

            BasePlugin.Asset.Add<Sprite>("MainMenuDarkMode", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "MainMenu.png"));
            BasePlugin.Asset.Add<Sprite>("ExitNotHighlitghedDarkMode", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "ExitNotHighlitghed.png"));
            BasePlugin.Asset.Add<Sprite>("ExitHighlitghedDarkMode", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "ExitHighlitghed.png"));

            BasePlugin.Asset.Add<Sprite>("OptionsMenuDarkMode", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "OptionsMenu.png"));
            BasePlugin.Asset.Add<Sprite>("WhiteCheckbox", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "WhiteCheckBox.png"));

            BasePlugin.Asset.Add<Sprite>("ChallengeWinDarkMode", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 1, "DarkMode", "ChallengeWin.png"));

            BasePlugin.Asset.Add<Sprite>("YTPMapIcon", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 70, "MapIcons", "YTPIcon.png"));
            BasePlugin.Asset.Add<Sprite>("TapePlayerIcon", AssetLoader.SpriteFromMod(this, Vector2.one / 2f, 22 , "MapIcons", "TapePlayerIcon.png"));

            BasePlugin.Asset.Add<Sprite>("White", AssetLoader.SpriteFromTexture2D(Texture2D.whiteTexture, 1));

            BasePlugin.Asset.Add<Sprite>("ArrowLeftHigh", sprites.Find(x => x.name == "MenuArrowSheet_0"));
            BasePlugin.Asset.Add<Sprite>("ArrowLeftUnhigh", sprites.Find(x => x.name == "MenuArrowSheet_2"));
            BasePlugin.Asset.Add<Sprite>("ArrowRightHigh", sprites.Find(x => x.name == "MenuArrowSheet_1"));
            BasePlugin.Asset.Add<Sprite>("ArrowRightUnhigh", sprites.Find(x => x.name == "MenuArrowSheet_3"));

        }


    }
}