using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordBotTest.Models.RsWikiPrices
{
    public class LatestPriceModel
    {
        [JsonProperty("data")]
        public Dictionary<string, PriceData> Data { get; set; }
    }

    public class PriceData
    {
        [JsonProperty("high")]
        public int? High { get; set; }

        [JsonProperty("highTime")]
        public int? HighTime { get; set; }

        [JsonProperty("low")]
        public int? Low { get; set; }

        [JsonProperty("lowTime")]
        public int? LowTime { get; set; }
    }
}

