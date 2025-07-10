using System.Text;
using WeatherApp.Domain;
using WeatherApp.Services;

string? GOOGLE_API_KEY = Environment.GetEnvironmentVariable("GOOGLEMAPS_APIKEY");
string? WEATHER_API_KEY = Environment.GetEnvironmentVariable("OPENWEATHER_APIKEY");
if (GOOGLE_API_KEY == null || WEATHER_API_KEY == null)
{
    Console.WriteLine("Invalid API Keys in Environment Variables");
    Console.WriteLine("Press any key to close...");
    Console.ReadKey(true);
    Environment.Exit(1);
}

HttpClient client = new HttpClient();
string location = " ";
while (true)
{
    Console.Clear();
    location = PromptForLocation();
    try
    {
        // API Calls first to Google Maps for Lat Long, then OpenWeather for temperature data.
        LatLng latLng = await GoogleMaps.GetLatLong(GOOGLE_API_KEY, location, client);
        Temperature temps = await OpenWeatherMap.GetTemps(latLng.Lat, latLng.Lng, WEATHER_API_KEY, client);

        Console.WriteLine($"In {ToTitleCase(location)} it is currently {temps.Actual:F1}°C, and feels like {temps.FeelsLike:F1}°C.");
        Console.Write("Press 'Q' to exit, or any other key to search again.");

        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.Q) break;
    } catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Console.WriteLine("Press any key to try again...");
        Console.ReadKey(true);
        continue;
    }
}

// Helper Functions
string PromptForLocation()
{
    string? location;
    do
    {
        Console.Write("Please Enter a location: ");
        location = Console.ReadLine();
    } while (location == null);
    return location;
}

string ToTitleCase(string location)
{
    string[] words = location.Split(' ');
    string[] output = new string[words.Count()];
    for (int i = 0; i < words.Count(); i++)
    {
        string word = words[i];
        string Titled = char.ToUpper(word[0]) + word.Substring(1).ToLower();
        output[i] = Titled;
    }
    StringBuilder sb = new StringBuilder();
    foreach(string word in output)
    {
        sb.Append(word).Append(" ");
    }
    sb.Length--; // Removes the last character 

    return sb.ToString();
}