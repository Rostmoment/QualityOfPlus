using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.Gameplay.GameplayHistory
{
    public class GameplayHistoryEntry
    {
        public int seed;

        [JsonConverter(typeof(IsoDateTimeConverter), "dd-MM-yyyy")]
        public DateTime date;

        public string name;
        public string level;

        [JsonConstructor]
        public GameplayHistoryEntry(int seed, DateTime date, string name, string level)
        {
            this.seed = seed;
            this.date = date;
            this.name = name;
            this.level = level;
        }

        public static GameplayHistoryEntry CreateNow(int seed, SceneObject scene) => CreateNow(seed, scene.levelTitle);
        public static GameplayHistoryEntry CreateNow(int seed, string level) => new GameplayHistoryEntry(seed, DateTime.Now, PlayerFileManager.Instance.fileName, level);
    }

    internal static class GameplayHistoryStorage
    {
        private static string FilePath => Path.Combine(Application.persistentDataPath, "QOPGameplayHistory.json");
        private static List<GameplayHistoryEntry> entries;

        public static GameplayHistoryEntry[] Entries
        {
            get
            {
                if (entries == null)
                    LoadHistory();
                
                return entries.ToArray();
            }
        }

        public static void Clear()
        {
            entries = new List<GameplayHistoryEntry>();
            SaveHistory();
        }

        public static void AddEntry(GameplayHistoryEntry entry)
        {
            if (entries == null)
                LoadHistory();
            
            entries.Add(entry);
            SaveHistory();
        }

        public static void SaveHistory()
        {
            List<GameplayHistoryEntry> dataToSave = entries ?? new List<GameplayHistoryEntry>();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(dataToSave, settings);

            string directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(FilePath, json, Encoding.UTF8);
        }

        public static void LoadHistory()
        {
            if (!File.Exists(FilePath))
            {
                entries = new List<GameplayHistoryEntry>();
                return;
            }

            string json = File.ReadAllText(FilePath, Encoding.UTF8);

            List<GameplayHistoryEntry> loaded = JsonConvert.DeserializeObject<List<GameplayHistoryEntry>>(json);
            entries = loaded ?? new List<GameplayHistoryEntry>();
        }
    }
}