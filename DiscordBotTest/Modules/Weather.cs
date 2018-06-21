using APIXULib;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace DiscordBotTest.Modules
{
    public class Weather : ModuleBase<SocketCommandContext>
    {
        //Replace this with your own key from https://www.apixu.com/ 
        private readonly string key = Configuration.GetAppSettings().Keys.ApiuxKey;
        private readonly HttpClient _client = new HttpClient();

        [Command("weather")]
        public async Task WeatherByCity([Remainder]string city = "goeteborg")
        {
            var result = await GetWeatherData(city.ToLower());

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Current Weather at {result.location.name} - {result.location.country}")
                .WithDescription($"Condition: {result.current.condition.text} \nTemp: {result.current.temp_c}C " +
                $"\nHumidity: {result.current.humidity}% \nWind Direction: {result.current.wind_dir} \nWind Speed: {result.current.wind_kph}km/h")
                .WithFooter($"Updated: {result.current.last_updated}")
                .WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .WithImageUrl("http:" + result.current.condition.icon);

            await ReplyAsync("", false, builder.Build());
        }

        public async Task<WeatherModel> GetWeatherData(string city)
        {
            var url = "http://api.apixu.com/v1/current.json?key=" + key + "&q=" + city;

            var jsonResponds = await _client.GetStringAsync(url);

            var result = JsonConvert.DeserializeObject<WeatherModel>(jsonResponds);

            return result;
        }
    }
}
