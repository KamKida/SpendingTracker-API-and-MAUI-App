using SpendingTrackerApp.Pages;
using SpendingTrackerApp.Pages.FundCategorysPages;
using SpendingTrackerApp.Pages.FundsPages;
using SpendingTrackerApp.Pages.SpendingCategoryPages;
using SpendingTrackerApp.Pages.SpendingPages;
using SpendingTrackerApp.Pages.UserPages;
using SpendingTrackerApp.ViewModels.UserPagesViewModels;

namespace SpendingTrackerApp.AddShells;

public partial class AppShellMain : Shell
{
	public AppShellMain()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

		Routing.RegisterRoute(nameof(AddFundPage), typeof(AddFundPage));
		Routing.RegisterRoute(nameof(FundsHistoryPage), typeof(FundsHistoryPage));
		Routing.RegisterRoute(nameof(EditFundPage), typeof(EditFundPage));

		Routing.RegisterRoute(nameof(FundCategoryListPage), typeof(FundCategoryListPage));
		Routing.RegisterRoute(nameof(AddFundCategoryPage), typeof(AddFundCategoryPage));
		Routing.RegisterRoute(nameof(EditFundCategoryPage), typeof(EditFundCategoryPage));

		Routing.RegisterRoute(nameof(SpendingHistoryPage), typeof(SpendingHistoryPage));
		Routing.RegisterRoute(nameof(AddSpendingPage), typeof(AddSpendingPage));
		Routing.RegisterRoute(nameof(EditSpendingPage), typeof(EditSpendingPage));

		Routing.RegisterRoute(nameof(SpendingCategoryListPage), typeof(SpendingCategoryListPage));
		Routing.RegisterRoute(nameof(AddSpendingCategoryPage), typeof(AddSpendingCategoryPage));
		Routing.RegisterRoute(nameof(EditSpendingCategoryPage), typeof(EditSpendingCategoryPage));

		Routing.RegisterRoute(nameof(EditUserPage), typeof(EditUserPage));
	}
}