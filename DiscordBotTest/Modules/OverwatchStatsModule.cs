using Discord;
using Discord.Commands;
using DiscordBotTest.OverwatchModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class OverwatchStatsModule : ModuleBase<SocketCommandContext>
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string Url = "https://ow-api.com/v1/stats";

        [Command("owstats")]
        public async Task GetOverwatchStatsCommand(string platform = "pc", string region = "eu", [Remainder]string battletag = "TheSweMaster#2929")
        {
            var fixedBattletag = battletag.Replace('#', '-');
            var datastring = await TryGetApiData(platform, region, fixedBattletag);

            var overwatchData = OverwatchStats.FromJson(datastring);

            var builder = GetMessage(overwatchData);

            await ReplyAsync("", embed: builder.Build());
        }


        private async Task<string> TryGetApiData(string platform, string region, string battletag)
        {
            try
            {
                return await _httpClient.GetStringAsync($"{Url}/{platform}/{region}/{battletag}/profile");
            }
            catch (Exception)
            {
                await ReplyAsync($"Could not get any stats on platform '{platform}' in region '{region}' with the battletag '{battletag}'");
                throw;
            }
        }

        private static EmbedBuilder GetMessage(OverwatchStats overwatchStats)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle($"Overwatch Stats - {overwatchStats.Name} ")
                .WithDescription($"Raiting: {overwatchStats.RatingName} - {overwatchStats.Rating}\n" +
                $"Games Won: {overwatchStats.GamesWon}")
                // Todo: Add rank emoji icons
                .WithFooter($"Data from https://ow-api.com/")
                .WithCurrentTimestamp()
                .WithColor(Color.LightOrange)
                .WithImageUrl(overwatchStats.RatingIcon);
            
            return builder;
        }
    }
}
