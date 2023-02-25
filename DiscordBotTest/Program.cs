using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTest.Helpers;
using DiscordBotTest.Modules;
using DiscordBotTest.Runners;
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
        private TrackManiaMapLeaderboardModelCache _trackManiaMapLeaderboardCache;
        private IServiceProvider _services;
        private readonly string _botToken = Configuration.GetAppSettings().Keys.BotToken;
        private readonly Timer CheckForUpdateTimer = new Timer(30 * 60 * 1000);
        private readonly Timer CheckForUpdate1mTimer = new Timer(60 * 1000);
        public const ulong MyServerId = 199189022894063627;
        private readonly ulong _testServerId = 430643719880835072;
        public const ulong MyUserId = 198806112852508672;
        // private static readonly string _testUser = "TheSweMasterX#5203";

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _trackManiaMapLeaderboardCache = new TrackManiaMapLeaderboardModelCache();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_httpClient)
                .BuildServiceProvider();

            // Event subsciptions
            _client.Log += LogEvent;
            _client.UserJoined += UserJoinedEvent;

            CheckForUpdate1mTimer.Enabled = true;

            CheckForUpdate1mTimer.Elapsed += new ElapsedEventHandler(TmNewRecordNotification);
            //CheckForUpdateTimer.Elapsed += new ElapsedEventHandler(UpdateTotalLevelEvent);
            //CheckForUpdateTimer.Elapsed += new ElapsedEventHandler(LevelUpNotificationsEvent);
            //CheckForUpdateTimer.Elapsed += new ElapsedEventHandler(UpdateFloorBallSheetEvent);

            await SetBotGameStatus();

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async void TmNewRecordNotification(object sender, ElapsedEventArgs e)
        {
            await TrackManiaNewRecordNotification.TrySendNotification(_client, _httpClient, _trackManiaMapLeaderboardCache);
        }

        private void UpdateFloorBallSheetEvent(object sender, ElapsedEventArgs e)
        {
            new UpdateFloorballSheetRunner().Run(0);
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
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            await channel.SendMessageAsync($":wave: Welcome {user.Mention} to {guild.Name}!");
        }

        private Task LogEvent(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
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
