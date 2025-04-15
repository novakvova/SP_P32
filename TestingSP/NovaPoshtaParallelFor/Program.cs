// See https://aka.ms/new-console-template for more information
using NovaPoshtaParallelFor;
using NovaPoshtaParallelFor.Entity;

Console.WriteLine("Hello, World!");

NewPostService newPostService = new NewPostService();

Console.WriteLine("Read Areas ...");

var areasData = await newPostService.GetAreasDataAsync();

Console.WriteLine($"Count Areas: {areasData.Count()}");

Console.WriteLine("Read regions ...");

var regionsData = await newPostService.GetRegionsDataAsync(areasData.Select(x => x.Ref));
Console.WriteLine($"Count Regions: {regionsData.Count()}");

Console.WriteLine("Read settlements ...");
var settlementsData = await newPostService.GetSettlementsDataAsync(regionsData);

Console.WriteLine($"Count settlements: {settlementsData.Count()}");
