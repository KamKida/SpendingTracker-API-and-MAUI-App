using SpendingTrackerApp.ViewModels.FundViewModels;

namespace SpendingTrackerApp.Pages.FundsPages;

public partial class EditFundPage : ContentPage
{
	public EditFundPage(EditFundPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var vm = BindingContext as EditFundPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}

}