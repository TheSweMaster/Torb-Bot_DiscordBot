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
                    BotToken = "your_key",
                    WeatherstackKey = "your_key"
                }
            };
        }
    }
}
