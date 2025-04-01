using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadAndDatabase
{
    public class BananService
    {
        public event Action<int> DataInserted;
        private int totalInserted = 0;
        public static ManualResetEvent _mre = new ManualResetEvent(false);

        public void InsertBanansThread(CancellationToken token, ref bool pause, object lockObj, int count)
        {
            using (var context = new ThreadAppContext())
            {
                while (!token.IsCancellationRequested)
                {
                    lock (lockObj)
                    {
                        if (pause) continue;
                    }

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var newBanans = new List<Banan>();
                            for (int i = 0; i < count; i++)
                            {
                                context.Banans.Add(new Banan { FirstName = $"User{i + 1}", LastName = "Doe", Sex = i % 2 == 0 });
                                context.SaveChanges();

                                _mre.WaitOne(Timeout.Infinite); //Якщо був залочений потік то ми чекаємо поки його розлочать

                                DataInserted?.Invoke(i);
                            }
                            transaction.Commit();
                            break;


                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Transaction failed: {ex.Message}");
                        }
                    }

                    //Thread.Sleep(2000);
                }
            }
        }
    }
}
