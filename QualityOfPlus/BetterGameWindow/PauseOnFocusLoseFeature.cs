using PlusLevelStudio.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterGameWindow
{
    public class PauseOnFocusLoseFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.PAUSE.ON.FOCUS.LOSE";

        protected override string EnabledConfigKey => "Pause On Focus Lose";
        protected override string EnabledConfigDescription => "Pauses the game when the game window loses focus";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
            GameObject.DontDestroyOnLoad(new GameObject("GameWatches").AddComponent<GameWindowWatcher>());
        }

    }

    class GameWindowWatcher : MonoBehaviour
    {
        private void PauseWithLevelStudio()
        {
            if (EditorController.Instance == null)
                CoreGameManager.Instance.Pause(true);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (CoreGameManager.Instance == null || CoreGameManager.Instance.disablePause || GlobalCam.Instance.TransitionActive || 
                CoreGameManager.Instance.Paused || !QOPManager.Instance.GetFeatureIfEnabled<PauseOnFocusLoseFeature>(out _) || hasFocus)
                return;

            if (Compats.LevelStudioInstalled)
                PauseWithLevelStudio();
            else
                CoreGameManager.Instance.Pause(true);
        }
    }
}
