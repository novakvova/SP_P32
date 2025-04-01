namespace ThreadAndDatabase;

public delegate void DeletageContextConnection(ThreadAppContext threadAppContext);

public class DataBaseManager
{
    private ThreadAppContext _threadAppContext;

    public event DeletageContextConnection GetConnectionEvent;

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
}
