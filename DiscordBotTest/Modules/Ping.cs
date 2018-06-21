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
    public class Ping : ModuleBase<SocketCommandContext>
    {
        private readonly string botInviteLink = "https://discordapp.com/api/oauth2/authorize?client_id=437972563876904970&scope=bot&permissions=1";

        [Command("botlink")]
        public async Task GetBotInviteLink()
        {
            await Context.User.SendMessageAsync($"Click the link and add this bot to your server \n{botInviteLink}");
        }

        [Command("greg")]
        public async Task GetGreg()
        {
            var url = "http://www.house.leg.state.mn.us/hinfo/memberimgls90/28B.jpg?v=2018";

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Greg")
                .WithDescription("Greg is a noob!")
                .WithColor(Color.Green)
                .WithImageUrl(url);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("chatgf")]
        public async Task ChatWithGF()
        {
            var r = new Random();
            string[] chatLines = 
            {
                "I like to go to the beach for a nice walk in the evenings, Wanna join?",
                "Do you wanna see a movie with me?",
                "What is your favorite food?",
                "I work as nurse, What do you do for a living?",
                "RIP Avicii :'(",
                "I really hate that Greg, Don't you think he is an asshole?",
                "Do you want tea, a bath or me?",
            };

            var url = "https://vignette.wikia.nocookie.net/animal-jam-clans-1/images/b/b8/A66fc890c3769dd3826313d2e0732d08--pretty-anime-girl-manga-art.jpg";

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Torbitte")
                .WithDescription($"Hi {Context.User.Username}! \n{chatLines[r.Next(0, 6)]}")
                .WithColor(Color.Magenta)
                .WithImageUrl(url);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("message")]
        public async Task Message(SocketGuildUser user, [Remainder]string text)
        {
            await user.SendMessageAsync(text);
            string name = user.Nickname ?? user.Username;

            await ReplyAsync($"Message sent to {name}");
        }

        [Command("ping user")]
        public async Task Say(SocketGuildUser user)
        {
            await ReplyAsync($"Pong! {user.Mention}");
        }

        [Command("say-admin"), RequireUserPermission(GuildPermission.Administrator)]
        public async Task SayAdmin([Remainder] string stuffToSay)
        {
            await ReplyAsync($"{stuffToSay}");
        }

        [Command("say")]
        public async Task Say([Remainder] string stuffToSay)
        {
            await ReplyAsync($"{stuffToSay}");
        }

        [Command("noob")]
        public async Task SayIsNoob(string name = "Greg")
        {
            await ReplyAsync($"{name} is a noob");
        }

        [Command("ping")]
        public async Task PingAsync()
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

        private async Task PingTextFields()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.AddField("Field 1", "test")
                .AddInlineField("Field 2", "test")
                .AddInlineField("field 3", "test");

            await ReplyAsync("", false, builder.Build());
        }

    }
}
