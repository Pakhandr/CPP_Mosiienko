using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Location = Android.Locations.Location;

namespace maui_lab_1.Platforms.Android
{
    public class AndroidLocationProvider : Java.Lang.Object, ILocationListener
    {
        private readonly LocationManager _locationManager;
        private readonly string _provider;
        private TaskCompletionSource<Location> _locationTcs;

        public AndroidLocationProvider()
        {
            _locationManager = (LocationManager)Platform.CurrentActivity.GetSystemService(Context.LocationService);
            _provider = LocationManager.GpsProvider;
        }

        public async Task<Location> GetLocationAsync()
        {
            if (_locationManager.IsProviderEnabled(_provider))
            {
                _locationTcs = new TaskCompletionSource<Location>();

                try
                {
                    _locationManager.RequestSingleUpdate(_provider, this, null);
                    return await _locationTcs.Task;
                }
                catch (Exception ex)
                {
                    Log.Error("AndroidLocationProvider", $"Error getting location: {ex.Message}");
                    return null;
                }
            }
            else
            {
                Log.Warn("AndroidLocationProvider", "GPS Provider is disabled.");
                return null;
            }
        }

        public void OnLocationChanged(Location location)
        {
            _locationTcs?.TrySetResult(location);
        }
   
        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }
    }
}