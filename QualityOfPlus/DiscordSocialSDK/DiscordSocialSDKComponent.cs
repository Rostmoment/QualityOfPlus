using BepInEx.Configuration;
using BepInEx.DiscordSocialSDK.Client;
using BepInEx.DiscordSocialSDK.Handles;
using BepInEx.DiscordSocialSDK.RPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK
{
    class DiscordSocialSDKComponent : BaseQOLThing
    {
        protected override string CategoryName => "Discord Social SDK";

        private static ConfigEntry<bool> autoDND;
        private static ConfigEntry<bool> enableDiscordRPC;
        private static ConfigEntry<string> buttonOneText;
        private static ConfigEntry<string> buttonTwoText;
        private static ConfigEntry<string> buttonOneUrl;
        private static ConfigEntry<string> buttonTwoUrl;


        public static bool EnableDiscordRPC => enableDiscordRPC != null && enableDiscordRPC.Value;
        public static bool AutoDND => autoDND != null && autoDND.Value;
        public static string ButtonOneText => buttonOneText.Value;
        public static string ButtonOneUrl => buttonOneUrl.Value;
        public static string ButtonTwoText => buttonTwoText.Value;
        public static string ButtonTwoUrl => buttonTwoUrl.Value;

        public static ClientWrapper client;
        public static DiscordRPCWrapper rpc;

        public override void Initialize()
        {

            autoDND = CreateConfig("Auto DND", false, "Automatically set your Discord status to Do Not Disturb when you are in-game");
            enableDiscordRPC = CreateConfig("Enable Discord RPC", true, "Enable Discord Rich Presence integration");

            buttonOneText = CreateConfig("Button One Text", "", "Text on first button in your DiscordRPC activity");
            buttonOneUrl = CreateConfig("Button One URL", "", "URL that opens when user clicks first button");
            buttonTwoText = CreateConfig("Button Two Text", "", "Text on second button in your DiscordRPC activity");
            buttonTwoUrl = CreateConfig("Button Two URL", "", "URL that opens when user clicks second button");

            client = new ClientWrapper(1487165611397222540);
            rpc = new DiscordRPCWrapper(client);
            
        }

    }
}
