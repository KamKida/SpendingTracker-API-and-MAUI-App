using SpendingTrackerApp.ViewModels.SpendingCategoryViewModels;

namespace SpendingTrackerApp.Pages.SpendingCategoryPages;

public partial class AddSpendingCategoryPage : ContentPage
{
	public AddSpendingCategoryPage(AddSpendingCategoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as AddSpendingCategoryPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}