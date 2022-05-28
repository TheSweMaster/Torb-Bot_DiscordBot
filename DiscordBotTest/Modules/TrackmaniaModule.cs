using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTest.Models.Trackmania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class TrackmaniaModule : ModuleBase<SocketCommandContext>
    {
        [Command("tm")]
        public async Task TrackManiaMapLeaderBoardCheckTest()
        {
            var httpClient = new HttpClient();
            var trackManiaMapLeaderboardModelCache = new TrackManiaMapLeaderboardModelCache()
            {
                TrackManiaMapLeaderboardCache = new TrackManiaMapLeaderboardModel()
                {
                    Tops = new List<Top>()
                }
            };
            await TrackManiaNewRecordNotification.TrySendNotification(Context.Client, httpClient, trackManiaMapLeaderboardModelCache);

            await ReplyAsync("Completed TrackManiaMapLeaderBoardCheckTest!");
        }
    }

    public class TrackManiaMapLeaderboardModelCache
    {
        public TrackManiaMapLeaderboardModel TrackManiaMapLeaderboardCache { get; set; }
    }

    public static class TrackManiaNewRecordNotification
    {
        public static async Task TrySendNotification(DiscordSocketClient client, HttpClient httpClient, TrackManiaMapLeaderboardModelCache trackManiaMapLeaderboardModelCache)
        {
            const string tm_log = "tm_log";

            var guild = client.GetGuild(Program.MyServerId);
            if (guild == null)
            {
                Console.WriteLine($"Could not find a server with id '{Program.MyServerId}'");
                return;
            }

            var channel = guild.TextChannels.SingleOrDefault(c => c.Name == tm_log);
            if (channel == null)
            {
                Console.WriteLine($"Could not find a text channel named '{tm_log}' on the server named '{guild.Name}'");
                return;
            }

            var oldData = trackManiaMapLeaderboardModelCache.TrackManiaMapLeaderboardCache;
            var recentData = await new TrackManiaRecordGetter(httpClient).GetTmData();

            if (oldData == null)
            {
                Console.WriteLine($"Cache was empty!");
                await channel.SendMessageAsync($"The TrackManiaNewRecordNotification was initialized!");
                trackManiaMapLeaderboardModelCache.TrackManiaMapLeaderboardCache = recentData;
                return;
            }

            if (oldData.Playercount != recentData.Playercount)
            {
                var oldNames = oldData.Tops.Select(x => x.Player.Name);
                var recentNames = recentData.Tops.Select(x => x.Player.Name);

                var newRecordNames = recentNames.Except(oldNames).ToList();
                foreach (var newRecordName in newRecordNames)
                {
                    await channel.SendMessageAsync($"The player {newRecordName} got a new record on the map https://trackmania.exchange/maps/59504/009-dreamscape <@{Program.MyUserId}>");
                }

                trackManiaMapLeaderboardModelCache.TrackManiaMapLeaderboardCache = recentData;
            }
        }
    }

    public class TrackManiaRecordGetter
    {
        private readonly HttpClient _httpClient;
        public TrackManiaRecordGetter(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _httpClient.DefaultRequestHeaders.Remove("User-Agent");
            }

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "TheSweMaster#8595  Map record notification updates");
        }

        public async Task<TrackManiaMapLeaderboardModel> GetTmData()
        {
            const string mapId = "svIF0Z8WLQ1pQmJqEGl5xG0Qg52";
            var url = "https://trackmania.io/api/leaderboard/map/" + mapId;

            var jsonResponds = await _httpClient.GetStringAsync(url);

            return TrackManiaMapLeaderboardModel.FromJson(jsonResponds);
        }
    }
}
