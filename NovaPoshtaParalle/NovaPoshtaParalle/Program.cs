using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NovaPoshtaParalle;
using NovaPoshtaParalle.Entities;

Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;

NovaPostaService novaPostaService = new NovaPostaService();

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

await novaPostaService.SeedAreas();

stopWatch.Stop();
// Get the elapsed time as a TimeSpan value.
TimeSpan ts = stopWatch.Elapsed;

// Format and display the TimeSpan value.
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("Seed Areas time " + elapsedTime);

stopWatch.Reset();
stopWatch.Start();

await novaPostaService.SeedCities();

stopWatch.Stop();
// Get the elapsed time as a TimeSpan value.
ts = stopWatch.Elapsed;

// Format and display the TimeSpan value.
elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("Seed Cities time " + elapsedTime);


stopWatch.Reset();

stopWatch.Start();

await novaPostaService.SeedDepartments();

stopWatch.Stop();
// Get the elapsed time as a TimeSpan value.
ts = stopWatch.Elapsed;

// Format and display the TimeSpan value.
elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine("Seed Departments time " + elapsedTime);





