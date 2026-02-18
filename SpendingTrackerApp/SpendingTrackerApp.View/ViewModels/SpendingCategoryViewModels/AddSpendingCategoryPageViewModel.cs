using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.SpendingCategoryViewModels
{
	public class AddSpendingCategoryPageViewModel : INotifyPropertyChanged
	{
		private readonly ISpendingCategoryService _spendingCategoryService;
		private readonly ILogger<AddSpendingCategoryPageViewModel> _logger;

		private SpendingCategoryRequest _spendingCategoryRequest;

		private string _message;
		private Color _messageColor;
		private Color _nameEntryColor;
		private Color _weeklyLimitEntryColor;
		private Color _monthlyLimitEntryColor;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

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
			=> SpendingCategoryRequest.Description?.Length ?? 0;

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

		public ICommand AddSpendingCategoryCommand { get; }
		public ICommand CancelAddCommand { get; }

		public AddSpendingCategoryPageViewModel(
			ISpendingCategoryService spendingCategoryService,
			ILogger<AddSpendingCategoryPageViewModel> logger)
		{
			_logger = logger;
			_spendingCategoryService = spendingCategoryService;

			_spendingCategoryRequest = new SpendingCategoryRequest();

			AddSpendingCategoryCommand = new Command(async () => await AddSpendingCategory());
			CancelAddCommand = new Command(async () => await CancelAdd());
		}

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu formularza dodawania kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				_logger.LogInformation("Resetowanie danych formularza kategorii wydatków.");

				SpendingCategoryRequest = new SpendingCategoryRequest();

				Message =
					"Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku. " +
					"Dozwolone są jedynie liczby dodatnie. Nazwa kategorii jest wymagana.";

				MessageColor = WeeklyLimitEntryColor = (Color)Application.Current.Resources["Positive"];
				NameEntryColor = Colors.White;
				MonthlyLimitEntryColor = Colors.White;

				_logger.LogInformation("Stan formularza oraz komunikaty UI zostały zresetowane poprawnie.");
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono resetowanie formularza dodawania kategorii wydatków.");
			}
		}

		public async Task AddSpendingCategory()
		{
			_logger.LogInformation(
				"Rozpoczynam proces dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
				SpendingCategoryRequest.Name,
				SpendingCategoryRequest.WeeklyLimit,
				SpendingCategoryRequest.MonthlyLimit
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (string.IsNullOrEmpty(SpendingCategoryRequest.Name))
				{
					_logger.LogWarning(
						"Nie podano nazwy kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);

					Message =
						"Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku. " +
						"Dozwolone są jedynie liczby dodatnie. Nazwa kategorii jest wymagana.";

					MessageColor = NameEntryColor =
						(Color)Application.Current.Resources["Negative"];

					return;
				}

				if (SpendingCategoryRequest.WeeklyLimit.HasValue &&
					!CheckLimit((decimal)SpendingCategoryRequest.WeeklyLimit))
				{
					_logger.LogWarning(
						"Niepoprawny limit tygodniowy kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);

					Message =
						"Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku. " +
						"Dozwolone są jedynie liczby dodatnie. Nazwa kategorii jest wymagana.";

					MessageColor = WeeklyLimitEntryColor =
						(Color)Application.Current.Resources["Negative"];

					return;
				}

				if (SpendingCategoryRequest.MonthlyLimit.HasValue &&
					!CheckLimit((decimal)SpendingCategoryRequest.MonthlyLimit))
				{
					_logger.LogWarning(
						"Niepoprawny limit miesięczny kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);

					Message =
						"Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku. " +
						"Dozwolone są jedynie liczby dodatnie. Nazwa kategorii jest wymagana.";

					MessageColor = MonthlyLimitEntryColor =
						(Color)Application.Current.Resources["Negative"];

					return;
				}

				if (SpendingCategoryRequest.WeeklyLimit.HasValue &&
					SpendingCategoryRequest.MonthlyLimit.HasValue &&
					!CheckLimits())
				{
					_logger.LogWarning(
						"Limit tygodniowy jest większy niż limit miesięczny. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit
					);

					Message = "Limit tygodniowy nie może być większy od limitu miesięcznego.";
					MessageColor = WeeklyLimitEntryColor = MonthlyLimitEntryColor =
						(Color)Application.Current.Resources["Negative"];

					return;
				}

				var response = await _spendingCategoryService
	.AddSpendingCategory(SpendingCategoryRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Dodawanie kategorii wydatków nie powiodło się. " +
						"Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}, " +
						"StatusCode={StatusCode}, Content={Content}",
						SpendingCategoryRequest.Name,
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit,
						response.StatusCode,
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message =
						"Coś poszło nie tak. Kategoria o tej nazwie już istnieje. " +
						"Jeśli nie, zrestartuj aplikację i spróbuj ponownie.";

					return;
				}

				MessageColor = (Color)Application.Current.Resources["Positive"];
				NameEntryColor = WeeklyLimitEntryColor = MonthlyLimitEntryColor = Colors.White;
				Message = "Kategoria wydatków została dodana pomyślnie.";

				_logger.LogInformation(
					"Kategoria wydatków została dodana pomyślnie. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
			}
			catch (HttpRequestException httpException)
			{
				_logger.LogError(
					httpException,
					"Błąd sieci podczas dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);

				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd połączenia z serwerem. Sprawdź internet i spróbuj ponownie.";
				throw;
			}
			catch (Exception exception)
			{
				_logger.LogError(
					exception,
					"Nieoczekiwany błąd podczas dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);

				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd aplikacji.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.Name,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
			}
		}

		private bool CheckLimit(decimal limit)
		{
			_logger.LogInformation(
				"Rozpoczynam weryfikację limitu kategorii wydatków. Limit={Limit}",
				limit
			);

			if (limit <= 0)
			{
				_logger.LogWarning(
					"Limit kategorii wydatków jest mniejszy lub równy zero. Limit={Limit}",
					limit
				);
				return false;
			}

			string amountString = limit.ToString();
			var amountParts = amountString.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Limit kategorii wydatków przekracza 15 cyfr przed przecinkiem. Limit={Limit}",
					limit
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Limit kategorii wydatków przekracza 2 miejsca po przecinku. Limit={Limit}",
					limit
				);
				return false;
			}

			_logger.LogInformation(
				"Limit kategorii wydatków został zweryfikowany poprawnie. Limit={Limit}",
				limit
			);

			return true;
		}

		private bool CheckLimits()
		{
			_logger.LogInformation(
				"Sprawdzam relację limitów tygodniowego i miesięcznego kategorii wydatków. WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
				SpendingCategoryRequest.WeeklyLimit,
				SpendingCategoryRequest.MonthlyLimit
			);

			if (SpendingCategoryRequest.WeeklyLimit > SpendingCategoryRequest.MonthlyLimit)
			{
				_logger.LogWarning(
					"Limit tygodniowy jest większy niż limit miesięczny. WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
				return false;
			}

			return true;
		}

		public async Task CancelAdd()
		{
			_logger.LogInformation(
				"Rozpoczynam anulowanie dodawania kategorii wydatków i powrót do poprzedniego widoku."
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation(
					"Powrót do poprzedniego widoku zakończony sukcesem."
				);
			}
			catch (Exception exception)
			{
				_logger.LogError(
					exception,
					"Błąd podczas anulowania dodawania kategorii wydatków."
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

		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	}
}
