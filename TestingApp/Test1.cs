using SQLite;


namespace TestForApp
{
    [TestClass]
    public sealed class Test1
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [TestMethod]
        public async Task TestMethod1() //Test for connecting to the API and getting data
        {
            // Arrange
            string url = $"https://api.weatherapi.com/v1/current.json?key=a08b57d9b7ce4af38c3121348252103&q=London&aqi=no";
            // Act
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task TestMethod2() // test for checking the data in db
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "locations.db3");

            
            FileInfo batadase = new FileInfo(dbPath);
            Assert.IsTrue(batadase.Exists);
            Assert.IsTrue(batadase.Length > 0);
        }
    }
}
