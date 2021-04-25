using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DiscordBotTest.OverwatchModels
{
    public partial class OverwatchStats
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("levelIcon")]
        public string LevelIcon { get; set; }

        [JsonProperty("prestige")]
        public long Prestige { get; set; }

        [JsonProperty("prestigeIcon")]
        public string PrestigeIcon { get; set; }

        [JsonProperty("rating")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Rating { get; set; }

        [JsonProperty("ratingName")]
        public string RatingName { get; set; }

        [JsonProperty("ratingIcon")]
        public string RatingIcon { get; set; }

        [JsonProperty("gamesWon")]
        public long GamesWon { get; set; }

        [JsonProperty("quickPlayStats")]
        public Stats QuickPlayStats { get; set; }

        [JsonProperty("competitiveStats")]
        public Stats CompetitiveStats { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("eliminationsAvg")]
        public double EliminationsAvg { get; set; }

        [JsonProperty("damageDoneAvg")]
        public long DamageDoneAvg { get; set; }

        [JsonProperty("deathsAvg")]
        public double DeathsAvg { get; set; }

        [JsonProperty("finalBlowsAvg")]
        public double FinalBlowsAvg { get; set; }

        [JsonProperty("healingDoneAvg")]
        public long HealingDoneAvg { get; set; }

        [JsonProperty("objectiveKillsAvg")]
        public double ObjectiveKillsAvg { get; set; }

        [JsonProperty("objectiveTimeAvg")]
        public string ObjectiveTimeAvg { get; set; }

        [JsonProperty("soloKillsAvg")]
        public double SoloKillsAvg { get; set; }

        [JsonProperty("games")]
        public Games Games { get; set; }

        [JsonProperty("awards")]
        public Awards Awards { get; set; }
    }

    public partial class Awards
    {
        [JsonProperty("cards")]
        public long Cards { get; set; }

        [JsonProperty("medals")]
        public long Medals { get; set; }

        [JsonProperty("medalsBronze")]
        public long MedalsBronze { get; set; }

        [JsonProperty("medalsSilver")]
        public long MedalsSilver { get; set; }

        [JsonProperty("medalsGold")]
        public long MedalsGold { get; set; }
    }

    public partial class Games
    {
        [JsonProperty("played")]
        public long Played { get; set; }

        [JsonProperty("won")]
        public long Won { get; set; }
    }

    public partial class OverwatchStats
    {
        public static OverwatchStats FromJson(string json) => JsonConvert.DeserializeObject<OverwatchStats>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this OverwatchStats self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            return long.TryParse(value, out long l) ? l : 0;
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
