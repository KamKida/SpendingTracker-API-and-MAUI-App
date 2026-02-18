namespace SpendingTrackerApp.Pages.LoginPages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
	protected async override void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as LoginViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}