using Discord;
using Discord.WebSocket;
using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Helpers
{
    public static class RSNotificationHelper
    {
        private static List<KeyValuePair<string, int>> _ListToUpdate = new List<KeyValuePair<string, int>>();
        public static void LevelUpNotification(DiscordSocketClient client, ulong serverId)
        {
            _ListToUpdate.Clear();
            var guild = client.GetGuild(serverId);

            foreach (var keyPair in RunescapeAccountWatchList.GetRunescapeAccountWatchList())
            {
                SendNotificationByAccount(guild, keyPair, client).Wait();
            }

            foreach (var item in _ListToUpdate)
            {
                RunescapeAccountWatchList.TryUpdateList(item);
            }
        }

        private static async Task SendNotificationByAccount(SocketGuild guild, KeyValuePair<string, int> keyPair, DiscordSocketClient client)
        {
            var channel = guild.TextChannels.SingleOrDefault(c => c.Name == "rs_log");

            var rsUsername = keyPair.Key;
            var stream = await RSHighScoreHelper.TryGetStreamData(rsUsername);

            var lines = RSHighScoreHelper.ReadLines(() => stream).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillDataList(lines);

            var oldTotalLevel = keyPair.Value;
            var totalLevel = skillDataList.SingleOrDefault(x => x.Skill == "Overall").Level;

            if (totalLevel > oldTotalLevel && oldTotalLevel != -1)
            {
                await channel.SendMessageAsync("", embed: GetMessage(oldTotalLevel, rsUsername, skillDataList, totalLevel));
            };
            _ListToUpdate.Add(new KeyValuePair<string, int>(rsUsername, totalLevel));
        }

        private static EmbedBuilder GetMessage(int oldTotalLevel, string rsUsername, List<SkillData> skillDataList, int totalLevel)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle($"OSRS - Level Up Notification")
                .WithDescription($"{rsUsername} gained {totalLevel - oldTotalLevel} total levels!\n" +
                $"Now has a total level of {totalLevel} :clap:")
                .WithColor(Color.DarkOrange);

            // Add what skill updated (from --> to in level per skill)
            //foreach (var item in skillDataList)
            //{
            //    builder.AddInlineField($"{item.EmojiCode} {item.Skill}", item.Level);
            //    //.AddInlineField($"{item.Level}", item.Xp.ToString());
            //}

            return builder;
        }
    }
}
