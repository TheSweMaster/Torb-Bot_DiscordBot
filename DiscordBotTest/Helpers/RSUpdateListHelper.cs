using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Helpers
{
    public static class RSUpdateListHelper
    {
        public static async Task UpdateAllNickNameTotalLevel(DiscordSocketClient client, ulong serverId)
        {
            var guild = client.GetGuild(serverId);

            foreach (var keyPair in RunescapeAccountList.GetRunescapeAccountList())
            {
                await UpdateNickNameOnUser(guild, keyPair, client);
            }
        }

        public static async Task UpdateNickNameOnUser(SocketGuild guild, KeyValuePair<string, string> keyPair, DiscordSocketClient client)
        {
            var username = keyPair.Key.Split('#')[0];
            var discriminator = keyPair.Key.Split('#')[1];
            var user = client.GetUser(username, discriminator);
            var guildUser = guild.GetUser(user.Id);
            var oldNickname = guildUser.Nickname ?? guildUser.Username;

            string newNickname = "";

            if (oldNickname.Split('(') != null || oldNickname.Split('(').Count() == 2)
            {
                newNickname = oldNickname.Split('(')[0] + $"({await GetHighScoreTotalLevel(keyPair.Value)}/2277)";
            }
            else
            {
                newNickname = oldNickname + $" ({await GetHighScoreTotalLevel(keyPair.Value)}/2277)";
            }
            await guildUser.ModifyAsync(x => x.Nickname = $"{newNickname}");
        }

        private static async Task<string> GetHighScoreTotalLevel(string rsUsername)
        {
            var streamData = await RSHighScoreHelper.TryGetStreamData(rsUsername);
            var lines = RSHighScoreHelper.ReadLines(() => streamData).ToList();
            var skillDataList = RSHighScoreHelper.GetSkillTotalLevel(lines);
            return skillDataList.ToString();
        }
    }
}
