using Discord;
using Discord.WebSocket;
using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTest.Helpers
{
    public static class RSNotificationHelper
    {
        private static List<KeyValuePair<string, List<SkillData>>> _ListToUpdate = new List<KeyValuePair<string, List<SkillData>>>();
        public static void LevelUpNotification(DiscordSocketClient client, ulong serverId)
        {
            var guild = client.GetGuild(serverId);

            foreach (var keyPair in RunescapeAccountWatchList.GetRunescapeAccountWatchList())
            {
                SendNotificationByAccount(guild, keyPair, client).Wait();
            }

            foreach (var item in _ListToUpdate)
            {
                RunescapeAccountWatchList.TryUpdateList(item);
            }
            _ListToUpdate.Clear();
        }

        private static async Task SendNotificationByAccount(SocketGuild guild, KeyValuePair<string, List<SkillData>> keyPair, DiscordSocketClient client)
        {
            var channel = guild.TextChannels.SingleOrDefault(c => c.Name == "rs_log");
            var jonteUserId = 199879807947898880ul;
            var jonteUser = guild.Users.FirstOrDefault(x => x.Id == jonteUserId);

            var rsUsername = keyPair.Key;
            var oldSkillDataList = keyPair.Value;

            var stream = await RSHighScoreHelper.TryGetStreamData(rsUsername);
            var lines = RSHighScoreHelper.ReadLines(() => stream).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillDataList(lines);

            var totalLevel = RSHighScoreHelper.CalculateTotalLevel(skillDataList);
            var oldTotalLevel = RSHighScoreHelper.CalculateTotalLevel(oldSkillDataList);

            if (totalLevel > oldTotalLevel && oldTotalLevel != -1)
            {
                await channel.SendMessageAsync("", embed: GetMessage(rsUsername, skillDataList, oldSkillDataList, totalLevel, oldTotalLevel));
                if (rsUsername == "skorpish")
                {
                    await jonteUser.SendMessageAsync("", embed: GetMessage(rsUsername, skillDataList, oldSkillDataList, totalLevel, oldTotalLevel));
                }
            };
            _ListToUpdate.Add(new KeyValuePair<string, List<SkillData>>(rsUsername, skillDataList));
        }

        private static EmbedBuilder GetMessage(string rsUsername, List<SkillData> skillDataList, List<SkillData> oldSkillDataList, int totalLevel, int oldTotalLevel)
        {
            var builder = new EmbedBuilder()
                .WithTitle($"OSRS Notification - {UppercaseFirst(rsUsername)} Leveled Up!")
                .WithColor(Color.DarkOrange);

            // Display what skill updated (from --> to in level per skill)
            var updatedSkillDataList = skillDataList.Where(sd => oldSkillDataList.Any(osd => sd.Level > osd.Level && sd.Skill == osd.Skill)).ToList();
            foreach (var skillData in updatedSkillDataList)
            {
                var oldSkillLevel = oldSkillDataList.SingleOrDefault(osd => skillData.Skill == osd.Skill).Level;
                builder.AddField($"{skillData.EmojiCode} {skillData.Skill}", $"{oldSkillLevel} --> {skillData.Level}");
            }

            return builder;
        }

        private static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
