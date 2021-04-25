using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotTest.OsBuddyExchangeModels
{
    public partial class OsBuddyExchangeData
    {
        [JsonProperty("members")]
        public bool Members { get; set; }

        [JsonProperty("buy_average")]
        public long BuyAverage { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overall_average")]
        public long OverallAverage { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("sell_average")]
        public long SellAverage { get; set; }

        [JsonProperty("sp")]
        public long Sp { get; set; }
    }

    public partial class OsBuddyExchangeData
    {
        public static Dictionary<string, OsBuddyExchangeData> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, OsBuddyExchangeData>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, OsBuddyExchangeData> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }

}
