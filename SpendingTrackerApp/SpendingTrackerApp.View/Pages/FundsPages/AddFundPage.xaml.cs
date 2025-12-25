using SpendingTrackerApp.ViewModels.FundViewModels;

namespace SpendingTrackerApp.Pages.FundsPages;

public partial class AddFundPage : ContentPage
{
	public AddFundPage(AddFundPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}