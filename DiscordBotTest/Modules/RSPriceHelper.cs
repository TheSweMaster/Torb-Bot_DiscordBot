using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTest.RsExchangeModels;
using DiscordBotTest.RsExchangeModels.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class RSPriceHelper : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _client = new HttpClient();
        [Command("rsprice")]
        public async Task GetCurrentRSPrice([Remainder]string itemName = "Rune Platebody")
        {
            var exhangeDataList = await GetExchangeDataList();

            var itemId = GetItemIdByName(exhangeDataList, itemName);

            var priceData = await GetPriceDetails(itemId);

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Runescape Price Data")
                .AddField($"Current price for item '{itemName}'", $"{priceData.Overall}gp")
                .WithColor(Color.DarkOrange)
                .WithCurrentTimestamp();

            await ReplyAsync("", false, builder.Build());
        }

        private async Task<Dictionary<string, RsExchangeData>> GetExchangeDataList()
        {
            var url = "https://rsbuddy.com/exchange/summary.json";

            var responds = await _client.GetStringAsync(url);

            var exhangeDataList = RsExchangeData.FromJson(responds);
            return exhangeDataList;
        }

        private string GetItemIdByName(Dictionary<string, RsExchangeData> exhangeDataList, string itemName)
        {
            var result = exhangeDataList.FirstOrDefault(x => x.Value.Name.ToLower() == itemName.ToLower()).Key;
            return result;
        }

        public class PriceResult
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }

        private async Task<RSExchangePrice> GetPriceDetails(string id)
        {
            string url = "https://api.rsbuddy.com/grandExchange?a=guidePrice&i=" + id;

            var responds = await _client.GetStringAsync(url);

            var result = RSExchangePrice.FromJson(responds);

            return result;
        }

    }
}
