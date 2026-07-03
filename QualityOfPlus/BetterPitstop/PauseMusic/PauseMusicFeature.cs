using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.BetterPitstop.PauseMusic
{
    public class PauseMusicFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.PAUSE.MUSIC";

        protected override string EnabledConfigKey => "Pause Music During Loading Screen";
        protected override string EnabledConfigDescription => "Pauses the music during the loading screen when entering or exiting a field trip";

        public override void PostInitialize(QOPCategory category)
        {

        }

    }
}
