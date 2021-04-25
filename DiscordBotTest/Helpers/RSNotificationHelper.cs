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
        private static List<KeyValuePair<string, RSAccountData>> _ListToUpdate = new List<KeyValuePair<string, RSAccountData>>();
        public static void LevelUpNotification(DiscordSocketClient client)
        {
            _ListToUpdate.Clear();

            foreach (var keyPair in RunescapeAccountWatchList.GetRunescapeAccountWatchList())
            {
                SendNotificationByAccount(keyPair, client);
            }

            foreach (var item in _ListToUpdate)
            {
                RunescapeAccountWatchList.TryUpdateList(item);
            }
        }

        private static void SendNotificationByAccount(KeyValuePair<string, RSAccountData> keyPair, DiscordSocketClient client)
        {
            const string rs_log = "rs_log";

            var rsUsername = keyPair.Key;
            var oldAccountData = keyPair.Value;

            var stream = RSHighScoreHelper.TryGetStreamData(rsUsername).Result;
            var lines = RSHighScoreHelper.ReadLines(() => stream).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillDataList(lines);

            var totalLevel = RSHighScoreHelper.CalculateTotalLevel(skillDataList);
            var oldTotalLevel = RSHighScoreHelper.CalculateTotalLevel(oldAccountData.SkillDataList);

            if (totalLevel > oldTotalLevel && oldTotalLevel != -1)
            {
                foreach (var serverId in oldAccountData.ServerIds)
                {
                    var guild = client.GetGuild(serverId);
                    if (guild == null)
                    {
                        Console.WriteLine($"Could not find a server with id '{serverId}'");
                        continue;
                    }

                    var channel = guild.TextChannels.SingleOrDefault(c => c.Name == rs_log);
                    if (channel == null)
                    {
                        Console.WriteLine($"Could not find a text channel named '{rs_log}' on the server named '{guild.Name}'");
                        continue;
                    }

                    channel.SendMessageAsync(embed: GetMessage(rsUsername, skillDataList, oldAccountData.SkillDataList, totalLevel, oldTotalLevel).Build()).Wait();
                }
            };
            _ListToUpdate.Add(new KeyValuePair<string, RSAccountData>(rsUsername, new RSAccountData(oldAccountData.ServerIds, skillDataList)));
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
