﻿using System.Text;
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

var model = new NovaPostaRequest
{
    ApiKey = apiKey,
    ModelName = "Address",
    CalledMethod = "getSettlementAreas",
    MethodProperties = new() { Page = 1, Limit = 200 }
};

string json = JsonConvert.SerializeObject(model); //перетворює модел у json

string url = "https://api.novaposhta.ua/v2.0/json/ ";

HttpClient client = new ();

HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
HttpResponseMessage resp = await client.PostAsync(url, content);
if (resp.IsSuccessStatusCode)
{
    var respJson = await resp.Content.ReadAsStringAsync();

    if(respJson is not null)
    {
        var areasData = JsonConvert.DeserializeObject<NovaPoshtaResponse<Area>>(respJson);
        await dbContext.Areas.AddRangeAsync(areasData.Data);
        await dbContext.SaveChangesAsync();

        //if (areasData is not null)
        //{
        //    foreach (var area in areasData.Data)
        //    {
        //        Console.WriteLine($"Result {area.Ref}\t {area.Description}");
        //    }
        //}
    }

}
else
{
    Console.WriteLine("Помилка запиту");
}
