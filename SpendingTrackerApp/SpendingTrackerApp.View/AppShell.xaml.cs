using SpendingTrackerApp.Pages;
using SpendingTrackerApp.Pages.FundCategorysPages;
using SpendingTrackerApp.Pages.LoginPages;
using SpendingTrackerApp.Pages.SpendingPages;

namespace SpendingTrackerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CreateAcountPage), typeof(CreateAcountPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(LoadingDataPage), typeof(LoadingDataPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AddFundPage), typeof(AddFundPage));
            Routing.RegisterRoute(nameof(FundsHistoryPage), typeof(FundsHistoryPage));
            Routing.RegisterRoute(nameof(EditFundPage), typeof(EditFundPage));
            Routing.RegisterRoute(nameof(FundCategoryList), typeof(FundCategoryList));
            Routing.RegisterRoute(nameof(AddFundCategoryPage), typeof(AddFundCategoryPage));
            Routing.RegisterRoute(nameof(SpendingHistoryPage), typeof(SpendingHistoryPage));
			Routing.RegisterRoute(nameof(AddSpendingPage), typeof(AddSpendingPage));
			Routing.RegisterRoute(nameof(EditSpendingPage), typeof(EditSpendingPage));
		}
    }
}
