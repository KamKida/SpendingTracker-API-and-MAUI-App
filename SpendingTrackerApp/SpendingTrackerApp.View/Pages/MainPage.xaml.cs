using SpendingTrackerApp.ViewModels;

namespace SpendingTrackerApp.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var vm = BindingContext as MainPageViewModel;
		if (vm != null)
		{
			await vm.Reset();
		}
	}
}