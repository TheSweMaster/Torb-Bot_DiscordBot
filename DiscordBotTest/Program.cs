using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordBotTest
{
    public class Program
    {
        public static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private static readonly HttpClient _httpClient = new HttpClient();
        private CommandService _commands;
        private IServiceProvider _services;
        private readonly string _botToken = Configuration.GetAppSettings().Keys.BotToken;
        private readonly Timer CheckForUpdateTimer = new Timer(30 * 60 * 1000);
        private readonly ulong _myServerId = 199189022894063627;
        private readonly ulong _testServerId = 430643719880835072;
        private static readonly ulong _myUserId = 198806112852508672;
        private static readonly string _testUser = "TheSweMasterX#5203";

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            // Event subsciptions
            _client.Log += LogEvent;
            _client.UserJoined += UserJoinedEvent;

            CheckForUpdateTimer.Enabled = true;
            //CheckForUpdateTimer.Elapsed += new ElapsedEventHandler(UpdateTotalLevelEvent);
            //CheckForUpdateTimer.Elapsed += new ElapsedEventHandler(LevelUpNotificationsEvent);

            await SetBotGameStatus();

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private void LevelUpNotificationsEvent(object sender, ElapsedEventArgs e)
        {
            RSNotificationHelper.LevelUpNotification(_client);
        }

        private void UpdateTotalLevelEvent(object source, ElapsedEventArgs e)
        {
            RSUpdateListHelper.UpdateAllNickNameTotalLevel(_client, _testServerId).Wait();
        }

        private async Task SetBotGameStatus()
        {
            await _client.SetGameAsync("as Torbjörn | !!help");
        }

        private async Task UserJoinedEvent(SocketGuildUser user)
        {
            //if (KickedBotUser(user))
            //    return;

            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            await channel.SendMessageAsync($":wave: Welcome {user.Mention} to {guild.Name}!");
        }

        private bool KickedBotUser(SocketGuildUser user)
        {
            var pattern = @"(twitch.tv\/)|(youtube.com\/)|(discord.gg\/)|(twitter.com\/)|(facebook.com\/)";
            
            if (Regex.IsMatch(user.Username, pattern, RegexOptions.IgnoreCase))
            {
                user.KickAsync("Kicked a user for advertising.");
                return true;
            }
            else
            {
                return false;
            }
        }

        private Task LogEvent(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message) || message.Author.IsBot)
                return;

            int argPos = 0;

            if (message.HasStringPrefix("!!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
