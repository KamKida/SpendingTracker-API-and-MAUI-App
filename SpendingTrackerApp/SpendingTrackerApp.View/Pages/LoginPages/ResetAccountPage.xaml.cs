using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages.LoginPages;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage(ResetAccountPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as ResetAccountPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}