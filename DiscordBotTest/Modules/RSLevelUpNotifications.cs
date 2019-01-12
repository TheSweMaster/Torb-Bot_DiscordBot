using Discord.Commands;
using DiscordBotTest.Helpers;
using DiscordBotTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class RSLevelUpNotifications : ModuleBase<SocketCommandContext>
    {
        [Command("rslevelup")]
        public async Task OSRSLevelUpNotificationCommand([Remainder]string rsUsername)
        {
            var result = RunescapeAccountWatchList.TryUpdateList(new KeyValuePair<string, RSAccountData>(
                rsUsername.ToLower(), new RSAccountData(new[] { Context.Guild.Id }, RunescapeAccountWatchList.InitialSkillDataList())));
            //Todo: Add support to add to more than one RS username to one server by command

            if (result)
            {
                await ReplyAsync($"'{rsUsername}' was updated successfully!");
            }
            else
            {
                await ReplyAsync($"'{rsUsername}' failed to be updated!");
            }
        }
    }
}
