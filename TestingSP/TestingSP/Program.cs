using System.Diagnostics;
using System.Text;

namespace TestingSP
{
    internal class Program
    {
        //виникає проблема одногчого доступу до ресурсів у системі.
        //Виникає проблема коли один потік змінює колір консолі, а інший потік 
        //у цей же самий час звертається то тієї консолі. Виникає не відповідність у роботі.
        //Викорисаємо lock - блокування, що ніхто інший у інших потоках не мав доступу до консолі
        //valera - це змінна, яка фіксує блокування
        private static readonly object valera = new object();
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //Обчислюємо час роботи програми

            //Процес - це запущена програма у Windows
            //Кожен прцоес має свій ідентифікатор у системі Widnows або macOS
            //ctrl+shift+esc - диспетчер задач Windows

            //У прцоесі можна виділяти потоки - поток - це окрема задача.
            //Завжди ми маємо основний потік - main
            //Якщо запускаємо прогармау вона виконується в осному потоці.
            
            //Thread - клас для роботи з потоками
            //Отримуємо ідентифікатор потоку
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Основний потік має ідентифікатор {0}", threadId);
            int coreCount = Environment.ProcessorCount; //повертає кількість ядер у системі
            Console.WriteLine("Кількість ядер {0}", coreCount);

            Thread runner = new Thread(ViewInfo); //Створили окремеий потік 
            //і вказали, який мето буде запускатися при роботі даного потоку.
            runner.Start(); //запускаємо потік -- він працює асинхронно. - це паралельно

            for (int i = 0; i < 10; i++) 
            {
                //var color = Console.ForegroundColor;
                lock (valera)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"----Запуск ракети {i + 1}---");
                    Console.ResetColor();
                }
                Thread.Sleep(600);
            }
            runner.Join(); //Якщо потік не завершив роботу він блоку основний потік, поки 
            //поки не завершиться свою роботу.

            stopwatch.Stop();
            //TimeSpan - змінна, яка може зберігаати, сек, мілісенди, хв, год, дні
            TimeSpan ts = stopwatch.Elapsed; //Оперує тіками - одиниці часу
            Console.WriteLine($"Run time {ts}");
            
            Console.WriteLine("Програма завершила роботу :(");
            return;
        }

        private static void ViewInfo()
        {
            for (int i = 0; i < 10; i++)
            {
                lock (valera)
                {
                    //var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"----Напад дикіх кабанів {i + 1}---");
                    //Console.ForegroundColor = color;
                    Console.ResetColor();
                }
                Thread.Sleep(400);
            }
        }
    }
}
