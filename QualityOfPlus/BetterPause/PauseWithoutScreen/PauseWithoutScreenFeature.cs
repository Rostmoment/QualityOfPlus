using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using UnityEngine;

namespace QualityOfPlus.BetterPause.PauseWithoutScreen
{
    public class PauseWithoutScreenFeature : QOPToggleableFeature, IUpdatable
    {
        public override string ID => "QOP.FEATURE.PAUSE.WITHOUT.SCREEN";

        private ConfigEntry<KeyCode> keyBind;

        internal bool PauseNoScreen { get; set; }

        protected override string EnabledConfigKey => "Enable Pause Without Screen";
        protected override string EnabledConfigDescription => "Allows pausing the game without opening the pause screen";

        protected override void OnPreInitialize(QOPCategory category)
        {
            keyBind = category.CreateEntry<KeyCode>("Pause Without Screen Key", KeyCode.Backspace, "Key that will be used to pause without pause screen");
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