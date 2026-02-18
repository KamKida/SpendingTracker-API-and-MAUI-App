using SpendingTrackerApp.ViewModels.FundCategoryViewModels;

namespace SpendingTrackerApp.Pages.FundCategorysPages;

public partial class FundCategoryListPage : ContentPage
{
	public FundCategoryListPage(FundCategoryListViewModel vm )
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as FundCategoryListViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}