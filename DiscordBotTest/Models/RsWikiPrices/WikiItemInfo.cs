using Newtonsoft.Json;

namespace DiscordBotTest.Models.RsWikiPrices
{
    public class WikiItemInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("examine")]
        public string Examine { get; set; }

        [JsonProperty("members")]
        public bool Members { get; set; }

        [JsonProperty("lowalch")]
        public int? Lowalch { get; set; }

        [JsonProperty("highalch")]
        public int? Highalch { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
