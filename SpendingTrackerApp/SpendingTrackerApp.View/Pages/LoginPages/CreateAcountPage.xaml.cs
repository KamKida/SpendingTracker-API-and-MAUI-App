using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages;

public partial class CreateAcountPage : ContentPage
{
	public CreateAcountPage(CreateAccountViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CreateAccountViewModel vm)
        {
            vm.Reset();
        }
    }
}