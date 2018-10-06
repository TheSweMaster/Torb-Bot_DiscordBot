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
            _ListToUpdate.Clear();
            var guild = client.GetGuild(serverId);

            foreach (var keyPair in RunescapeAccountWatchList.GetRunescapeAccountWatchList())
            {
                SendNotificationByAccount(guild, keyPair, client);
            }

            foreach (var item in _ListToUpdate)
            {
                RunescapeAccountWatchList.TryUpdateList(item);
            }
        }

        private static void SendNotificationByAccount(SocketGuild guild, KeyValuePair<string, List<SkillData>> keyPair, DiscordSocketClient client)
        {
            const string rs_log = "rs_log";
            const ulong scrubsServerId = 202956682932912129;
            var scrubsGuild = client.GetGuild(scrubsServerId);

            var myChannel = guild.TextChannels.SingleOrDefault(c => c.Name == rs_log);
            var scrubsChannel = scrubsGuild.TextChannels.SingleOrDefault(c => c.Name == rs_log);

            var rsUsername = keyPair.Key;
            var oldSkillDataList = keyPair.Value;

            var stream = RSHighScoreHelper.TryGetStreamData(rsUsername).Result;
            var lines = RSHighScoreHelper.ReadLines(() => stream).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillDataList(lines);

            var totalLevel = RSHighScoreHelper.CalculateTotalLevel(skillDataList);
            var oldTotalLevel = RSHighScoreHelper.CalculateTotalLevel(oldSkillDataList);

            if (totalLevel > oldTotalLevel && oldTotalLevel != -1)
            {
                myChannel.SendMessageAsync("", embed: GetMessage(rsUsername, skillDataList, oldSkillDataList, totalLevel, oldTotalLevel)).Wait();
                scrubsChannel.SendMessageAsync("", embed: GetMessage(rsUsername, skillDataList, oldSkillDataList, totalLevel, oldTotalLevel)).Wait();
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
