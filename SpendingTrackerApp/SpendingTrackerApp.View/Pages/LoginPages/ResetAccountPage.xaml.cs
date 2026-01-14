using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages.LoginPages;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage(ResetAccountPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}