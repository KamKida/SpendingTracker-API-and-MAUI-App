using SpendingTrackerApp.ViewModels.LoginViewModels;

namespace SpendingTrackerApp.Pages;

public partial class LoadingDataPage : ContentPage
{
	public LoadingDataPage(LoadingDataPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

    }
}