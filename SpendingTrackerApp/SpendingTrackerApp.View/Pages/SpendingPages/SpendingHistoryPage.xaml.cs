using SpendingTrackerApp.ViewModels.SpendingViewModels;

namespace SpendingTrackerApp.Pages.SpendingPages;

public partial class SpendingHistoryPage : ContentPage
{
	public SpendingHistoryPage(SpendingHistoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as SpendingHistoryPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}