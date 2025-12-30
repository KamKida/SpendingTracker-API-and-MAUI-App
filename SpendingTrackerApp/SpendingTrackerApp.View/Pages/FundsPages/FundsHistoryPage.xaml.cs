using SpendingTrackerApp.ViewModels.FundViewModels;

namespace SpendingTrackerApp.Pages.FundsPages;

public partial class FundsHistoryPage : ContentPage
{
	public FundsHistoryPage(FundsHistoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as FundsHistoryPageViewModel;
		if(vm != null)
		{
			await vm.GetTop10Funds();
		}
	}
}