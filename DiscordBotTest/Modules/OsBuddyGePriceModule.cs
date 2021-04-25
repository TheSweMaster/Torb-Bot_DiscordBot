using Discord;
using Discord.Commands;
using DiscordBotTest.OsBuddyExchangeModels;
using DiscordBotTest.OsBuddyExchangeModels.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class OsBuddyGePriceModule : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _client = new HttpClient();
        private const string exchangeUrl = "https://rsbuddy.com/exchange/summary.json";

        [Command("osbuddyprice")]
        public async Task CurrentRSPriceCommand([Remainder]string itemName = "Old School Bond")
        {
            var exhangeDataList = await GetExchangeDataList();

            if (exhangeDataList == null)
            {
                await ReplyAsync($"Could retrieve any exchange data from '{exchangeUrl}'.");
                return;
            }

            var exchangeData = GetExchangeItemByName(exhangeDataList, itemName);

            if (exchangeData == null)
            {
                await ReplyAsync($"Could not find any item named '{itemName}'.");
                return;
            }

            await SendPriceMessage(exchangeData);
        }

        private async Task SendPriceMessage(OsBuddyExchangeData exchangeData)
        {
            var builder = new EmbedBuilder()
                .WithTitle($"Runescape Price Data")
                .AddField($"Current average price for item '{exchangeData.Name}'", $"{exchangeData.OverallAverage}gp")
                .WithColor(Color.DarkOrange)
                .WithFooter($"Data from: https://rsbuddy.com/exchange")
                .WithCurrentTimestamp();

            await ReplyAsync(embed: builder.Build());
        }

        private async Task<Dictionary<string, OsBuddyExchangeData>> GetExchangeDataList()
        {
            var responds = await _client.GetStringAsync(exchangeUrl);
            return OsBuddyExchangeData.FromJson(responds);
        }

        private OsBuddyExchangeData GetExchangeItemByName(Dictionary<string, OsBuddyExchangeData> exhangeDataList, string itemName)
        {
            return exhangeDataList.FirstOrDefault(x => x.Value.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)).Value;
        }

        private string GetItemIdByName(Dictionary<string, OsBuddyExchangeData> exhangeDataList, string itemName)
        {
            return exhangeDataList.FirstOrDefault(x => x.Value.Name.ToLower() == itemName.ToLower()).Key;
        }

        public class PriceResult
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }

        private async Task<OsBuddyExchangePrice> GetPriceDetails(string id)
        {
            string url = "https://api.rsbuddy.com/grandExchange?a=guidePrice&i=" + id;

            var responds = await _client.GetStringAsync(url);

            return OsBuddyExchangePrice.FromJson(responds);
        }
    }
}
