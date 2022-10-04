using LinksStorage.Services;

namespace LinksStorage;

public partial class App : Application
{
    private readonly DataPersistenceOutbox _outbox;

    public App(DataPersistenceOutbox outbox)
    {
        _outbox = outbox;
        InitializeComponent();

        MainPage = new AppShell();
    }
}