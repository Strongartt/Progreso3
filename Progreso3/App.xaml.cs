using Progreso3.Repositories;

namespace Progreso3
{
    public partial class App : Application
    {
        public static PaisesRepositories PaisRepo { get; private set; }

        public App(PaisesRepositories repo)
        {
            InitializeComponent();
            MainPage = new AppShell();
            PaisRepo = repo;
        }
    }
}
