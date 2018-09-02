using Discord;
using Discord.Commands;
using DiscordBotTest.OverwatchModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class OverwatchStatsModule : ModuleBase<SocketCommandContext>
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string Url = "https://ow-api.com/v1/stats";

        private static readonly string[] ValidPlatforms = { "pc", "xbox", "playstation" };
        private static readonly string[] ValidRegions = { "eu", "us", "asia" };

        [Command("owstats")]
        public async Task OverwatchStatsCommand(string platform, string region, [Remainder]string battletag)
        {
            platform = platform.ToLower();
            region = region.ToLower();
            var fixedBattletag = battletag.Replace('#', '-');

            if (!ValidPlatforms.Any(x => x == platform))
            {
                await ReplyAsync($"The platform '{platform}' is invalid. Valid platforms: {string.Join(", ", ValidPlatforms)}");
                return;
            }

            if (!ValidRegions.Any(x => x == region))
            {
                await ReplyAsync($"The region '{region}' is invalid. Valid regions: {string.Join(", ", ValidRegions)}");
                return;
            }

            var responseString = await TryGetApiData(platform, region, fixedBattletag);

            var overwatchData = OverwatchStats.FromJson(responseString);

            if (string.IsNullOrEmpty(overwatchData.Name))
            {
                await ReplyAsync($"Could not get any profile stats on platform '{platform}' in region '{region}' with the battletag '{battletag}'");
                return;
            }
            
            var builder = GetMessage(overwatchData);

            await ReplyAsync("", embed: builder.Build());
        }


        private async Task<string> TryGetApiData(string platform, string region, string battletag)
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync($"{Url}/{platform}/{region}/{battletag}/profile");

                return httpResponse.StatusCode == HttpStatusCode.OK 
                    ? await httpResponse.Content.ReadAsStringAsync() 
                    : "";
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Unexpected error, Something went wrong!");
                throw ex;
            }
        }

        private static EmbedBuilder GetMessage(OverwatchStats overwatchStats)
        {
            var raiting = overwatchStats.Rating != 0 
                ? $"{overwatchStats.RatingName} - {overwatchStats.Rating}"
                : "Unranked";

            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle($"Overwatch Stats - {overwatchStats.Name} ")
                .WithDescription($"Raiting: {raiting}\n" +
                $"Total Level: {overwatchStats.Prestige * 100 + overwatchStats.Level}\n" +
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
