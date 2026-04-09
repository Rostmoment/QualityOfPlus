using BepInEx.Configuration;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus
{
    public static class Extensions
    {
        public static bool Exists<T>(this AssetManager asset, string key)
        {
            if (!asset.data.ContainsKey(typeof(T)))
                return false;

            return asset.data[typeof(T)].ContainsKey(key);
        }
        public static int GetElevatorsCount(this EnvironmentController ec) => ec.ElevatorManager.Elevators.Count;
        public static int GetTotalOutOfOrderElevators(this EnvironmentController ec) => ec.ElevatorManager.TotalOutOfOrderElevators;
        public static int GetOutOfElevatorsCount(this EnvironmentController ec) => ec.ElevatorManager.Elevators.Count(x => x.CurrentState == ElevatorState.OutOfOrder);
        public static T ValueOrDefault<T>(this ConfigEntry<T> config, T defaultValue)
        {
            if (config == null)
                return defaultValue;
            
            return config.Value;
        }
        public static bool TexturesAreEqual(this Texture2D tex1, Texture2D tex2)
        {
            if (tex1 == null)
                return tex2 == null;
            if (tex2 == null)
                return false;

            if (tex1.width != tex2.width || tex1.height != tex2.height)
                return false;

            Color[] pixels1 = tex1.GetPixels();
            Color[] pixels2 = tex2.GetPixels();
            for (int i = 0; i < pixels1.Length; i++)
            {
                if (pixels1[i] != pixels2[i])
                    return false;
            }
            return true;
        }
        public static bool IsNullOrDestroyed(this object obj)
        {
            try
            {
                if (obj == null) { return true; }
                else if (obj is UnityEngine.Object unityObj && !unityObj) { return true; }
                return false;
            }
            catch { return true; }
        }
        public static bool EmptyOrNull<T>(this ICollection<T> values)
        {
            if (values == null)
                return true;
            if (values.Count == 0)
                return true;
            return false;
        }
        public static T Find<T>(this T[] array, Func<T, bool> predicate) => array.Where(predicate).FirstOrDefault();

        public static bool CheckForHotKey(this KeyCode keyCode) =>
          (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) && Input.GetKeyDown(keyCode);

        public static void EndGame(this CoreGameManager instance, Transform player, Transform targerPosition) =>
           instance.EndGame(player, targerPosition, ((Baldi)NPCMetaStorage.Instance.Get(Character.Baldi).value).loseSounds);
        public static void EndGame(this CoreGameManager instance, Transform player, Transform targetPosition, WeightedSoundObject[] loseSounds)
        {
            Time.timeScale = 0f;
            MusicManager.Instance.StopMidi();
            instance.disablePause = true;
            instance.GetCamera(0).UpdateTargets(targetPosition, 0);
            instance.GetCamera(0).offestPos = (player.position - targetPosition.position).normalized * 2f + Vector3.up;
            instance.GetCamera(0).SetControllable(value: false);
            instance.GetCamera(0).matchTargetRotation = false;
            instance.audMan.volumeModifier = 0.6f;
            if (!loseSounds.EmptyOrNull()) 
                instance.audMan.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(loseSounds));
            instance.StartCoroutine(instance.EndSequence());
            InputManager.Instance.Rumble(1f, 2f);

        }
    }
}
