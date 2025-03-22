using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Devices.Sensors;
using maui_lab_1.Controls;
using maui_lab_1.Services;
using System.Collections.ObjectModel;


#if ANDROID
using Android.Content;
using Android.Provider;
#endif

namespace maui_lab_1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private string _currentDateTime;
        private double _latitude;
        private double _longitude;
        private ObservableCollection<Location> _savedLocations;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string CurrentDateTime
        {
            get => _currentDateTime;
            set => SetProperty(ref _currentDateTime, value);
        }

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public ObservableCollection<Location> SavedLocations
        {
            get => _savedLocations;
            set => SetProperty(ref _savedLocations, value);
        }

        public ICommand UpdateTimeCommand { get; }
        public ICommand GetLocationCommand { get; }
        public ICommand SaveLocationCommand { get; }
        public ICommand DeleteLocationCommand { get; }
        public ICommand ClearLocationsCommand { get; }

        public MainViewModel()
        {
            _databaseService = new DatabaseService();
            SavedLocations = new ObservableCollection<Location>();

            // Initialize with current time
            CurrentDateTime = DateTime.Now.ToString("F");

            // Define commands
            UpdateTimeCommand = new Command(UpdateTime);
            GetLocationCommand = new Command(async () => await GetCurrentLocation());
            SaveLocationCommand = new Command(async () => await SaveLocation());
            DeleteLocationCommand = new Command<int>(async (id) => await DeleteLocation(id));
            ClearLocationsCommand = new Command(async () => await ClearLocations());

            // Load saved locations when the ViewModel is created
            LoadSavedLocations();
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateTime()
        {
            CurrentDateTime = DateTime.Now.ToString("F");
        }

        private async Task GetCurrentLocation()
        {
            try
            {
                // Check if location is enabled
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                Location location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., location services disabled)
                await Application.Current.MainPage.DisplayAlert("Error", $"Unable to get location: {ex.Message}", "OK");
            }
        }

        private async Task SaveLocation()
        {
            if (Latitude != 0 && Longitude != 0)
            {
                var location = new Location
                {
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Timestamp = DateTime.Now
                };

                await _databaseService.SaveLocationAsync(location);
                await LoadSavedLocations();

                await Application.Current.MainPage.DisplayAlert("Success", "Location saved successfully", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please get current location first", "OK");
            }
        }

        private async Task LoadSavedLocations()
        {
            SavedLocations = await _databaseService.GetLocationsAsync();
        }

        private async Task DeleteLocation(int id)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this location?", "Yes", "No");

            if (confirm)
            {
                await _databaseService.DeleteLocationAsync(id);
                await LoadSavedLocations();
            }
        }

        private async Task ClearLocations()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete all saved locations?", "Yes", "No");

            if (confirm)
            {
                await _databaseService.ClearAllLocationsAsync();
                await LoadSavedLocations();
            }
        }
    }
}

#if ANDROID
public class PlatformSpecific
        {
            public void AndroidSpecificFeature()
            {
                var context = Platform.CurrentActivity;
                if (context != null)
                {
                    var uri = Location_lookup.LastLatitude != null ? Android.Net.Uri.Parse(Location_lookup.LastLatitude) : null;
                    if (uri != null)
                    {
                        var values = new ContentValues[0]; 
                        var latitude = context.ContentResolver.BulkInsert(uri, values);
                    }
                }
            }
        }
#endif
