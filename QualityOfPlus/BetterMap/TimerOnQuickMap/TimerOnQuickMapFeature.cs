using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterMap.TimerOnQuickMap
{
    public class TimerOnQuickMapFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.TIMER.ON.QUICK.MAP";

        protected override string EnabledConfigKey => "Timer On Quick Map";
        protected override string EnabledConfigDescription => "Adds timer before lights out event on quick map";

        public override void PostInitialize(QOPCategory category)
        {

        }

        public override bool IsEnabled()
        {
            return base.IsEnabled() && !Compats.NoTimerInstalled;
        }
    }
}
