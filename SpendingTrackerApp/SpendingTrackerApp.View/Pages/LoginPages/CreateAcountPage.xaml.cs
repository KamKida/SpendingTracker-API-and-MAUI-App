using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages.LoginPages;

public partial class CreateAcountPage : ContentPage
{
	public CreateAcountPage(CreateAccountViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as CreateAccountViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}

}