using BepInEx.DiscordSocialSDK.RPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK.RPC
{
    public class DiscordRPCFeature : QOPToggleableFeature
    {
        internal DiscordRPCWrapper RPC { get; private set; }
        public override string ID => "QOP.FEATURE.DISCORD.RPC";

        protected override string EnabledConfigKey => "Discord RPC";
        protected override string EnabledConfigDescription => "Enables Discord Rich Presence support";

        public override void PostInitialize(QOPCategory category)
        {
            RPC = new DiscordRPCWrapper(DiscordSocialSDKCategory.Client);
            RPC.StartTimerNow();
        }

        public void Clear() => RPC.Wipe();
    }
}
