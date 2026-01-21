using SpendingTrackerApp.ViewModels.SpendingCategoryViewModels;

namespace SpendingTrackerApp.Pages.SpendingCategoryPages;

public partial class SpendingCategoryListPage : ContentPage
{
	public SpendingCategoryListPage(SpendingCategoryListPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as SpendingCategoryListPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}

}