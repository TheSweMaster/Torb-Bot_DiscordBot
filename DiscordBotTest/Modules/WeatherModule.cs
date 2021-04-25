using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using DiscordBotTest.Models.WeatherstackLib;
using System.Linq;

namespace DiscordBotTest.Modules
{
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        // Replace this with your own key from https://weatherstack.com 
        private readonly string key = Configuration.GetAppSettings().Keys.WeatherstackKey;
        private readonly HttpClient _client = new HttpClient();

        [Command("weather")]
        public async Task WeatherByCityCommand([Remainder]string city = "goeteborg")
        {
            var result = await GetCurrentWeather(city.ToLower());

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Current Weather at {result.Location.Name} - {result.Location.Country}")
                .WithDescription($"Condition: {string.Join(", ", result.Current.Weather_descriptions)} \nTemperature: {result.Current.Temperature} °C " +
                $"\nHumidity: {result.Current.Humidity}% \nPrecipitation level: {result.Current.Precip} mm" +
                $"\nWind Direction: {result.Current.Wind_dir} \nWind Speed: {result.Current.Wind_speed} km/h")
                .WithFooter($"Local time: {result.Location.Localtime}")
                .WithColor(Color.Blue)
                .WithImageUrl(result.Current.Weather_icons.FirstOrDefault());

            await ReplyAsync(embed: builder.Build());
        }

        public async Task<WeatherModel> GetCurrentWeather(string city)
        {
            var url = "http://api.weatherstack.com/current?access_key=" + key + "&query=" + city;

            var jsonResponds = await _client.GetStringAsync(url);

            var result = JsonConvert.DeserializeObject<WeatherModel>(jsonResponds);

            return result;
        }
    }
}
