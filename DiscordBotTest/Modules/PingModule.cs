using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        private readonly string botInviteLink = "https://discordapp.com/api/oauth2/authorize?client_id=437972563876904970&scope=bot&permissions=1813466177";

        [Command("botlink")]
        public async Task BotInviteLinkCommand()
        {
            await Context.User.SendMessageAsync($"Click the link and add this bot to your server \n{botInviteLink}");
        }
        
        [Command("message")]
        public async Task MessageCommand(SocketGuildUser user, [Remainder]string text)
        {
            await user.SendMessageAsync(text);
            string name = user.Nickname ?? user.Username;

            await ReplyAsync($"Message sent to {name}");
        }
        
        [Command("say-admin"), RequireUserPermission(GuildPermission.Administrator)]
        public async Task SayAdminCommand([Remainder] string stuffToSay)
        {
            await ReplyAsync($"{stuffToSay}");
        }

        [Command("say")]
        public async Task SayCommand([Remainder] string stuffToSay)
        {
            await ReplyAsync($"{stuffToSay}");
        }
        
        [Command("ping")]
        public async Task PingCommand()
        {
            await SendPingMessage();
        }

        private async Task SendPingMessage()
        {
            var latency = Context.Client.Latency;

            Color color;
            if (latency <= 100)
            {
                color = Color.Green;
            }
            else if (latency <= 250)
            {
                color = Color.LightOrange;
            }
            else
            {
                color = Color.Red;
            }

            var builder = new EmbedBuilder()
                .WithTitle("Pong!")
                .WithDescription($"The latency is {latency}ms.")
                .WithColor(color);

            await ReplyAsync(embed: builder.Build());
        }
    }
}
