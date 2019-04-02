using DiscordBotTest.Models;
using Newtonsoft.Json;

namespace DiscordBotTest.Converters
{
    public static class RSAccountDataConverter
    {
        public static string ToJson(RSAccountData accountData)
        {
            return JsonConvert.SerializeObject(accountData);
        }

        public static RSAccountData ToRSAccountData(string json)
        {
            return JsonConvert.DeserializeObject<RSAccountData>(json);
        }
    }
}
