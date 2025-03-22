using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace WeatherMauiApp
{
    
    public class WeatherResponse
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("current")]
        public CurrentWeather Current { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temp_c")]
        public float TemperatureCelsius { get; set; }

        [JsonPropertyName("condition")]
        public WeatherCondition Condition { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("wind_kph")]
        public float WindSpeedKph { get; set; }
    }

    public class WeatherCondition
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    
    public class WeatherService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;

        public WeatherService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            try
            {
                string url = $"https://api.weatherapi.com/v1/current.json?key=a08b57d9b7ce4af38c3121348252103&q=London&aqi=no";
                

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var weatherData = await response.Content.ReadFromJsonAsync<WeatherResponse>();
                Trace.WriteLine(weatherData);
                return weatherData;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error fetching weather data: {ex.Message}");
                return null;
            }
        }
    }
}
