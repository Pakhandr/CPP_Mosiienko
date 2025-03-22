using WeatherMauiApp;
namespace maui_lab_1

{
    public partial class MainPage : ContentPage
    {
        private WeatherService _weatherService;

        public MainPage()
        {
            InitializeComponent();
            _weatherService = new WeatherService("a08b57d9b7ce4af38c3121348252103");

            
            LoadWeatherAsync();
        }

        private async void LoadWeatherAsync()
        {
            LoadingIndicator.IsRunning = true;

            var weatherData = await _weatherService.GetWeatherAsync("London");

            if (weatherData != null)
            {
                LocationLabel.Text = $"{weatherData.Location.Name}, {weatherData.Location.Country}";
                TemperatureLabel.Text = $"{weatherData.Current.TemperatureCelsius}°C";
                ConditionLabel.Text = weatherData.Current.Condition.Text;
                HumidityLabel.Text = $"Humidity: {weatherData.Current.Humidity}%";
                WindLabel.Text = $"Wind: {weatherData.Current.WindSpeedKph} km/h";
            }
            else
            {
                LocationLabel.Text = "Unable to fetch weather data";
            }

            LoadingIndicator.IsRunning = false;
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadWeatherAsync();
        }
    }
}

