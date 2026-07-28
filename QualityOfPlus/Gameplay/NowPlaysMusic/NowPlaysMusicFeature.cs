using QualityOfPlus.Helpers.Extensions;
using QualityOfPlus.Interfaces;
using QualityOfPlus.NotificationSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay.NowPlaysMusic
{
    public class NowPlaysMusicFeature : QOPToggleableFeature, IOnAPIPre
    {
        public override string ID => "QOP.FEATURE.NOW.PLAYS.MUSIC";

        protected override string EnabledConfigKey => "Now Plays Music Notification";
        protected override string EnabledConfigDescription => "Enable notification that shows what music plays now and who composed it";
        protected override bool DefaultValue => false;


        private NotificationManager notification;
        private Dictionary<string, (string name, string author)> music = new Dictionary<string, (string name, string author)>(); 

        public override void PostInitialize(QOPCategory category)
        {
            AddMusic("Elevator", "Thanks For Playing", "Mystman12");
            AddMusic("DanceV0_5", "Dancin' Time", "Mystman12");
            AddMusic("TimeOut_MMP_Corrected", "Time Out", "Mystman12");
            AddMusic("school", "Baldi's Schoolhouse", "Mystman12");
            AddMusic("Tutorial_MMP_Corrected", "Tutorial", "Mystman12");
        }

        /// <summary>
        /// Add "now plays" notification to music
        /// </summary>
        /// <param name="key">Unique key that is used in <see cref="MusicManager.PlayMidi(string, bool)"/></param>
        /// <param name="name">Music name</param>
        /// <param name="author">Music author</param>
        public void AddMusic(string key, string name, string author)
        {
            music.Add(key, (name, author));
        }

        public void ShowMusic(string key)
        {
            if (!music.TryGetValue(key, out (string name, string author) data) || notification.IsNullOrDestroyed())
                return;

            notification.ShowMessageAndHide(new NotificationData("QOP_NOW_PLAYS_TITLE", 
                string.Format(LocalizationManager.Instance.GetLocalizedText("QOP_NOW_PLAYS"), data.name, data.author), 
                NotificationColor.Cyan), 3);
        }

        public IEnumerator APIPreAction()
        {
            yield return "Creating now plays notification...";
            notification = NotificationManager.CreateInstance("QopNowPlays");
        }
    }
}
