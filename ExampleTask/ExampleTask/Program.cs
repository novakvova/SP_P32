using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Привіт команда!");

//Це метод, який нічого не віртає
Task<int> task = new Task<int>(() =>
{
    Console.WriteLine("Привіт 1");
    Console.WriteLine($"Номер потоку {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);
    Console.WriteLine("Потік завершив свою роботу");
    return 1;
});

task.Start(); //теж саме, що Task.Run
//var result = task.Result; // Просто тормозить роботу потоку, поки не буде результату
//task.Wait(); //Працює аналогічно task.Result - тормозить поточний потік

int result = await task; //Очікує завершення задачі, томозить поточний потік, але
//Воно виходить із методу і запамятовую коди має повенутися назад

Console.WriteLine($"Основний потік папа {Thread.CurrentThread.ManagedThreadId}");