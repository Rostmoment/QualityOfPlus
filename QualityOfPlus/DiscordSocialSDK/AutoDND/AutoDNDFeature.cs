using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK.AutoDND
{
    public class AutoDNDFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.DISCORD.AUTODND";

        protected override string EnabledConfigKey => "Auto DND";
        protected override string EnabledConfigDescription => "Automatically set your Discord status to Do Not Disturb when you are playing";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }
    }
}
