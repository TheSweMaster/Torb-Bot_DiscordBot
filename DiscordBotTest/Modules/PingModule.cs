using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    //[Group("ping")]
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        private readonly string botInviteLink = "https://discordapp.com/api/oauth2/authorize?client_id=437972563876904970&scope=bot&permissions=1";

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
            await MakeNicePing();
        }

        private async Task TestContext()
        {
            EmbedBuilder builder = new EmbedBuilder();

            //The Discord Bot Entity: Context.Client;
            //The Discord Server-Channel: Context.Guild;    
            //The Discord User (writing the command): Context.User;
            //The Message the bots sends: Context.Message;
            // Context.Channel;

            await ReplyAsync($"{Context.Client.CurrentUser.Mention} || {Context.User.Mention} sent " +
                $"{Context.Message.Content} in {Context.Guild.Name}!");
        }

        private async Task MakeNicePing()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Ping!")
                .WithDescription("This is a really nice ping!")
                .WithColor(Color.Green);

            await ReplyAsync("", false, builder.Build());
        }
        
    }
}
