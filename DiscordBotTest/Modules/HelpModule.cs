using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpCommand()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"-- All Tor-Bot Commands (Beta) --")
                .WithDescription("The bot is still under development, If you have any suggestions let me know! :)")
                .AddField("!!botlink", "Gets the invite link for the bot (Only for Admins).")
                .AddField("!!ping", "Ping the bot and get a responds with the latency.")
                .AddField("!!icons <message>", "Converts the message to a series of icons/emojis.")
                .AddField("!!emojis <message>", "Converts the message to a series of icons/emojis.")
                .AddField("!!opencase", "Opens a random CSGO case.")
                .AddField("!!opencases <amount>", "Open a specific amout of CSGO cases.")
                .AddField("!!weather <city>", "Shows the current weather at the specified city.")
                .AddField("!!rolldice", "Roll a dice and see what you get.")
                .AddField("!!message <@user> <message here>", "Bot sends a private message to a user.")
                .AddField("!!reminder <time> <message>", "Sends you a PM after the time has passed with a message. Timeformat: 'HH:mm:ss'")
                .AddField("!!reminderuser <@user> <time> <message>", "Sends the user a PM after the time has passed with a message. Timeformat: 'HH:mm:ss'")
                .AddField("!!rsprice <item name>", "Gets the current market price of a Runescape item.")
                .AddField("!!rshighscore <username>", "Shows the current Runescape levels by username.")
                .AddField("!!floorball", "Shows the current stats from the Swedish floor ball league Allsvenskan Södra.")
                .AddField("!!owstats <platform> <region> <battletag>", "Shows the current Overwatch stat by profile." +
                "\n Platforms:(pc, xbox, playstation), Regions:(eu, us, asia), Battletag is case sensetive")
                .WithFooter("Created and Hosted by @TheSweMaster#8595")
                .WithColor(Color.Orange);

            await ReplyAsync("", false, builder.Build());
        }
    }
}
