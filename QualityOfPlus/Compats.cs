using BepInEx.Bootstrap;
using BepInEx.DiscordSocialSDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus
{
    class Compats
    {
        public const string LEVEL_STUDIO_GUID = "mtm101.rulerp.baldiplus.levelstudio";
        public static bool LevelStudioInstalled => Chainloader.PluginInfos.ContainsKey(LEVEL_STUDIO_GUID);

        public static bool DiscordSDKInstalled => Chainloader.PluginInfos.ContainsKey(DiscordSocialSDKPlugin.GUID);
    }
}
