using Discord;
using Discord.Commands;
using DiscordBotTest.Models.RsWikiPrices;
using HtmlAgilityPack;
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

            string imageUrl = await GetRsBaseImageUrl();
            var imageIconUrl = new Uri($"{imageUrl}{foundItem.Id}");
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

        private async Task<string> GetRsBaseImageUrl()
        {
            const string itemUrl = "https://secure.runescape.com/m=itemdb_oldschool/Old+school+bond/viewitem?obj=13190";
            var web = new HtmlWeb();
            var htmlDocument = await web.LoadFromWebAsync(itemUrl);
            const string xpathImageUrl = "//*[@id='grandexchange']/div/div/main/div[2]/div[1]/img";
            var imageSourceUrl = htmlDocument.DocumentNode.SelectSingleNode(xpathImageUrl)?.GetAttributeValue("src", string.Empty) ?? string.Empty;
            // Example: src="https://secure.runescape.com/m=itemdb_oldschool/1619431325318_obj_big.gif?id=13190"
            var rsBaseImageUrl = imageSourceUrl.TrimEnd("13190".ToArray());

            return rsBaseImageUrl;
        }

        private string GetDisplayPrice(int? price)
        {
            return !price.HasValue ? "-" : $"{price.Value:#,##0}gp";
        }
    }
}
