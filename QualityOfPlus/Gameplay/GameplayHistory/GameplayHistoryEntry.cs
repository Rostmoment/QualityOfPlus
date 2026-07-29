using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Gameplay.GameplayHistory
{
    public class GameplayHistoryEntry
    {
        public int seed;
        [JsonConverter(typeof(IsoDateTimeConverter), "dd-MM-yyyy")]
        public DateTime date;
        public string name;

        public GameplayHistoryEntry(int seed, DateTime date, string name)
        {
            this.seed = seed;
            this.date = date;
            this.name = name;
        }

        public static GameplayHistoryEntry CreateNow(int seed) => new GameplayHistoryEntry(seed, DateTime.Now, PlayerFileManager.Instance.fileName);
    }
}
