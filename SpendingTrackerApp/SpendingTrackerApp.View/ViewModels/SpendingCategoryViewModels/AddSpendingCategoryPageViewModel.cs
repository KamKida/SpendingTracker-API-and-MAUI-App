using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.SpendingCategoryViewModels
{
	public class AddSpendingCategoryPageViewModel : INotifyPropertyChanged
	{
		private SpendingCategoryRequest _spendingCategoryRequest;
		private ISpendingCategoryService _spendingCategoryService;
		private ILogger<AddSpendingCategoryPageViewModel> _logger;

		private string _message;
		private Color _messageColor;
		private Color _nameEntryColor;
		private Color _weeklyLimitEntryColor;
		private Color _monthlyLimitEntryColor;
		public bool _showLoadingIcon;
		public bool _runLoadingIcon;

		public bool _blockInteraction;

		public SpendingCategoryRequest SpendingCategoryRequest
		{
			get => _spendingCategoryRequest;
			set
			{
				if (_spendingCategoryRequest != value)
				{
					_spendingCategoryRequest = value;
					OnPropertyChanged(nameof(SpendingCategoryRequest));
				}
			}
		}

		public string Message
		{
			get => _message;
			set
			{
				if (_message != value)
				{
					_message = value;
					OnPropertyChanged(nameof(Message));
				}
			}
		}

		public Color MessageColor
		{
			get => _messageColor;
			set
			{
				if (_messageColor != value)
				{
					_messageColor = value;
					OnPropertyChanged(nameof(MessageColor));
				}
			}
		}

		public Color NameEntryColor
		{
			get => _nameEntryColor;
			set
			{
				if (_nameEntryColor != value)
				{
					_nameEntryColor = value;
					OnPropertyChanged(nameof(NameEntryColor));
				}
			}
		}

		public Color WeeklyLimitEntryColor
		{
			get => _weeklyLimitEntryColor;
			set
			{
				if (_weeklyLimitEntryColor != value)
				{
					_weeklyLimitEntryColor = value;
					OnPropertyChanged(nameof(WeeklyLimitEntryColor));
				}
			}
		}

		public Color MonthlyLimitEntryColor
		{
			get => _monthlyLimitEntryColor;
			set
			{
				if (_monthlyLimitEntryColor != value)
				{
					_monthlyLimitEntryColor = value;
					OnPropertyChanged(nameof(MonthlyLimitEntryColor));
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

		public string Description
		{
			get => SpendingCategoryRequest.Description;
			set
			{
				if (SpendingCategoryRequest.Description == value)
					return;

				SpendingCategoryRequest.Description = value;
				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(DescriptionCount));
			}
		}

		public int DescriptionCount
		{
			get => SpendingCategoryRequest.Description?.Length ?? 0;
		}
		public AddSpendingCategoryPageViewModel(
		ISpendingCategoryService spendingCategoryService,
		ILogger<AddSpendingCategoryPageViewModel> logger)
		{
			_spendingCategoryService = spendingCategoryService;
			_logger = logger;
			_spendingCategoryRequest = new SpendingCategoryRequest();

			AddSpendingCategoryCommand = new Command(async () => await AddSpendingCategory());
			CancelAddCommand = new Command(async () => await CancelAdd());
		}

		public ICommand AddSpendingCategoryCommand { get; }
		public ICommand CancelAddCommand { get; }
		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu kategorii wydatków i błędów UI.");

			// Blokada interakcji i ikony ładowania
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				// Reset danych formularza
				SpendingCategoryRequest = new SpendingCategoryRequest();

				// Reset stanu UI
				Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa jest wymagana";
				MessageColor = WeeklyLimitEntryColor = (Color)Application.Current.Resources["Positive"];
				NameEntryColor = Colors.White;
				MonthlyLimitEntryColor = Colors.White; // powiązane z _monthlyLimitEntryColor
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono resetowanie stanu kategorii wydatków i błędów UI.");
			}
		}

		public async Task AddSpendingCategory()
		{
			_logger.LogInformation(
				"Rozpoczynam dodawanie kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
				SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (String.IsNullOrEmpty(SpendingCategoryRequest.Name))
				{
					_logger.LogWarning(
						"Brak nazwy kategorii. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);
					Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa jest wymagana";
					MessageColor = NameEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}


				if (SpendingCategoryRequest.WeeklyLimit.HasValue && !CheckLimit((decimal)SpendingCategoryRequest.WeeklyLimit))
				{
					_logger.LogWarning(
						"Niepoprawna limit tygodniowy kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);
					Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa jest wymagana";
					MessageColor = WeeklyLimitEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				if (SpendingCategoryRequest.MonthlyLimit.HasValue && !CheckLimit((decimal)SpendingCategoryRequest.MonthlyLimit))
				{
					_logger.LogWarning(
						"Niepoprawna limit tygodniowy kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);
					Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa jest wymagana";
					MessageColor = MonthlyLimitEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				if (SpendingCategoryRequest.WeeklyLimit.HasValue && SpendingCategoryRequest.MonthlyLimit.HasValue)
				{
					if(!CheckLimits()){
						_logger.LogWarning(
						"Niepoprawna limity tygodniowe i miesięczne kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);
						Message = "Limit tygodniowy nie może być większy od limitu miesięcznego.";
						MessageColor = WeeklyLimitEntryColor = MonthlyLimitEntryColor = (Color)Application.Current.Resources["Negative"];
						return;
					}
				}


				var response = await _spendingCategoryService.AddSpendingCategory(SpendingCategoryRequest);
				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Dodawanie kategorii wydatków nie powiodło się. Name={Name}, ShouldBe={ShouldBe}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}, Content={Content}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit,
						response.StatusCode,
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak. Kategoria o tej nazwie już istnieje. Jeśli nie, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				MessageColor = (Color)Application.Current.Resources["Positive"];
				WeeklyLimitEntryColor = NameEntryColor = Colors.White;
				Message = "Kategoria wydatków dodana pomyślnie.";

				_logger.LogInformation(
					"Kategoria wydatków dodana pomyślnie. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
			}
		}
		private bool CheckLimit(decimal limit)
		{
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie kwoty kategorii wydatków. Limit={limit}",
				limit
			);

			if (limit <= 0)
			{
				_logger.LogWarning(
					"Kwota kategorii wydatków jest mniejsza lub równa zero. Limit={limit}",
					limit
				);
				return false;
			}

			string amountStr = limit.ToString();
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota kategorii wydatków przekracza 15 cyfr przed przecinkiem. Limit={limit}",
					limit
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota kategorii wydatków przekracza 2 miejsca po przecinku. Limit={limit}",
					limit
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota kategorii wydatków jest poprawna. Limit={limit}",
				limit
			);

			return true;
		}

		private bool CheckLimits()
		{
			if (SpendingCategoryRequest.WeeklyLimit > SpendingCategoryRequest.MonthlyLimit)
				return false;

			return true;
		}
		public async Task CancelAdd()
		{
			_logger.LogInformation("Rozpoczynam anulowanie dodawania kategorii wydatków (powrót do historii wydatków).");

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation("Nawigacja po anulowaniu dodawania kategorii wydatków zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas anulowania dodawania kategorii wydatków."
				);
				throw;
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
