using BepInEx.Configuration;
using MTM101BaldAPI.Registers;
using QualityOfPlus.Interfaces;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace QualityOfPlus.BetterPause.RestartButton
{
    public class RestartButtonFeature : QOPFeature, IToggleableFeature, IOnAPIStart
    {
        private Dictionary<Type, Action<BaseGameManager>> actions = new Dictionary<Type, Action<BaseGameManager>>();
        private WeightedSoundObject[] loseSounds;

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }


        public override string ID => "QOP.FEATURE.RESTART.BUTTON";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Enable Restart Button", true, "If true, restart button will appear in pause menu");
        }
        public override void PostInitialize(QOPCategory category)
        {
            AddCustomAction<TutorialGameManager>(x => CoreGameManager.Instance.Quit());
            AddCustomAction<PitstopGameManager>(x => x.LoadNextLevel());
        }
        public IEnumerator APIStartAction()
        {
            yield return "Copying lose sounds...";
            loseSounds = ((Baldi)NPCMetaStorage.Instance.Get(Character.Baldi).value).loseSounds;
        }

        private void DefaultAction(BaseGameManager gameManager)
        {
            GameCamera camera = CoreGameManager.Instance.GetCamera(0);

            Time.timeScale = 0f;
            MusicManager.Instance.StopMidi();
            CoreGameManager.Instance.disablePause = true;
            camera.UpdateTargets(CoreGameManager.Instance.GetPlayer(0).transform, 0);
            camera.offestPos = Vector3.up;
            camera.SetControllable(value: false);
            camera.matchTargetRotation = false;
            CoreGameManager.Instance.audMan.volumeModifier = 0.6f;
            CoreGameManager.Instance.audMan.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(loseSounds));
            CoreGameManager.Instance.StartCoroutine(CoreGameManager.Instance.EndSequence());
            InputManager.Instance.Rumble(1f, 2f);
        }

        public void AddCustomAction<T>(Action<T> action) where T : BaseGameManager
        {
            actions[typeof(T)] = (gameManager) =>
            {
                if (gameManager is T typedManager)
                    action(typedManager);
            };
        }
        public Action<BaseGameManager> GetAction(Type type)
        {
            if (actions.TryGetValue(type, out var action))
                return action;
            return DefaultAction;
        }
    }
}
