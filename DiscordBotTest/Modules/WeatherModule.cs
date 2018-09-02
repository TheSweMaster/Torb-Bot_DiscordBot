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
using DiscordBotTest.Models.APIXULib;

namespace DiscordBotTest.Modules
{
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        //Replace this with your own key from https://www.apixu.com/ 
        private readonly string key = Configuration.GetAppSettings().Keys.ApiuxKey;
        private readonly HttpClient _client = new HttpClient();

        [Command("weather")]
        public async Task WeatherByCityCommand([Remainder]string city = "goeteborg")
        {
            var result = await GetWeatherData(city.ToLower());

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Current Weather at {result.Location.Name} - {result.Location.Country}")
                .WithDescription($"Condition: {result.Current.Condition.Text} \nTemp: {result.Current.Temp_c}C " +
                $"\nHumidity: {result.Current.Humidity}% \nWind Direction: {result.Current.Wind_dir} \nWind Speed: {result.Current.Wind_kph}km/h")
                .WithFooter($"Updated: {result.Current.Last_updated}")
                .WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .WithImageUrl("http:" + result.Current.Condition.Icon);

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
