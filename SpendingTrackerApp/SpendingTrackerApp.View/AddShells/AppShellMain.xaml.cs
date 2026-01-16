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

		Routing.RegisterRoute(nameof(AddFundPage), typeof(AddFundPage));
		Routing.RegisterRoute(nameof(EditFundPage), typeof(EditFundPage));

		Routing.RegisterRoute(nameof(AddFundCategoryPage), typeof(AddFundCategoryPage));
		Routing.RegisterRoute(nameof(EditFundCategoryPage), typeof(EditFundCategoryPage));

		Routing.RegisterRoute(nameof(AddSpendingPage), typeof(AddSpendingPage));
		Routing.RegisterRoute(nameof(EditSpendingPage), typeof(EditSpendingPage));

		Routing.RegisterRoute(nameof(AddSpendingCategoryPage), typeof(AddSpendingCategoryPage));
		Routing.RegisterRoute(nameof(EditSpendingCategoryPage), typeof(EditSpendingCategoryPage));

	}
}