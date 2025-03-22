using Microsoft.Maui.Devices.Sensors;
using System;
using System.Threading.Tasks;
#if ANDROID
using maui_lab_1.Platforms.Android;
#endif

namespace maui_lab_1.Controls
{
    public class LocationEventArgs : EventArgs
    {
        public Location Location { get; }

        public LocationEventArgs(Location location)
        {
            Location = location;
        }
    }

    public class Location_lookup
    {
        public event EventHandler<LocationEventArgs> LocationUpdated = delegate { };

        public static string? LastLatitude { get; private set; }
        public static string? LastLongitude { get; private set; }
        public DateTimeOffset? LastTimestamp { get; private set; }

#if ANDROID
        private readonly AndroidLocationProvider _androidLocationProvider = new();
#endif

        public async Task<LocationData> GetCurrentLocationAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        return new LocationData
                        {
                            Success = false,
                            ErrorMessage = "Location permission was denied"
                        };
                    }
                }

                Location location = await Geolocation.GetLastKnownLocationAsync();
#if ANDROID
                Android.Locations.Location androidLocation = null;
                androidLocation = await _androidLocationProvider.GetLocationAsync();
                if (androidLocation != null)
                {
                    location = new Location
                    {
                        Latitude = androidLocation.Latitude,
                        Longitude = androidLocation.Longitude
                    };
                }
                return new LocationData
                {
                    Success = true,
                    Latitude = androidLocation?.Latitude,
                    Longitude = androidLocation?.Longitude
                };
#endif

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium
                    });
                }

                if (location != null)
                {
                    LastLatitude = location.Latitude.ToString();
                    LastLongitude = location.Longitude.ToString();

                    LocationUpdated(this, new LocationEventArgs(location));

                    return new LocationData
                    {
                        Success = true,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };
                }
                else
                {
                    return new LocationData
                    {
                        Success = false,
                        ErrorMessage = "Unable to get location"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LocationData
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}

public class LocationData
{
    public bool Success { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string ErrorMessage { get; set; }
}

