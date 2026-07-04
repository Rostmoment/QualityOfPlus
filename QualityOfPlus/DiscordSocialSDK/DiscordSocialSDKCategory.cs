using BepInEx.DiscordSocialSDK.Client;
using QualityOfPlus.DiscordSocialSDK.AutoDND;
using QualityOfPlus.DiscordSocialSDK.RPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK
{
    public class DiscordSocialSDKCategory : QOPCategory
    {
        public override string ID => "QOP.CATEGORY.DISCORD.SOCIAL.SDK";
        public override string Name => "Discord Social SDK";


        public override void PreInitialize()
        {
            Client = new ClientWrapper(1487165611397222540);

            AddFeature<DiscordRPCFeature>();
            AddFeature<AutoDNDFeature>();
        }
        public override void PostInitialize()
        {
        }

        internal static ClientWrapper Client { get; private set; }
    }
}
