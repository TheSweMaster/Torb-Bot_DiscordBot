using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiscordBotTest.Models.WeatherstackLib
{
    public class Request
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("query")]
        public string Query { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }

    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lon")]
        public double Lon { get; set; }
        [JsonProperty("timezone_id")]
        public string Timezone_id { get; set; }
        [JsonProperty("localtime_epoch")]
        public int Localtime_epoch { get; set; }
        [JsonProperty("localtime")]
        public string Localtime { get; set; }
        [JsonProperty("utc_offset")]
        public double Utc_offset { get; set; }
    }

    public class Current
    {
        [JsonProperty("observation_time")]
        public string Observation_time { get; set; }
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
        [JsonProperty("weather_code")]
        public string Weather_code { get; set; }
        [JsonProperty("weather_icons")]
        public List<string> Weather_icons { get; set; }
        [JsonProperty("weather_descriptions")]
        public List<string> Weather_descriptions { get; set; }
        [JsonProperty("wind_speed")]
        public int Wind_speed { get; set; }
        [JsonProperty("wind_degree")]
        public int Wind_degree { get; set; }
        [JsonProperty("wind_dir")]
        public string Wind_dir { get; set; }
        [JsonProperty("pressure")]
        public int Pressure { get; set; }
        [JsonProperty("precip")]
        public double Precip { get; set; }
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
        [JsonProperty("cloudcover")]
        public int Cloudcover { get; set; }
        [JsonProperty("feelslike")]
        public int Feelslike { get; set; }
        [JsonProperty("uv_index")]
        public int Uv_index { get; set; }
        [JsonProperty("visibility")]
        public int Visibility { get; set; }
    }

    public class WeatherModel
    {
        [JsonProperty("request")]
        public Request Request { get; set; }
        [JsonProperty("location")]
        public Location Location { get; set; }
        [JsonProperty("current")]
        public Current Current { get; set; }
    }
}
