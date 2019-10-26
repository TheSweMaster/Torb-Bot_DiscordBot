using Discord;
using Discord.Commands;
using DiscordBotTest.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class FloorBallTableModule : ModuleBase<SocketCommandContext>
    {
        [Command("floorball")]
        public async Task FloorBallTableCommand()
        {
            var floorBallTableList = await GetFloorballData();

            var builder = BuildMessage(floorBallTableList);

            await ReplyAsync("", embed: builder.Build());
        }

        private static async Task<List<FloorBallTable>> GetFloorballData()
        {
            var url = "http://statistik.innebandy.se/ft.aspx?scr=table&ftid=11565";

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            var valueNodes = doc.DocumentNode.SelectNodes("//*[@id='IbisInfo']/table[2]/tbody/tr/td")
                .Where(x => !x.HasClass("ext")).ToList();

            var floorBallTableList = new List<FloorBallTable>();
            for (int i = 0; i < valueNodes.Count; i += 9)
            {
                floorBallTableList.Add(
                    new FloorBallTable(
                        valueNodes[i].InnerText,
                        valueNodes[i + 1].InnerText,
                        valueNodes[i + 2].InnerText,
                        valueNodes[i + 3].InnerText,
                        valueNodes[i + 4].InnerText,
                        valueNodes[i + 5].InnerText,
                        valueNodes[i + 6].InnerText,
                        valueNodes[i + 7].InnerText,
                        valueNodes[i + 8].InnerText)
                    );
            }

            return floorBallTableList;
        }

        private static EmbedBuilder BuildMessage(List<FloorBallTable> floorBallTableList)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle($"Swedish Floorball League Stats - Allsvenskan Södra")
                .WithFooter($"Official data from IBIS")
                .WithCurrentTimestamp()
                .WithColor(Color.Blue);

            var dummyEmoji = "<:Lindas:637452238977237012>";

            foreach (var item in floorBallTableList)
            {
                builder.AddField(title: $"{item.Rank} {dummyEmoji} {item.Team}", 
                    text: $"Played: {item.Played}, Wins: {item.Wins}, Draws: {item.Draws}, Losses: {item.Losses}, Goal Diff: {item.GoalDifferens}, Points: {item.Points}");
            }

            return builder;
        }
    }
}
