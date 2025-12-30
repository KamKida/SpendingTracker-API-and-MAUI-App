using SpendingTrackerApp.Pages;
using SpendingTrackerApp.Pages.FundsPages;

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
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AddFundPage), typeof(AddFundPage));
            Routing.RegisterRoute(nameof(FundsHistoryPage), typeof(FundsHistoryPage));
            Routing.RegisterRoute(nameof(EditFundPage), typeof(EditFundPage));
        }
    }
}
