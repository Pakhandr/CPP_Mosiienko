using SQLite;
using maui_lab_1.ViewModels;
using System.Collections.ObjectModel;
using System.IO; 
using System.Threading.Tasks; 

namespace maui_lab_1.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            Init();
        }

        private async void Init()
        {
            if (_database != null)
                return;

            // Get the database path
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "locations.db3");

            // Create the connection
            _database = new SQLiteAsyncConnection(dbPath);

            // Create the table if it doesn't exist
            await _database.CreateTableAsync<Location>();
        }

        public async Task<int> SaveLocationAsync(Location location)
        {
            if (_database == null)
                Init();

            // Set timestamp to current time
            location.Timestamp = DateTime.Now;

            // Save to database
            return await _database.InsertAsync(location);
        }

        public async Task<ObservableCollection<Location>> GetLocationsAsync()
        {
            if (_database == null)
                Init();

            
            var locations = await _database.Table<Location>()
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();

            return new ObservableCollection<Location>(locations);
        }

        public async Task<int> DeleteLocationAsync(int id)
        {
            if (_database == null)
                Init();

            return await _database.DeleteAsync<Location>(id);
        }

        public async Task<int> ClearAllLocationsAsync()
        {
            if (_database == null)
                Init();

            return await _database.DeleteAllAsync<Location>();
        }
    }
}
