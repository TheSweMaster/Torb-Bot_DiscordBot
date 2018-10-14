using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTest.Helpers;
using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordBotTest.Modules
{
    public class RSHighScoreModule : ModuleBase<SocketCommandContext>
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        [Command("rshighscore")]
        public async Task OSRSHighScoreCommand([Remainder]string rsUsername = "TheSweMaster")
        {
            var stream = await TryGetStreamData(rsUsername);

            var lines = RSHighScoreHelper.ReadLines(() => stream).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillDataList(lines);

            var builder = GetMessage(rsUsername, skillDataList, false);

            await ReplyAsync("", embed: builder.Build());
        }

        [Command("rshighscoreall")]
        public async Task OSRSHighScoreAllCommand([Remainder]string rsUsername = "TheSweMaster")
        {
            var stream = await TryGetStreamData(rsUsername);

            var lines = RSHighScoreHelper.ReadLines(() => stream).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillDataList(lines);

            var builder = GetMessage(rsUsername, skillDataList, true);

            await ReplyAsync("", embed: builder.Build());
        }

        [Command("rstotallevel")]
        public async Task UpdateTotalLevelListCommand([Remainder]string rsUsername = "swemasterx")
        {
            var userName = Context.User.Username + "#" + Context.User.Discriminator;

            var result = RunescapeAccountList.TryUpdateList(userName, rsUsername);

            if (result)
            {
                await RSUpdateListHelper.UpdateNickNameOnUser(Context.Guild, new KeyValuePair<string, string>(userName, rsUsername), Context.Client);
                return;
            }
            await ReplyAsync("Something went wrong! :(");
        }

        private async Task<Stream> TryGetStreamData(string rsUsername)
        {
            Stream stream;
            try
            {
                stream = await _httpClient.GetStreamAsync($"http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player={rsUsername}");
            }
            catch (Exception)
            {
                await ReplyAsync($"Could not get any Runescape highscore data with the username: '{rsUsername}'");
                throw;
            }

            return stream;
        }

        private static EmbedBuilder GetMessage(string rsUsername, List<SkillData> skillDataList, bool showAll)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle($"OSRS Highscores - {rsUsername} ")
                //.WithUrl("http://services.runescape.com/m=hiscore_oldschool/hiscorepersonal.ws?user1=" + rsUsername)
                .WithFooter($"Official Old-School Runescape Highscore Data")
                .WithCurrentTimestamp()
                .WithColor(Color.DarkOrange);

            foreach (var item in skillDataList)
            {
                if (showAll)
                {
                    var displayRank = item.Rank == -1 ? "Unranked" : $"{item.Rank} Rank";
                    builder.AddInlineField($"{item.EmojiCode} {item.Skill}", $"{item.Level} Levels | {item.Xp} Xp | {displayRank}");
                }
                else
                {
                    builder.AddInlineField($"{item.EmojiCode} {item.Skill}", $"{item.Level} Levels | {item.Xp} Xp");
                }
                //.AddInlineField($"{item.Level}", item.Xp.ToString());
            }

            return builder;
        }
        
    }
}
