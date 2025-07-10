using Newtonsoft.Json;
using WeatherApp.Domain;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    internal class OpenWeatherMap
    {
        private static string excludes = "minutely,hourly,daily,alerts";
        public static async Task<Temperature> GetTemps(double lat, double lng, string API_KEY, HttpClient client)
        {
            var WEATHER_API_URL = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lng}&exclude={excludes}&appid={API_KEY}&units=metric";
            string response1 = await client.GetStringAsync(WEATHER_API_URL);
            WeatherResponse res1 = JsonConvert.DeserializeObject<WeatherResponse>(response1);
            return new Temperature(res1.current.temp, res1.current.feels_like);
        }
    }
}
