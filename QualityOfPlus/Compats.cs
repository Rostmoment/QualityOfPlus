using BepInEx.Bootstrap;
using BepInEx.DiscordSocialSDK;
using HarmonyLib;
using MTM101BaldAPI;
using QualityOfPlus.ConditionalPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QualityOfPlus
{
    internal static class Compats
    {
        public const string LEVEL_STUDIO_GUID = "mtm101.rulerp.baldiplus.levelstudio";
        public static bool LevelStudioInstalled => Chainloader.PluginInfos.ContainsKey(LEVEL_STUDIO_GUID);

        public static bool DiscordSDKInstalled => Chainloader.PluginInfos.ContainsKey(DiscordSocialSDKPlugin.GUID);
        public const string NO_TIMER_GUID = "rost.moment.baldiplus.notimer";

        public static bool BetterStickerUIInstalled => Chainloader.PluginInfos.ContainsKey(BETTER_STICKER_UI_GUID);
        public const string BETTER_STICKER_UI_GUID = "nekitconstructions.betterstickers";

    }
}
