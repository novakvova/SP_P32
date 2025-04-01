using System.Diagnostics.Metrics;
using System.Reflection;
using Bogus;
using Bogus.DataSets;

namespace ThreadAndDatabase;

//public delegate void DeletageContextConnection(ThreadAppContext threadAppContext);

public class DataBaseManager
{
    private ThreadAppContext _threadAppContext;

    public event Action<int> DataInserted;
    public event Action<ThreadAppContext> GetConnectionEvent;

    //Спеціальний обєкт, який вміє призупитняти довільний потік
    public static ManualResetEvent mre = new ManualResetEvent(false);

    //public event DeletageContextConnection GetConnectionEvent;

    public DataBaseManager()
    {
        Thread runConection = new Thread(RunAsyncConnection);
        runConection.Start();
    }

    private void RunAsyncConnection()
    {
        _threadAppContext = new ThreadAppContext();
        _threadAppContext.Banans.Any();
        if (GetConnectionEvent != null)
            GetConnectionEvent(_threadAppContext);
    }

    public void AddBanansAsync(int count, CancellationToken? token = null)
    {
        Thread thread = new Thread(()=>AddBanans(count, token));
        thread.Start();
    }

    public void AddBanans(int count, CancellationToken? token = null)
    {
        var faker = new Faker<Banan>("uk")
            .RuleFor(b => b.FirstName, f => f.Name.FirstName())
            .RuleFor(b => b.LastName, f => f.Name.LastName())
            .RuleFor(b => b.Image, f => f.Internet.Avatar())
            .RuleFor(b => b.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(b => b.Sex, f => f.Random.Bool());

        for (int i = 0; i < count; i++)
        {
            var b = faker.Generate(1);
            _threadAppContext.Add(b[0]);
            _threadAppContext.SaveChanges();
            DataInserted?.Invoke(i + 1);

            mre.WaitOne(Timeout.Infinite); //Якщо потік зупиненто,
                                           //то у цьому місці буде очіувати продоваження

            if (token != null)
            {
                // Перевіряємо, чи був отриманий сигнал на скасування завдання  
                if (token.Value.IsCancellationRequested)
                {
                    return; // Виходимо з методу, завершуючи виконання завдання  
                }
            }

        }
    }
}
