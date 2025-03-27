using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace UploadImageThread
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Привіт козаки!");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //Обчислюємо час роботи програми

            const int count = 1000;
            //for (int i = 0; i < 20; i++)
            //{
            //    UploadImage();
            //}
            Thread[] list = new Thread[count];

            for (int i = 0; i < count; i++)
            {
                list[i] = new Thread(UploadImage);
                list[i].Start();
            }
            for (int i = 0; i < count; i++)
            {
                list[i].Join(); //очікуємо завершення хоча б одного із потоків.
            }
            stopwatch.Stop();
            //TimeSpan - змінна, яка може зберігаати, сек, мілісенди, хв, год, дні
            TimeSpan ts = stopwatch.Elapsed; //Оперує тіками - одиниці часу
            Console.WriteLine($"Run time {ts}");
        }

        static void UploadImage()
        {
            string url = "https://picsum.photos/1200/800";

            using HttpClient client = new HttpClient(); // цей вміє качати bytes з інтнету

            //видає з мережі набері байтів для фото - по факуту його скачує
            var bytes = client.GetByteArrayAsync(url).Result;
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "images");
            Directory.CreateDirectory(folder);
            File.WriteAllBytes(Path.Combine(folder, Path.GetRandomFileName() + ".jpg"), bytes);

            Console.WriteLine("-----Thread Id------{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
