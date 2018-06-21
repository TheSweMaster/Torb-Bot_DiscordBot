using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordBotTest
{
    public class Configuration
    {
        private static readonly string _appSettingsPath = @"..\..\..\AppSettings.json";
        public static AppSettings GetAppSettings()
        {
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(_appSettingsPath));
        }
    }

    public class AppSettings
    {
        [JsonProperty("Keys")]
        public Key Keys { get; protected set; }
    }

    public class Key
    {
        [JsonProperty("botToken")]
        public string BotToken { get; protected set; }

        [JsonProperty("apiuxKey")]
        public string ApiuxKey { get; protected set; }
    }
}
