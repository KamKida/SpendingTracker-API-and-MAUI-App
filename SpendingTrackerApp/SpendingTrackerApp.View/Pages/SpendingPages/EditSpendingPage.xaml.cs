using SpendingTrackerApp.ViewModels.SpendingViewModels;

namespace SpendingTrackerApp.Pages.SpendingPages;

public partial class EditSpendingPage : ContentPage
{
	public EditSpendingPage(EditSpendingPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var vm = BindingContext as EditSpendingPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}