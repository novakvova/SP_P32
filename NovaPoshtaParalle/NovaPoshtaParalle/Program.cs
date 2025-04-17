using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NovaPoshtaParalle;
using NovaPoshtaParalle.Entities;

Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;

string apiKey = "27e98316d8976226c4d4185fa2650f10";

//Хочу отримтаи список областей

MyApplicationContext dbContext = new MyApplicationContext();
dbContext.Database.Migrate();

string url = "https://api.novaposhta.ua/v2.0/json/ ";

HttpClient client = new ();

if(!dbContext.Areas.Any())
{
    var model = new NovaPostaRequest
    {
        ApiKey = apiKey,
        ModelName = "Address",
        CalledMethod = "getSettlementAreas",
        MethodProperties = new() { Page = "1" }
    };

    string json = JsonConvert.SerializeObject(model); //перетворює модел у json

    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
    HttpResponseMessage resp = await client.PostAsync(url, content);
    if (resp.IsSuccessStatusCode)
    {
        var respJson = await resp.Content.ReadAsStringAsync();

        if (respJson is not null)
        {
            var areasData = JsonConvert.DeserializeObject<NovaPoshtaResponseArea<Area>>(respJson);
            await dbContext.Areas.AddRangeAsync(areasData.Data);
            await dbContext.SaveChangesAsync();
        }
    }
    else
    {
        Console.WriteLine("Помилка запиту");
    }
}

// getting cities asyncronously

if (!dbContext.Cities.Any())
{
    int pages = Environment.ProcessorCount * 2;
    int lenght;
    var model = new NovaPostaRequest()
    {
        ApiKey = apiKey,
        ModelName = "AddressGeneral",
        CalledMethod = "getCities",
        MethodProperties = new NovaPoshtaMethodProperties { Page = "1", Limit = 1 }
    };
    var json = JsonConvert.SerializeObject(model);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PostAsync(url, content);
    if (response.IsSuccessStatusCode)
    {
        string responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine("DATA {0}", responseString);
        NovaPoshtaResponse<City> result = JsonConvert.DeserializeObject<NovaPoshtaResponse<City>>(responseString);
        Console.WriteLine(result.Info.TotalCount);
        Console.WriteLine("\n\n");
        lenght = Convert.ToInt32(Math.Ceiling((double)result.Info.TotalCount / pages));
        Console.WriteLine(pages);
        List<City> listCities = new List<City>();
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        //for (int i = 1; i < pages+1; i++)
        //{
        //    var localModel = new NovaPostaRequest()
        //    {
        //        ApiKey = apiKey,
        //        ModelName = "AddressGeneral",
        //        CalledMethod = "getCities",
        //        MethodProperties = new() { Page = i.ToString(), Limit = lenght }
        //    };

        //    var localJson = JsonConvert.SerializeObject(localModel);
        //    var localContent = new StringContent(localJson, Encoding.UTF8, "application/json");

        //    var localResponse = await client.PostAsync(url, localContent);
        //    var localResponseString = await localResponse.Content.ReadAsStringAsync();
        //    var localResult = JsonConvert.DeserializeObject<NovaPoshtaResponse<City>>(localResponseString);

        //    if (localResult?.Data != null && localResult.Data.Length > 0)
        //    {
        //        Console.WriteLine("Cities count {0}", localResult.Data.Length);
        //        listCities.AddRange(localResult.Data);
        //    }
        //}

        await Parallel.ForAsync(1, pages + 1, async (i, _) =>
        {
            //await Task.Delay(1000);

            var localModel = new NovaPostaRequest()
            {
                ApiKey = apiKey,
                ModelName = "AddressGeneral",
                CalledMethod = "getCities",
                MethodProperties = new() { Page = i.ToString(), Limit = lenght }
            };

            var localJson = JsonConvert.SerializeObject(localModel);
            var localContent = new StringContent(localJson, Encoding.UTF8, "application/json");

            var localResponse = await client.PostAsync(url, localContent);
            var localResponseString = await localResponse.Content.ReadAsStringAsync();
            var localResult = JsonConvert.DeserializeObject<NovaPoshtaResponse<City>>(localResponseString);

            if (localResult?.Data != null && localResult.Data.Length > 0)
            {
                Console.WriteLine("Cities count {0}", localResult.Data.Length);
                listCities.AddRange(localResult.Data);
            }
        });

        Console.WriteLine("Count Cities {0}", listCities.Count);

        foreach (var city in listCities)
        {

        }


        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("RunTime " + elapsedTime);
    }
}



