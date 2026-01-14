using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages.LoginPages;

public partial class CreateAcountPage : ContentPage
{
	public CreateAcountPage(CreateAccountViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

}