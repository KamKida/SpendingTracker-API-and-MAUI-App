using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.AddShells;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.FundsPages;
using SpendingTrackerApp.Pages.SpendingCategoryPages;
using SpendingTrackerApp.Pages.SpendingPages;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public User User { get; set; }
		private IUserService _userService;
		private IMapper _mapper;
		private JsonService _jsonService;
		private readonly ILogger<MainPageViewModel> _logger;

		private bool _showLoadingIcon = false;
		private bool _runLoadingIcon = false;
		private bool _blockInteraction = false;

		private decimal _difference;
		private Color _differenceColor;

		private string _title;
		public string Title
		{
			get => _title;
			set
			{
				if (_title != value)
				{
					_title = value;
					OnPropertyChanged(nameof(Title));
				}
			}
		}

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
		IUserService userService,
		IMapper mapper,
		JsonService jsonService,
		ILogger<MainPageViewModel> logger)
		{
			User = user;
			_userService = userService;
			_mapper = mapper;
			_jsonService = jsonService;
			_logger = logger;

			LogOffCommand = new Command(async () => await LogOff());
			GoToFundsHistoryCommand = new Command(async () => await GoToFundsHistory());
			GoToSpendingHistoryPageCommand = new Command(async () => await GoToSpendingHistoryPage());
			GoToSpendingCategoryyListPageCommand = new Command(async () => await GoToSpendingCategoryyListPage());
		}
		public ICommand LogOffCommand { get; }
		public ICommand GoToFundsHistoryCommand { get; }
		public ICommand GoToSpendingHistoryPageCommand { get; }
		public ICommand GoToSpendingCategoryyListPageCommand { get; }

		public async Task Reset()
		{
			var response = await _userService.GetBaseInfo();
			if (!response.IsSuccessStatusCode)
			{

			}

			UserResponse userResponse = _jsonService.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync());

			User = _mapper.Map<User>(userResponse);
			OnPropertyChanged(nameof(User));

			Difference = User.ThisMonthFund - User.ThisMonthSpendings;
			Title = $"Witaj {User.FirstName} {User.LastName}";

			if (Difference < 0)
			{
				DifferenceColor = (Color)Application.Current.Resources["Negative"];
			}
			else
			{
				DifferenceColor = (Color)Application.Current.Resources["Positive"];
			}

		}

		private async Task LogOff()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				Application.Current.MainPage = new AppShellLogin();

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

		private async Task GoToFundsHistory()
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

		private async Task GoToSpendingHistoryPage()
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

		private async Task GoToSpendingCategoryyListPage()
		{
			_logger.LogInformation("Rozpoczynam nawigację do listy kategori wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync(nameof(SpendingCategoryListPage), true);

				_logger.LogInformation(
					"Nawigacja do listy kategori wydatków zakończona sukcesem."
				);

			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Błąd podczas nawigacji do listy kategori wydatków."
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
