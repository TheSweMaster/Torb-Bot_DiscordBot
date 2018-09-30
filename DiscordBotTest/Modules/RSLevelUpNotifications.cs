using Discord;
using Discord.Commands;
using DiscordBotTest.Helpers;
using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class RSLevelUpNotifications : ModuleBase<SocketCommandContext>
    {
        [Command("rslevelup")]
        public async Task OSRSLevelUpNotificationCommand([Remainder]string rsUsername)
        {
            var result = RunescapeAccountWatchList.TryUpdateList(rsUsername.ToLower(), -1);

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
