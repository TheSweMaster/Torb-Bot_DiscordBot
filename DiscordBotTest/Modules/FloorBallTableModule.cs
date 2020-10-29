using Discord;
using Discord.Commands;
using DiscordBotTest.Helpers;
using DiscordBotTest.Models;
using DiscordBotTest.Runners;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Command("floorballrunner")]
        public async Task FloorBallRunnerCommand(string input)
        {
            if (int.TryParse(input, out int selectedRound))
            {
                new UpdateFloorballSheetRunner().Run(selectedRound);
            }

            await ReplyAsync("Runner completed!");
        }

        private static async Task<List<FloorBallTable>> GetFloorballData()
        {
            var url = "http://statistik.innebandy.se/ft.aspx?scr=table&ftid=28330";

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

            foreach (var item in floorBallTableList)
            {
                builder.AddField(title: $"{item.Rank} {GetTeamEmoji(item.Team)} {item.Team}",
                    text: $"Played: {item.Played}, Wins: {item.Wins}, Draws: {item.Draws}, Losses: {item.Losses}, Goal Diff: {item.GoalDifferens}, Points: {item.Points}");
            }

            return builder;
        }


        private static string GetTeamEmoji(string teamName)
        {
            var builder = new StringBuilder(teamName);
            builder.Replace(" ", "");
            builder.Replace("\t", "");
            builder.Replace("å", "");
            builder.Replace("ä", "");
            builder.Replace("ö", "");
            builder.Replace("Å", "");
            builder.Replace("Ä", "");
            builder.Replace("Ö", "");
            builder.Replace("/", "");

            var key = builder.ToString();
            var result = EmojiCodekeyValues.TryGetValue(key, out var value);

            if (!result)
            {
                return "";
            }

            return $"<:{key}:{value}>";
        }

        private static Dictionary<string, string> EmojiCodekeyValues => new Dictionary<string, string>
            {
                {"storpKvidingeIBS", "637452238557806594" },
                {"FagerhultHaboIBK", "637452239627354112" },
                {"KarlstadIBF", "637452238528315406" },
                {"LaganIBK", "637452238620721174" },
                {"LillnIBK", "637452239145009182" },
                {"LindsRastaIBK", "637452238977237012" },
                {"IBKLidkping", "637452239140683787" },
                {"IBKLockerudMariestad", "637452238851276840" },
                {"NykvarnsIBFUngdom", "637452239136620560" },
                {"OnyxIBK", "637452239577022489" },
                {"IBFrebro", "637452239627092002" },
                {"CraftstadensIBKOskarshamn", "637452239115517963" },
            };
    }
}
