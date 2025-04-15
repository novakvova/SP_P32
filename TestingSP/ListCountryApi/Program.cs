// See https://aka.ms/new-console-template for more information
//using System.Text.Json;
using Newtonsoft.Json;

await FetchCountriesAndCitiesAsync();


static async Task FetchCountriesAndCitiesAsync()
{
    var url = "https://countriesnow.space/api/v0.1/countries";

    using var client = new HttpClient();

    try
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        //Console.WriteLine("Data {0}", responseContent);
        var result = JsonConvert.DeserializeObject<CountryResponse>(responseContent);
        
        //var result = System.Text.Json.JsonSerializer.Deserialize<CountryResponse>(responseContent);

        if (result != null && result.Data != null)
        {
            var countryMap = new Dictionary<string, List<string>>();

            foreach (var entry in result.Data)
            {
                countryMap[entry.Country] = entry.Cities;
            }

            // Вивід на консоль
            foreach (var kvp in countryMap)
            {
                Console.WriteLine($"Країна: {kvp.Key}");
                //foreach (var city in kvp.Value)
                //{
                //    Console.WriteLine($" - {city}");
                //}
                Console.WriteLine();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка при завантаженні країн: {ex.Message}");
    }
}

// Класи для десеріалізації JSON
public class CountryResponse
{
    public bool Error { get; set; }
    public string Msg { get; set; }
    public List<CountryEntry> Data { get; set; }
}

public class CountryEntry
{
    public string Country { get; set; }
    public List<string> Cities { get; set; }
}