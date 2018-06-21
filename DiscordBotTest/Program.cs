using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
        private Timer TotalLevelUpdateTimer = new Timer(60 * 60 * 1000);
        private readonly ulong _myServerId = 199189022894063627;
        private readonly ulong _testServerId = 430643719880835072;
        private static readonly ulong _myUserId = 198806112852508672;
        private static readonly string _testUser = "TheSweMasterX#5203";

        public static Dictionary<string, string> RunescapeAccounts = new Dictionary<string, string>()
        {
            { _testUser, "theswemaster" },
        };

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            //event subsciptions
            _client.Log += Log;
            _client.UserJoined += AnnouceUserJoined;

            TotalLevelUpdateTimer.Enabled = true;
            TotalLevelUpdateTimer.Elapsed += new ElapsedEventHandler(UpdateTotalLevelOnEveryHourEvent);

            await SetBotGameStatus();

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async void UpdateTotalLevelOnEveryHourEvent(object source, ElapsedEventArgs e)
        {
            var guild = _client.GetGuild(_testServerId);

            foreach (var keyPair in RunescapeAccounts)
            {
                var username = keyPair.Key.Split('#')[0];
                var discriminator = keyPair.Key.Split('#')[1];
                var user = _client.GetUser(username, discriminator);
                var guildUser = guild.GetUser(user.Id);
                var oldNickname = guildUser.Nickname ?? guildUser.Username;

                string newNickname = "";
                var res = oldNickname.Split('(');
                if (oldNickname.Split('(') != null || oldNickname.Split('(').Count() == 2)
                {
                    newNickname = oldNickname.Split('(')[0] + $"({await GetHighScoreData(keyPair.Value)}/2277)";
                }
                else
                {
                    newNickname = oldNickname + $" ({await GetHighScoreData(keyPair.Value)}/2277)";
                }
                await guildUser.ModifyAsync(x => x.Nickname = $"{newNickname}");
            }
        }

        private async Task<string> GetHighScoreData(string rsUsername)
        {
            var stringData = await _httpClient.GetStringAsync($"http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player={rsUsername}");
            stringData = stringData.Substring(0, 20);
            var dataArray = stringData.Split(',');
            var currentTotalLevel = dataArray[1];

            return currentTotalLevel;
        }

        private async Task SetBotGameStatus()
        {
            await _client.SetGameAsync("as Torbjörn | !!help");
        }

        private async Task AnnouceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            await channel.SendMessageAsync($"Welcome, {user.Mention}");
        }

        private Task Log(LogMessage arg)
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
