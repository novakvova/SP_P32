using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NovaPoshtaParalle.Entities;

namespace NovaPoshtaParalle
{
    public class NovaPostaService
    {
        private MyApplicationContext dbContext;
        string url = "https://api.novaposhta.ua/v2.0/json/ ";
        string apiKey = "27e98316d8976226c4d4185fa2650f10";

        HttpClient client;
        public NovaPostaService() 
        {
            dbContext = new MyApplicationContext();
            dbContext.Database.Migrate();
            client = new();
        }

        public async Task SeedAreas()
        {
            if (!dbContext.Areas.Any())
            {
                var model = new NovaPostaRequest
                {
                    ApiKey = apiKey,
                    ModelName = "Address",
                    CalledMethod = "getAreas",
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
        }

        public async Task SeedCities()
        {
            if (!dbContext.Cities.Any())
            {
                int pages = Environment.ProcessorCount * 2;
                int lenght;
                var model = new NovaPostaRequest()
                {
                    ApiKey = apiKey,
                    ModelName = "Address",
                    CalledMethod = "getCities",
                    MethodProperties = new NovaPoshtaMethodProperties { Page = "1", Limit = 1 }
                };
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    NovaPoshtaResponseCity<City> result = JsonConvert.DeserializeObject<NovaPoshtaResponseCity<City>>(responseString);
                    lenght = Convert.ToInt32(Math.Ceiling((double)result.Info.TotalCount / pages));

                    List<City> listCities = new List<City>();

                    await Parallel.ForAsync(1, pages + 1, async (i, _) =>
                    {
                        await Task.Delay(700);

                        var localModel = new NovaPostaRequest()
                        {
                            ApiKey = apiKey,
                            ModelName = "Address",
                            CalledMethod = "getCities",
                            MethodProperties = new() { Page = i.ToString(), Limit = lenght }
                        };

                        var localJson = JsonConvert.SerializeObject(localModel);
                        var localContent = new StringContent(localJson, Encoding.UTF8, "application/json");

                        var localResponse = await client.PostAsync(url, localContent);
                        var localResponseString = await localResponse.Content.ReadAsStringAsync();
                        var localResult = JsonConvert.DeserializeObject<NovaPoshtaResponseCity<City>>(localResponseString);

                        if (localResult?.Data != null && localResult.Data.Length > 0)
                        {
                            listCities.AddRange(localResult.Data);
                        }
                    });

                    Console.WriteLine("Count Cities {0}", listCities.Count);

                    await dbContext.AddRangeAsync(listCities);
                    await dbContext.SaveChangesAsync();



                }
            }
        }

        public async Task SeedDepartments()
        {
            if (!dbContext.Departments.Any())
            {
                int pages = Environment.ProcessorCount * 2;
                int lenght;
                var model = new NovaPostaRequest()
                {
                    ApiKey = apiKey,
                    ModelName = "Address",
                    CalledMethod = "getWarehouses",
                    MethodProperties = new NovaPoshtaMethodProperties { Page = "1", Limit = 1 }
                };
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    DepartmentResponse result = JsonConvert.DeserializeObject<DepartmentResponse>(responseString);

                    lenght = Convert.ToInt32(Math.Ceiling((double)result.Info.TotalCount / pages));
                    //lenght = 10
                    List<DepartmentEntity> listDepartments = new List<DepartmentEntity>();

                    await Parallel.ForAsync(1, pages + 1, async (i, _) =>
                    {
                        await Task.Delay(700);
                        var localModel = new NovaPostaRequest()
                        {
                            ApiKey = apiKey,
                            ModelName = "Address",
                            CalledMethod = "getWarehouses",
                            MethodProperties = new() { Page = i.ToString(), Limit = lenght }
                        };

                        var localJson = JsonConvert.SerializeObject(localModel);
                        var localContent = new StringContent(localJson, Encoding.UTF8, "application/json");

                        var localResponse = await client.PostAsync(url, localContent);
                        var localResponseString = await localResponse.Content.ReadAsStringAsync();
                        var localResult = JsonConvert.DeserializeObject<DepartmentResponse>(localResponseString);

                        if (localResult?.Data != null && localResult.Data.Count > 0)
                        {
                            listDepartments.AddRange(localResult.Data);
                        }
                    });

                    Console.WriteLine("Count Department {0}", listDepartments.Count);

                    await dbContext.AddRangeAsync(listDepartments);
                    await dbContext.SaveChangesAsync();

                }
            }
        }
    }
}
