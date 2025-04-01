using Bogus;
using Bogus.DataSets;

namespace ThreadAndDatabase;

//public delegate void DeletageContextConnection(ThreadAppContext threadAppContext);

public class DataBaseManager
{
    private ThreadAppContext _threadAppContext;

    public event Action<int> DataInserted;
    public event Action<ThreadAppContext> GetConnectionEvent;

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

    public void AddBanansAsync(int count)
    {
        Thread thread = new Thread(()=>AddBanans(count));
        thread.Start();
    }

    public void AddBanans(int count)
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
        }
    }
}
