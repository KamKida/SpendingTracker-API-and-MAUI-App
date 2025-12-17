using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages;

public partial class EditPasswordPage : ContentPage
{
	public EditPasswordPage(ResetAccountPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}