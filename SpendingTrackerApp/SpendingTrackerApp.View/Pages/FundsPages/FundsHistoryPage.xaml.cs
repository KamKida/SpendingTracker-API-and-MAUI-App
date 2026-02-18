using SpendingTrackerApp.ViewModels.FundViewModels;

namespace SpendingTrackerApp.Pages.FundsPages;

public partial class FundsHistoryPage : ContentPage
{
	public FundsHistoryPage(FundsHistoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as FundsHistoryPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}

}

