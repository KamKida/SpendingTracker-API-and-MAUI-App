using SpendingTrackerApp.ViewModels.FundCategoryViewModels;

namespace SpendingTrackerApp.Pages.FundCategorysPages;

public partial class EditFundCategoryPage : ContentPage
{
	public EditFundCategoryPage(EditFundCategoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var vm = BindingContext as EditFundCategoryPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}