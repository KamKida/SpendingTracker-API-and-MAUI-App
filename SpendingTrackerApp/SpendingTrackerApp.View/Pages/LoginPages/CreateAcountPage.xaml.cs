using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages;

public partial class CreateAcountPage : ContentPage
{
	public CreateAcountPage(CreateAccountViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

}