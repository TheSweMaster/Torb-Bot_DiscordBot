using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DiscordBotTest.Models.Trackmania
{
    public partial class TrackManiaMapLeaderboardModel
    {
        [JsonProperty("tops")]
        public List<Top> Tops { get; set; }

        [JsonProperty("playercount")]
        public long Playercount { get; set; }
    }

    public partial class Top
    {
        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class Player
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("zone")]
        public Zone Zone { get; set; }

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public Meta Meta { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("vanity", NullValueHandling = NullValueHandling.Ignore)]
        public string Vanity { get; set; }

        [JsonProperty("tmgl", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tmgl { get; set; }

        [JsonProperty("twitch", NullValueHandling = NullValueHandling.Ignore)]
        public string Twitch { get; set; }

        [JsonProperty("youtube", NullValueHandling = NullValueHandling.Ignore)]
        public string Youtube { get; set; }

        [JsonProperty("twitter", NullValueHandling = NullValueHandling.Ignore)]
        public string Twitter { get; set; }
    }

    public partial class Zone
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("parent")]
        public ZoneParent Parent { get; set; }
    }

    public partial class ZoneParent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("parent")]
        public PurpleParent Parent { get; set; }
    }

    public partial class PurpleParent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyParent Parent { get; set; }
    }

    public partial class FluffyParent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public TentacledParent Parent { get; set; }
    }

    public partial class TentacledParent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }
    }

    public partial class TrackManiaMapLeaderboardModel
    {
        public static TrackManiaMapLeaderboardModel FromJson(string json) => JsonConvert.DeserializeObject<TrackManiaMapLeaderboardModel>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TrackManiaMapLeaderboardModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
