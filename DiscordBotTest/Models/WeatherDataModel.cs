using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiscordBotTest.Models.APIXULib
{
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
        [JsonProperty("tz_id")]
        public string Tz_id { get; set; }
        [JsonProperty("localtime_epoch")]
        public int Localtime_epoch { get; set; }
        [JsonProperty("localtime")]
        public string Localtime { get; set; }
    }

    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
    }

    public class Current
    {
        [JsonProperty("last_updated_epoch")]
        public int Last_updated_epoch { get; set; }
        [JsonProperty("last_updated")]
        public string Last_updated { get; set; }
        [JsonProperty("temp_c")]
        public double Temp_c { get; set; }
        [JsonProperty("temp_f")]
        public double Temp_f { get; set; }
        [JsonProperty("condition")]
        public Condition Condition { get; set; }
        [JsonProperty("wind_mph")]
        public double Wind_mph { get; set; }
        [JsonProperty("wind_kph")]
        public double Wind_kph { get; set; }
        [JsonProperty("wind_degree")]
        public int Wind_degree { get; set; }
        [JsonProperty("wind_dir")]
        public string Wind_dir { get; set; }
        [JsonProperty("pressure_mb")]
        public double Pressure_mb { get; set; }
        [JsonProperty("pressure_in")]
        public double Pressure_in { get; set; }
        [JsonProperty("precip_mm")]
        public double Precip_mm { get; set; }
        [JsonProperty("precip_in")]
        public double Precip_in { get; set; }
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
        [JsonProperty("cloud")]
        public int Cloud { get; set; }
        [JsonProperty("feelslike_c")]
        public double Feelslike_c { get; set; }
        [JsonProperty("feelslike_f")]
        public double Feelslike_f { get; set; }
    }

   

    public class Day
    {
        public double maxtemp_c { get; set; }
        public double maxtemp_f { get; set; }
        public double mintemp_c { get; set; }
        public double mintemp_f { get; set; }
        public double avgtemp_c { get; set; }
        public double avgtemp_f { get; set; }
        public double maxwind_mph { get; set; }
        public double maxwind_kph { get; set; }
        public double totalprecip_mm { get; set; }
        public double totalprecip_in { get; set; }
        public Condition condition { get; set; }
    }

    public class Astro
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string moonrise { get; set; }
        public string moonset { get; set; }
    }

   

    public class Hour
    {
        public int time_epoch { get; set; }
        public string time { get; set; }
        public double temp_c { get; set; }
        public double temp_f { get; set; }
        public Condition condition { get; set; }
        public double wind_mph { get; set; }
        public double wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public double pressure_mb { get; set; }
        public double pressure_in { get; set; }
        public double precip_mm { get; set; }
        public double precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public double feelslike_c { get; set; }
        public double feelslike_f { get; set; }
        public double windchill_c { get; set; }
        public double windchill_f { get; set; }
        public double heatindex_c { get; set; }
        public double heatindex_f { get; set; }
        public double dewpoint_c { get; set; }
        public double dewpoint_f { get; set; }
        public int will_it_rain { get; set; }
        public int will_it_snow { get; set; }
    }

    public class Forecastday
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("date_epoch")]
        public int Date_epoch { get; set; }
        [JsonProperty("day")]
        public Day Day { get; set; }
        [JsonProperty("astro")]
        public Astro Astro { get; set; }
        [JsonProperty("hour")]
        public List<Hour> Hour { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<Forecastday> Forecastday { get; set; }
    }

    public class WeatherModel
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
        [JsonProperty("current")]
        public Current Current { get; set; }
        [JsonProperty("forecast")]
        public Forecast Forecast { get; set; }
    }
}
