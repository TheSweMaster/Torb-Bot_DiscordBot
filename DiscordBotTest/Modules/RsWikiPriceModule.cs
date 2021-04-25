using Discord;
using Discord.Commands;
using DiscordBotTest.Models.RsWikiPrices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class RsWikiPriceModule : ModuleBase<SocketCommandContext>
    {
        private const string RsWikiPriceApiBaseUrl = "https://prices.runescape.wiki/api/v1/osrs";
        private readonly HttpClient _httpClient;

        public RsWikiPriceModule(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Command("rsprice")]
        public async Task CurrentRSPriceCommand([Remainder] string itemName = "Old School Bond")
        {
            var success = _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("Torbot_Discord_Bot - @TheSweMaster#8595");
            var jsonWikiItemResult = await _httpClient.GetStringAsync($"{RsWikiPriceApiBaseUrl}/mapping");
            var jsonPriceResult = await _httpClient.GetStringAsync($"{RsWikiPriceApiBaseUrl}/latest");

            if (string.IsNullOrEmpty(jsonWikiItemResult) || string.IsNullOrEmpty(jsonPriceResult))
            {
                await ReplyAsync($"Can't retrieve any data from '{RsWikiPriceApiBaseUrl}'.");
                return;
            }

            var wikiItems = JsonConvert.DeserializeObject<List<WikiItemInfo>>(jsonWikiItemResult);
            var latestPriceData = JsonConvert.DeserializeObject<LatestPriceModel>(jsonPriceResult);

            var foundItem = wikiItems.FirstOrDefault(item => item.Name.ToLowerInvariant() == itemName.ToLowerInvariant());
            if (foundItem == null)
            {
                await ReplyAsync($"Can't find any item with that name.");
                return;
            }
            var priceData = latestPriceData.Data.TryGetValue(foundItem.Id.ToString(), out PriceData value) ? value : new PriceData();

            var imageIconUrl = new Uri($"https://secure.runescape.com/m=itemdb_oldschool/a=142/1619001006055_obj_big.gif?id={foundItem.Id}");
            var builder = new EmbedBuilder()
                    .WithTitle($"Runescape Current Price Data")
                    .AddField($"{foundItem.Name}",
                        $"Buy price: {GetDisplayPrice(priceData.High)}\nSell price: {GetDisplayPrice(priceData.Low)}")
                    .WithColor(Color.DarkOrange)
                    .WithImageUrl(imageIconUrl.AbsoluteUri)
                    .WithFooter("Data from: https://prices.runescape.wiki/osrs")
                    .WithCurrentTimestamp();

            await ReplyAsync(embed: builder.Build());
        }

        private string GetDisplayPrice(int? price)
        {
            return !price.HasValue ? "-" : $"{price.Value:#,##0}gp";
        }
    }
}
