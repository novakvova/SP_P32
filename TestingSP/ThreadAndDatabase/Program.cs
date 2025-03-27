using System.Diagnostics;
using System.Text;

namespace ThreadAndDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //Обчислюємо час роботи програми

            
            ThreadAppContext threadAppContext = new ThreadAppContext();
            threadAppContext.Banans.Any();

            stopwatch.Stop();
            //TimeSpan - змінна, яка може зберігаати, сек, мілісенди, хв, год, дні
            TimeSpan ts = stopwatch.Elapsed; //Оперує тіками - одиниці часу
            Console.WriteLine($"Run time {ts}");
        }
    }
}
