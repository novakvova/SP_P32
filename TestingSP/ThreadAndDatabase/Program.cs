using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ThreadAndDatabase
{
    internal class Program
    {
        private static DataBaseManager _dataBaseManager; 
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //Обчислюємо час роботи програми
            _dataBaseManager = new DataBaseManager();
            _dataBaseManager.GetConnectionEvent += DataBaseManager_GetConnectionEvent;
            _dataBaseManager.DataInserted += _dataBaseManager_DataInserted;

            Console.WriteLine("Піготовка програми до запуску ...");
           
            //ThreadAppContext threadAppContext = new ThreadAppContext();
            //threadAppContext.Banans.Any();

            stopwatch.Stop();
            //TimeSpan - змінна, яка може зберігаати, сек, мілісенди, хв, год, дні
            TimeSpan ts = stopwatch.Elapsed; //Оперує тіками - одиниці часу
            Console.WriteLine($"Run time {ts}");
        }

        private static void _dataBaseManager_DataInserted(int obj)
        {
            Console.WriteLine($"Insert data --{obj}--");
        }

        private static void DataBaseManager_GetConnectionEvent(ThreadAppContext threadAppContext)
        {
            DataBaseManager.mre.Set(); //Потік буде працювати у стандартному режимі 
            //Console.WriteLine("Зєднання з БД успішно кількість бананів {0}", threadAppContext.Banans.Count());
            Console.WriteLine("Вкажіть кількість користувачів");
            int count = int.Parse(Console.ReadLine());
            //_dataBaseManager.AddBanans(count);
            _dataBaseManager.AddBanansAsync(count);

            while (true)
            {
                Console.WriteLine("Назміть p - пауза, r - відновити, q - вихід");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.P)
                {
                    DataBaseManager.mre.Reset(); //Призупинити виконання
                    Console.WriteLine("Пауза ...");
                }

                else if (key == ConsoleKey.R)
                {
                    DataBaseManager.mre.Set();
                    Console.WriteLine("Віновлено ...");
                }

                else if (key == ConsoleKey.Q)
                {
                    Console.WriteLine("Вихід");
                    break;
                }
            }
        }
    }
}
