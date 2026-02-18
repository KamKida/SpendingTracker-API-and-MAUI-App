using SpendingTrackerApp.ViewModels.FundCategoryViewModels;

namespace SpendingTrackerApp.Pages.FundCategorysPages;

public partial class AddFundCategoryPage : ContentPage
{
	public AddFundCategoryPage(AddFundCategoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as AddFundCategoryPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}