using SpendingTrackerApp.ViewModels.UserPagesViewModels;

namespace SpendingTrackerApp.Pages.UserPages;

public partial class EditUserPage : ContentPage
{
	public EditUserPage(EditUserPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as EditUserPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}