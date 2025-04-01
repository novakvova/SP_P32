using System.Diagnostics;
using System.Text;

namespace ThreadAndDatabase
{
    internal class Program
    {
        private static CancellationTokenSource ctSource = new CancellationTokenSource();
        private static CancellationToken token = ctSource.Token;
        private static bool _pause = false;
        private static object _lock = new object();
        private static BananService bananService = new BananService();
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //Обчислюємо час роботи програми

            BananService._mre.Set();

            bananService.DataInserted += (count) =>
            {
                Console.WriteLine($"Total records inserted so far: {count}");
            };

            Console.Write("Enter number of records to insert: ");
            if (!int.TryParse(Console.ReadLine(), out int numberOfRecords) || numberOfRecords <= 0)
            {
                Console.WriteLine("Invalid input. Using default value: 2");
                numberOfRecords = 2;
            }

            Thread insertThread = new Thread(() => bananService.InsertBanansThread(token, ref _pause, _lock, numberOfRecords));
            insertThread.Start();

            while (true)
            {
                Console.WriteLine("Press 'p' to pause, 'r' to resume, 'q' to quit.");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.P)
                {
                    lock (_lock) {
                        BananService._mre.Reset(); //Залочити потік
                        _pause = true; 
                    }
                    Console.WriteLine("Paused...");
                }
                else if (key == ConsoleKey.R)
                {
                    lock (_lock) {
                        BananService._mre.Set();
                        _pause = false; 
                    }
                    Console.WriteLine("Resumed...");
                }
                else if (key == ConsoleKey.Q)
                {
                    ctSource.Cancel();
                    break;
                }
            }

            stopwatch.Stop();
            //TimeSpan - змінна, яка може зберігаати, сек, мілісенди, хв, год, дні
            TimeSpan ts = stopwatch.Elapsed; //Оперує тіками - одиниці часу
            Console.WriteLine($"Run time {ts}");
        }

        private static void DataBaseManager_GetConnectionEvent(ThreadAppContext threadAppContext)
        {
            Console.WriteLine("Зєднання з БД успішно кількість бананів {0}", threadAppContext.Banans.Count());
        }
    }
}
