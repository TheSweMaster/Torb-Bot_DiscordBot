using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordBotTest
{
    public class Configuration
    {
        private const string AppSettingsFileName = "AppSettings.json";
        public static AppSettings GetAppSettings()
        {
            return JsonConvert.DeserializeObject<AppSettings>(SearchAndReadFile(AppSettingsFileName));
        }

        private static string SearchAndReadFile(string filename)
        {
            string text = "";
            string path = "";
#if DEBUG
            path = @"..\..\..\";
#else
            path = AppDomain.CurrentDomain.BaseDirectory;
#endif
            var filePaths = Directory.GetFiles(path, filename, SearchOption.AllDirectories);

            return text = (filePaths.Count() == 1)
                ? File.ReadAllText(filePaths.First())
                : throw new IOException($"Could not find the specified file '{filename}'");
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
