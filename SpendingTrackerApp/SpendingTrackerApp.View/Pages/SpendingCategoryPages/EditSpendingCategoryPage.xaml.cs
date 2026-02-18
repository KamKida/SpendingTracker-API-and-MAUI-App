using SpendingTrackerApp.ViewModels.SpendingCategoryViewModels;

namespace SpendingTrackerApp.Pages.SpendingCategoryPages;

public partial class EditSpendingCategoryPage : ContentPage
{
	public EditSpendingCategoryPage(EditSpendingCategoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var vm = BindingContext as EditSpendingCategoryPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}