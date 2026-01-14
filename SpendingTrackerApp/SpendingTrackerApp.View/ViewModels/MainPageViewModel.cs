using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Pages.FundsPages;
using SpendingTrackerApp.Pages.SpendingPages;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public User User { get; set; }
		private readonly ILogger<MainPageViewModel> _logger;

		private bool _showLoadingIcon = false;
		private bool _runLoadingIcon = false;
		private bool _blockInteraction = false;

		private decimal _difference;
		private Color _differenceColor;

		public string Title { get; set; }

		public bool ShowLoadingIcon
		{
			get => _showLoadingIcon;
			set
			{
				if (_showLoadingIcon != value)
				{
					_showLoadingIcon = value;
					OnPropertyChanged(nameof(ShowLoadingIcon));
				}
			}
		}

		public bool RunLoadingIcon
		{
			get => _runLoadingIcon;
			set
			{
				if (_runLoadingIcon != value)
				{
					_runLoadingIcon = value;
					OnPropertyChanged(nameof(RunLoadingIcon));
				}
			}
		}

		private bool BlockInteraction
		{
			get => _blockInteraction;
			set
			{
				if (_blockInteraction != value)
				{
					_blockInteraction = value;
					OnPropertyChanged(nameof(BlockInteraction));
				}
			}
		}

		public decimal Difference
		{
			get => _difference;
			set
			{
				if (_difference != value)
				{
					_difference = value;
					OnPropertyChanged(nameof(Difference));
				}
			}
		}

		public Color DifferenceColor
		{
			get => _differenceColor;
			set
			{
				if (_differenceColor != value)
				{
					_differenceColor = value;
					OnPropertyChanged(nameof(DifferenceColor));
				}
			}
		}

		public MainPageViewModel(
		User user,
		ILogger<MainPageViewModel> logger)
		{
			User = user;
			_logger = logger;
			Title = $"Witaj {User.FirstName} {User.LastName}";

			GoToFundsHistoryCommand = new Command(async () => await GoToFundsHistory());
			GoToSpendingHistoryPageCommand = new Command(async () => await GoToSpendingHistoryPage());
		}
		public ICommand GoToFundsHistoryCommand { get; }
		public ICommand GoToSpendingHistoryPageCommand { get; }

		public async Task Reset()
		{
			Difference = User.ThisMonthFund - User.ThisMonthSpendings;

			if (Difference < 0)
			{
				DifferenceColor = (Color)Application.Current.Resources["Negative"];
			}
			else
			{
				DifferenceColor = (Color)Application.Current.Resources["Positive"];
			}

		}

		public async Task GoToFundsHistory()
		{
			_logger.LogInformation("Rozpoczynam nawigację do historii środków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync(nameof(FundsHistoryPage), true);

				_logger.LogInformation(
					"Nawigacja do historii środków zakończona sukcesem."
				);

			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Błąd podczas nawigacji do historii środków."
				);
				throw;
			}
			finally
			{

				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task GoToSpendingHistoryPage()
		{
			_logger.LogInformation("Rozpoczynam nawigację do historii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync(nameof(SpendingHistoryPage), true);

				_logger.LogInformation(
					"Nawigacja do historii wydatków zakończona sukcesem."
				);

			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Błąd podczas nawigacji do historii wydatków."
				);
				throw;
			}
			finally
			{

				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}



		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
