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
		}
		public ICommand AddFundCommand { get; }


		public async void AddFund()
		{
			await Shell.Current.GoToAsync(nameof(AddFundPage));
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
