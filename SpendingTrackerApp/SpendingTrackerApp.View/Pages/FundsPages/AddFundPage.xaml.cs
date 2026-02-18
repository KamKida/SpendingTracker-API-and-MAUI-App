using SpendingTrackerApp.ViewModels.FundViewModels;

namespace SpendingTrackerApp.Pages.FundsPages;

public partial class AddFundPage : ContentPage
{
	public AddFundPage(AddFundPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as AddFundPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}

}