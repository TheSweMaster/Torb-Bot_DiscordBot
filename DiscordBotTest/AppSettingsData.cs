using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotTest
{
    public static class AppSettingsData
    {
        public static AppSettings Get()
        {
            return new AppSettings()
            {
                Keys = new Key()
                {
                    BotToken = "<YourBotToken>",
                    ApiuxKey = "<YouAPIKey>"
                }
            };
        }
    }
}
