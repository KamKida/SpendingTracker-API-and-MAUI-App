using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Pages.FundsPages;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public User User { get; }
		public string Title { get; set; }

		public MainPageViewModel(
		User user)
		{
			User = user;
			Title = $"Witaj {User.FirstName} {User.LastName}";

			AddFundCommand = new Command(AddFund);
			ShowHistoryCommand = new Command(ShowHistory);
		}
		public ICommand AddFundCommand { get; }
		public ICommand ShowHistoryCommand { get; }


		public async void AddFund()
		{
			await Shell.Current.GoToAsync(nameof(AddFundPage));
		}

		public async void ShowHistory()
		{
			await Shell.Current.GoToAsync(nameof(FundsHistoryPage), true);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
