using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Timers;

namespace DiscordBotTest.Modules
{
    public class SetReminder : ModuleBase<SocketCommandContext>
    {
        [Command("reminder")]
        public async Task SetReminderCommand(string time, [Remainder]string message)
        {
            TimeSpan timeSpan = TimeSpan.TryParse(time, out timeSpan) ? timeSpan : TimeSpan.Zero;
            if (timeSpan.Minutes < 1 && timeSpan.Hours > 24)
            {
                await ReplyAsync("Unvalid time, please choose a time between 1 min (00:01:00) and 24 hours (24:00:00).");
                return;
            }

            var userReminder = new UserReminder()
            {
                UserId = Context.User.Id,
                TimeSpan = timeSpan,
                Message = message,
            };

            userReminder.Timer = new Timer(userReminder.TimeSpan.TotalMilliseconds)
            {
                Enabled = true,
            };
            userReminder.Timer.Elapsed += async (sender, e) =>
            {
                await SendReminderMessage(userReminder);
            };

            await ReplyAsync($"Reminder was set!");
        }

        [Command("reminderuser")]
        public async Task SetReminderCommand(SocketGuildUser user, string time, [Remainder]string message)
        {
            TimeSpan timeSpan = TimeSpan.TryParse(time, out timeSpan) ? timeSpan : TimeSpan.Zero;
            if (timeSpan.Minutes < 1 || timeSpan.Hours > 24)
            {
                await ReplyAsync("Unvalid time, please choose a time between 1 min (00:01:00) and 24 hours (24:00:00).");
                return;
            }

            var userReminder = new UserReminder()
            {
                UserId = user.Id,
                TimeSpan = timeSpan,
                Message = message,
            };

            userReminder.Timer = new Timer(userReminder.TimeSpan.TotalMilliseconds)
            {
                Enabled = true,
            };
            userReminder.Timer.Elapsed += async (sender, e) =>
            {
                await SendReminderMessage(userReminder);
            };

            await ReplyAsync($"Reminder was set!");
        }

        private async Task SendReminderMessage(UserReminder userReminder)
        {
            var user = Context.Client.GetUser(userReminder.UserId);
            await user.SendMessageAsync($"Reminder after {userReminder.TimeSpan.ToString(@"hh\:mm\:ss")} with the message '{userReminder.Message}'");
            userReminder.HasReminded = true;
            userReminder.Timer.Dispose();
        }

    }
}
