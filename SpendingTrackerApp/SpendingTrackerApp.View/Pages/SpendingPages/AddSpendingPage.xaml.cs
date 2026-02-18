using SpendingTrackerApp.ViewModels.SpendingViewModels;

namespace SpendingTrackerApp.Pages.SpendingPages;

public partial class AddSpendingPage : ContentPage
{
	public AddSpendingPage(AddSpendingPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as AddSpendingPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}