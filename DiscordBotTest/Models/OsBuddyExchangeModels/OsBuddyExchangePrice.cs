﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DiscordBotTest.OsBuddyExchangeModels.Price
{
    public partial class OsBuddyExchangePrice
    {
        [JsonProperty("overall")]
        [DisplayName("Overall Price")]
        public long Overall { get; set; }

        [JsonProperty("buying")]
        [DisplayName("Buying Price")]
        public long Buying { get; set; }

        [JsonProperty("selling")]
        [DisplayName("Selling Price")]
        public long Selling { get; set; }

        [JsonProperty("buyingQuantity")]
        [DisplayName("Buying Quantity")]
        public long BuyingQuantity { get; set; }

        [JsonProperty("sellingQuantity")]
        [DisplayName("Selling Quantity")]
        public long SellingQuantity { get; set; }
    }

    public partial class OsBuddyExchangePrice
    {
        public static OsBuddyExchangePrice FromJson(string json) => JsonConvert.DeserializeObject<OsBuddyExchangePrice>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this OsBuddyExchangePrice self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
