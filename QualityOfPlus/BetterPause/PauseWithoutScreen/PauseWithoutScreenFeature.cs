using BepInEx.Configuration;
using QualityOfPlus.Extensions;
using QualityOfPlus.Interfaces;
using UnityEngine;

namespace QualityOfPlus.BetterPause.PauseWithoutScreen
{
    public class PauseWithoutScreenFeature : QOPFeature, IToggleableFeature, IUpdatable
    {
        public ConfigEntry<bool> Enabled { get; private set; }
        public bool ValueIfNull => false;


        public override string ID => "QOP.FEATURE.PAUSE.WITHOUT.SCREEN";

        private ConfigEntry<KeyCode> keyBind;

        internal bool PauseNoScreen { get; set; }

        public override void PreInitialize(QOPCategory category)
        {
            keyBind = category.CreateEntry<KeyCode>("Pause Without Screen Key", KeyCode.Backspace, "Key that will be used to pause without pause screen");
            Enabled = category.CreateEntry<bool>("Enable Pause Without Screen", true, "Allows pausing the game without opening the pause screen");
        }

        public override void PostInitialize(QOPCategory category)
        {
        }

        public void OnUpdate()
        {
            if (CoreGameManager.Instance.IsNullOrDestroyed())
                return;

            if (!Input.GetKeyDown(keyBind.Value) || !this.IsEnabled())
                return;

            if (CoreGameManager.Instance.disablePause || GlobalCam.Instance.TransitionActive)
                return;

            if (CoreGameManager.Instance.Paused)
            {
                CoreGameManager.Instance.Pause(false);
                return;
            }

            PauseNoScreen = true;
            CoreGameManager.Instance.Pause(false);

            if (CoreGameManager.Instance.Paused)
                CoreGameManager.Instance.GetHud(0).SetTooltip(LocalizationManager.Instance.GetLocalizedText("QOP_PAUSED_WITHOUT_SCREEN"));
        }
    }
}