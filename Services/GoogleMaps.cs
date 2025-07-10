using Newtonsoft.Json;
using WeatherApp.Domain;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    static internal class GoogleMaps
    {
        public static async Task<LatLng> GetLatLong(string API_KEY, string location, HttpClient client)
        {
            var google_request = $"https://maps.googleapis.com/maps/api/geocode/json?address={location}&key={API_KEY}";
            string response = await client.GetStringAsync(google_request);
            GoogleMapAPI res = JsonConvert.DeserializeObject<GoogleMapAPI>(response);
            if (res.results.Count == 0)
            {
                throw new Exception("Invalid Location Given");
            }
            double lat = res.results[0].geometry.location.lat;
            double lng = res.results[0].geometry.location.lng;

            return new LatLng(lat, lng);
        }
    }
}
