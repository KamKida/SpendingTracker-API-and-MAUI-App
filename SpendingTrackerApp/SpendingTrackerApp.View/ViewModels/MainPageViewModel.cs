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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		private readonly ILogger<MainPageViewModel> _logger;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly JsonService _jsonService;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

		private string _difference;
		private Color _differenceColor;

		private string _title;
		private User _user;
		private ObservableCollection<SpendingCategory> _spendingCategories = new ObservableCollection<SpendingCategory>();
		private ObservableCollection<Spending> _spendings = new ObservableCollection<Spending>();

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

		public User User
		{
			get => _user;
			set
			{
				if (_user != value)
				{
					_user = value;
					OnPropertyChanged(nameof(User));

				}
			}
		}

		public ObservableCollection<SpendingCategory> SpendingCategories
		{
			get => _spendingCategories;
			set
			{
				if (_spendingCategories != value)
				{
					_spendingCategories = value;
					OnPropertyChanged(nameof(SpendingCategories));
				}
			}
		}

		public ObservableCollection<Spending> Spendings
		{
			get => _spendings;
			set
			{
				if (_spendings != value)
				{
					_spendings = value;
					OnPropertyChanged(nameof(Spendings));
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

		public bool BlockInteraction
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

		public string Difference
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

		public ICommand LogOffCommand { get; }
		public ICommand GoToFundsHistoryCommand { get; }
		public ICommand GoToSpendingCategoryyListPageCommand { get; }
		public ICommand GoToSpendingHistoryPageCommand { get; }

		public MainPageViewModel(
			IUserService userService,
			IMapper mapper,
			JsonService jsonService,
			ILogger<MainPageViewModel> logger)
		{
			_logger = logger;
			_userService = userService;
			_jsonService = jsonService;
			_mapper = mapper;

			LogOffCommand = new Command(async () => await LogOff());
			GoToFundsHistoryCommand = new Command(async () => await GoToFundsHistory());
			GoToSpendingCategoryyListPageCommand = new Command(async () => await GoToSpendingCategoryyListPage());
			GoToSpendingHistoryPageCommand = new Command(async () => await GoToSpendingHistoryPage());
		}

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam reset danych głównej strony (UI).");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _userService.GetThisMonthInfo();

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobranie danych użytkownika nie powiodło się. StatusCode: {StatusCode}",
						response.StatusCode
					);
					return;
				}

				var userResponse = _jsonService.Deserialize<UserResponse>(
					await response.Content.ReadAsStringAsync()
				);

				User = _mapper.Map<User>(userResponse);
				SpendingCategories = _mapper.Map<ObservableCollection<SpendingCategory>>(userResponse.SpendingCategoryResponses);
				Spendings = _mapper.Map<ObservableCollection<Spending>>(userResponse.SpendingReponses);
				Title = $"Witaj {User.FirstName} {User.LastName}";

				decimal difference = User.ThisMonthFundSum - User.ThisMonthSpendingsSum;

				if (difference < 0)
				{
					Difference = $"- {difference}";
					DifferenceColor = (Color)Application.Current.Resources["Negative"];
				}
				else
				{
					Difference = $"+ {difference}";
					DifferenceColor = (Color)Application.Current.Resources["Positive"];
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas resetu danych głównej strony (UI).");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono reset danych głównej strony (UI).");
			}
		}

		private async Task LogOff()
		{
			_logger.LogInformation("Rozpoczynam wylogowanie użytkownika (UI).");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				Application.Current.MainPage = new AppShellLogin();

				_logger.LogInformation("Wylogowanie użytkownika zakończone sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas wylogowania użytkownika (UI).");
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
			_logger.LogInformation("Rozpoczynam nawigację do historii funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync($"///{nameof(FundsHistoryPage)}", true);

				_logger.LogInformation("Nawigacja do historii funduszy zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas nawigacji do historii funduszy.");
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
			_logger.LogInformation("Rozpoczynam nawigację do listy kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync($"///{nameof(SpendingCategoryListPage)}", true);

				_logger.LogInformation("Nawigacja do listy kategorii wydatków zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas nawigacji do listy kategorii wydatków");
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
				await Shell.Current.GoToAsync($"///{nameof(SpendingHistoryPage)}", true);

				_logger.LogInformation("Nawigacja do historii wydatków zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas nawigacji do historii wydatków.");
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

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}