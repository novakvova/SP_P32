using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

string apiKey = "4145bc6912b2487aad953945251104";
string city = "Elbasan";
int days = 14;

var tasks = new List<Task>();

var client = new HttpClient();

for (int i = 1; i <= days; i++)
{
    tasks.Add(Task.Run(async () =>
    {
        string url = $"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days={i}&aqi=no&alerts=no";

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(content);

            var forecast = weatherData?.Forecast?.Forecastday?.LastOrDefault();
            if (forecast != null)
            {
                Console.WriteLine($"Дата: {forecast.Date}");
                Console.WriteLine($"  Температура: {forecast.Day.AvgtempC} °C");
                Console.WriteLine($"  Стан: {forecast.Day.Condition.Text}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }));
}

await Task.WhenAll(tasks);


// Класи для JSON-десеріалізації

public class WeatherResponse
{
    public Forecast Forecast { get; set; }
}

public class Forecast
{
    public List<Forecastday> Forecastday { get; set; }
}

public class Forecastday
{
    public string Date { get; set; }
    public Day Day { get; set; }
}

public class Day
{
    public double AvgtempC { get; set; }
    public Condition Condition { get; set; }
}

public class Condition
{
    public string Text { get; set; }
}