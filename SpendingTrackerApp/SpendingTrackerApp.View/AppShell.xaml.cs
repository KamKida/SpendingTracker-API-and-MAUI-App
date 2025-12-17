using SpendingTrackerApp.Pages;

namespace SpendingTrackerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CreateAcountPage), typeof(CreateAcountPage));
            Routing.RegisterRoute(nameof(EditPasswordPage), typeof(EditPasswordPage));
            Routing.RegisterRoute(nameof(LoadingDataPage), typeof(LoadingDataPage));
        }
    }
}
