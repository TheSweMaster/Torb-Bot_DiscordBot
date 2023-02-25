using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace DiscordBotTest
{
    public class Configuration
    {
        private const string AppSettingsFileName = "AppSettings.json";
        public static AppSettings GetAppSettings()
        {
#if DEBUG
            return JsonConvert.DeserializeObject<AppSettings>(SearchAndReadFile(AppSettingsFileName));
#else
            return AppSettingsData.Get();
#endif
        }

        private static string SearchAndReadFile(string filename)
        {
            string path = @"..\..\..\";

            var filePaths = Directory.GetFiles(path, filename, SearchOption.AllDirectories);

            return (filePaths.Length == 1)
                ? File.ReadAllText(filePaths.First())
                : throw new IOException($"Could not find the specified file '{filename}'");
        }
    }

    public class AppSettings
    {
        [JsonProperty("Keys")]
        public Key Keys { get; set; }
    }

    public class Key
    {
        [JsonProperty("botToken")]
        public string BotToken { get; set; }

        [JsonProperty("weatherstackKey")]
        public string WeatherstackKey { get; set; }
    }
}
