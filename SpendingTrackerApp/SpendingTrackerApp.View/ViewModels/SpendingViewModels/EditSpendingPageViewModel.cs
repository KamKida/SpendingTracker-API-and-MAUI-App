using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.SpendingViewModels
{
	[QueryProperty(nameof(Spending), nameof(Spending))]
	public class EditSpendingPageViewModel : INotifyPropertyChanged
	{
		private SpendingRequest _spendingRequest = new SpendingRequest();
		private SpendingCategoryRequest _spendingCategoryRequest = new SpendingCategoryRequest();
		private SpendingCategoryFilterRequest _spendingCategoryFilterRequest = new SpendingCategoryFilterRequest();
		private ObservableCollection<SpendingCategory> _spendingCategories = new ObservableCollection<SpendingCategory>();
		private JsonService _jsonService;
		private ISpendingService _spendingService;
		private ISpendingCategoryService _spendingCategoryService;
		private IMapper _mapper;
		private ILogger<EditSpendingPageViewModel> _logger;

		private string _message;
		private Color _messageColor;
		private Color _amountEntryColor;
		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;
		private bool _showCategories;
		private decimal _spendingDifference;
		private Color _differenceColor;
		private bool _enableShowMore;
		private bool _showErrorMessage;
		private bool _filtersVisible;
		private bool _useDateFilter;
		private Color _filterEntryColorFrom;
		private Color _filterEntryColorTo;
		private Color _dateColor;
		private bool _showFilterErrorMessage;
		private string _filterErrorText;
		private Spending _spending;

		public Spending Spending
		{
			get => _spending;
			set
			{
				_spending = value;
				OnPropertyChanged(nameof(Spending));
				OnPropertyChanged(nameof(Spending.Amount));
			}
		}

		public SpendingRequest SpendingRequest
		{
			get => _spendingRequest;
			set
			{
				if (_spendingRequest != value)
				{
					_spendingRequest = value;
					OnPropertyChanged(nameof(SpendingRequest));
				}
			}
		}

		public string Description
		{
			get => SpendingRequest.Description;
			set
			{
				if (SpendingRequest.Description == value) return;

				SpendingRequest.Description = value;
				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(DescriptionCount));
			}
		}

		public SpendingCategoryRequest SpendingCategoryRequest
		{
			get => _spendingCategoryRequest;
			set
			{
				if (_spendingCategoryRequest != value)
				{
					_spendingCategoryRequest = value;
					OnPropertyChanged(nameof(SpendingCategoryRequest));
					OnPropertyChanged(nameof(Description));
					OnPropertyChanged(nameof(DescriptionCount));
				}
			}
		}

		public SpendingCategoryFilterRequest SpendingCategoryFilterRequest
		{
			get => _spendingCategoryFilterRequest;
			set
			{
				if (_spendingCategoryFilterRequest != value)
				{
					_spendingCategoryFilterRequest = value;
					OnPropertyChanged(nameof(SpendingCategoryFilterRequest));
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

		public Color AmountEntryColor
		{
			get => _amountEntryColor;
			set
			{
				if (_amountEntryColor != value)
				{
					_amountEntryColor = value;
					OnPropertyChanged(nameof(AmountEntryColor));
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

		public bool ShowCategories
		{
			get => _showCategories;
			set
			{
				if (_showCategories != value)
				{
					_showCategories = value;
					OnPropertyChanged(nameof(ShowCategories));
				}
			}
		}

		public decimal SpendingDifference
		{
			get => _spendingDifference;
			set
			{
				if (_spendingDifference != value)
				{
					_spendingDifference = value;
					OnPropertyChanged(nameof(SpendingDifference));
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

		public bool EnableShowMore
		{
			get => _enableShowMore;
			set
			{
				if (_enableShowMore != value)
				{
					_enableShowMore = value;
					OnPropertyChanged(nameof(EnableShowMore));
				}
			}
		}

		public bool ShowErrorMessage
		{
			get => _showErrorMessage;
			set
			{
				if (_showErrorMessage != value)
				{
					_showErrorMessage = value;
					OnPropertyChanged(nameof(ShowErrorMessage));
				}
			}
		}

		public bool FiltersVisible
		{
			get => _filtersVisible;
			set
			{
				if (_filtersVisible != value)
				{
					_filtersVisible = value;
					OnPropertyChanged(nameof(FiltersVisible));
				}
			}
		}

		public bool UseDateFilter
		{
			get => _useDateFilter;
			set
			{
				if (_useDateFilter != value)
				{
					_useDateFilter = value;
					OnPropertyChanged(nameof(UseDateFilter));
				}
			}
		}

		public Color DateColor
		{
			get => _dateColor;
			set
			{
				if (_dateColor != value)
				{
					_dateColor = value;
					OnPropertyChanged(nameof(DateColor));
				}
			}
		}

		public Color FilterEntryColorFrom
		{
			get => _filterEntryColorFrom;
			set
			{
				if (_filterEntryColorFrom != value)
				{
					_filterEntryColorFrom = value;
					OnPropertyChanged(nameof(FilterEntryColorFrom));
				}
			}
		}

		public Color FilterEntryColorTo
		{
			get => _filterEntryColorTo;
			set
			{
				if (_filterEntryColorTo != value)
				{
					_filterEntryColorTo = value;
					OnPropertyChanged(nameof(FilterEntryColorTo));
				}
			}
		}

		public bool ShowFilterErrorMessage
		{
			get => _showFilterErrorMessage;
			set
			{
				if (_showFilterErrorMessage != value)
				{
					_showFilterErrorMessage = value;
					OnPropertyChanged(nameof(ShowFilterErrorMessage));
				}
			}
		}

		public string FilterErrorText
		{
			get => _filterErrorText;
			set
			{
				if (_filterErrorText != value)
				{
					_filterErrorText = value;
					OnPropertyChanged(nameof(FilterErrorText));
				}
			}
		}

		public int DescriptionCount => SpendingRequest.Description?.Length ?? 0;

		public EditSpendingPageViewModel(
			JsonService jsonService,
			ISpendingService spendingService,
			ISpendingCategoryService spendingCategoryService,
			IMapper mapper,
			ILogger<EditSpendingPageViewModel> logger)
		{
			_jsonService = jsonService;
			_spendingService = spendingService;
			_spendingCategoryService = spendingCategoryService;
			_mapper = mapper;
			_logger = logger;

			EditSpendingCommand = new Command(async () => await EditSpending());
			CancelEditCommand = new Command(async () => await CancelEdit());
			ShowCategoryListCommand = new Command(async () => await ShowCategoryList());
			ShowMoreCategoriesCommand = new Command(async () => await ShowMoreCategories());
			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			CancelChoiceCategoryCommand = new Command(async () => await CancelChoiceCategory());
			SelectCategoryCommand = new Command<SpendingCategory>(async (category) => await SelectCategory(category));
			DeleteCategoryCommand = new Command(async () => await DeleteCategory());
		}

		public ICommand EditSpendingCommand { get; }
		public ICommand CancelEditCommand { get; }
		public ICommand ShowCategoryListCommand { get; }
		public ICommand ShowMoreCategoriesCommand { get; }
		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand CancelChoiceCategoryCommand { get; }
		public ICommand SelectCategoryCommand { get; }
		public ICommand DeleteCategoryCommand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu wydatku i błędów UI.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingRequest = _mapper.Map<SpendingRequest>(Spending);

				if (Spending.SpendingCategory != null)
				{
					SpendingCategoryRequest = _mapper.Map<SpendingCategoryRequest>(Spending.SpendingCategory);
				}
				else
				{
					SpendingCategoryRequest = new SpendingCategoryRequest();
				}

				SpendingCategoryFilterRequest = new SpendingCategoryFilterRequest();
				SpendingCategories = new ObservableCollection<SpendingCategory>();

				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(DescriptionCount));

				SpendingDifference = (decimal)(SpendingCategoryRequest.WeeklyLimit == null
					? SpendingRequest.Amount
					: SpendingCategoryRequest.WeeklyLimit - SpendingRequest.Amount);

				DifferenceColor = SpendingDifference > 0
					? (Color)Application.Current.Resources["Positive"]
					: (Color)Application.Current.Resources["Negative"];

				ShowCategories = false;
				EnableShowMore = false;
				ShowErrorMessage = false;
				FiltersVisible = false;
				UseDateFilter = false;
				FilterEntryColorFrom = Colors.White;
				FilterEntryColorTo = Colors.White;
				DateColor = Colors.White;
				ShowFilterErrorMessage = false;
				FilterErrorText = string.Empty;

				Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
				MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Positive"];
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono resetowanie stanu wydatku i błędów UI.");
			}
		}


		public async Task EditSpending()
		{
			_logger.LogInformation(
				"Rozpoczynam edycję wydatku. SpendingId={SpendingId}, OldAmount={OldAmount}, NewAmount={NewAmount}",
				Spending.Id,
				Spending.Amount,
				SpendingRequest.Amount
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				bool goodAmount = CheckAmount();
				if (!goodAmount)
				{
					_logger.LogWarning(
						"Niepoprawna kwota wydatku. SpendingId={SpendingId}, Amount={Amount}",
						Spending.Id,
						SpendingRequest.Amount
					);

					MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Negative"];
					Message = "Kwota wydatku jest niepoprawna.";
					return;
				}

				SpendingRequest.Id = Spending.Id;

				SpendingRequest.SpendingCategoryId = string.IsNullOrEmpty(SpendingCategoryRequest.Name)
					? null
					: SpendingCategoryRequest.Id;

				var response = await _spendingService.EditSpending(SpendingRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Edycja wydatku nie powiodła się. SpendingId={SpendingId}, StatusCode={StatusCode}, Content={Content}",
						Spending.Id,
						response.StatusCode,
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				Spending.Amount = SpendingRequest.Amount;
				SpendingDifference = (decimal)(SpendingCategoryRequest.WeeklyLimit == null
					? SpendingRequest.Amount
					: SpendingCategoryRequest.WeeklyLimit - SpendingRequest.Amount);

				DifferenceColor = SpendingDifference > 0
					? (Color)Application.Current.Resources["Positive"]
					: (Color)Application.Current.Resources["Negative"];

				MessageColor = (Color)Application.Current.Resources["Positive"];
				Message = "Wydatek zaktualizowany pomyślnie.";

				_logger.LogInformation(
					"Edycja wydatku zakończona sukcesem. SpendingId={SpendingId}, NewAmount={NewAmount}",
					Spending.Id,
					SpendingRequest.Amount
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji wydatku. SpendingId={SpendingId}, Amount={Amount}",
					Spending.Id,
					SpendingRequest.Amount
				);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji wydatku. SpendingId={SpendingId}, Amount={Amount}",
					Spending.Id,
					SpendingRequest.Amount
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

				_logger.LogInformation(
					"Zakończono proces edycji wydatku. SpendingId={SpendingId}",
					Spending.Id
				);
			}
		}

		public async Task ShowCategoryList()
		{
			_logger.LogInformation("Rozpoczynam pobieranie listy kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var request = SpendingCategoryFilterRequest.Clone();
				var result = await _spendingCategoryService.Get10(request);
				var categoryResult = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await result.Content.ReadAsStringAsync());

				SpendingCategoryFilterRequest = new SpendingCategoryFilterRequest();
				SpendingCategories = _mapper.Map<ObservableCollection<SpendingCategory>>(categoryResult);

				EnableShowMore = SpendingCategories.Count % 10 == 0;
				ShowCategories = true;

				_logger.LogInformation("Lista kategorii wydatków została pobrana. Łącznie kategorii: {Count}", SpendingCategories.Count);
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task ShowMoreCategories()
		{
			_logger.LogInformation("Rozpoczynam ładowanie kolejnych kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!SpendingCategories.Any())
				{
					_logger.LogWarning("Brak kategorii do załadowania kolejnych.");
					return;
				}

				var request = SpendingCategoryFilterRequest.Clone();
				request.LastDate = SpendingCategories.Last().CreationDate;

				var response = await _spendingCategoryService.Get10(request);

				_logger.LogInformation("Wynik pobierania kolejnych kategorii: StatusCode={StatusCode}", response.StatusCode);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie kolejnych kategorii nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					ShowErrorMessage = true;
					return;
				}

				int oldCount = SpendingCategories.Count;

				var categoryResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await response.Content.ReadAsStringAsync());
				var next10Categories = _mapper.Map<ObservableCollection<SpendingCategory>>(categoryResponse);

				foreach (var category in next10Categories.OrderByDescending(c => c.CreationDate))
				{
					SpendingCategories.Add(category);
				}

				EnableShowMore = !(SpendingCategories.Count % 10 != 0 || oldCount == SpendingCategories.Count);

				_logger.LogInformation(
					"Pobrano {NewCategories} kolejnych kategorii. Łączna liczba kategorii: {TotalCategories}. EnableShowMore={EnableShowMore}",
					next10Categories.Count,
					SpendingCategories.Count,
					EnableShowMore
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania kolejnych kategorii.");
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania kolejnych kategorii.");
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces ładowania kolejnych kategorii wydatków.");
			}
		}

		private bool CheckAmount()
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie kwoty wydatku. Amount={Amount}", SpendingRequest.Amount);

			if (SpendingRequest.Amount <= 0)
			{
				_logger.LogWarning("Kwota wydatku jest mniejsza lub równa zero. Amount={Amount}", SpendingRequest.Amount);
				return false;
			}

			string amountStr = SpendingRequest.Amount.ToString(CultureInfo.InvariantCulture);
			var parts = amountStr.Split('.');

			if (parts[0].Length > 15)
			{
				_logger.LogWarning("Kwota wydatku przekracza 15 cyfr przed przecinkiem. Amount={Amount}", SpendingRequest.Amount);
				return false;
			}

			if (parts.Length == 2 && parts[1].Length > 2)
			{
				_logger.LogWarning("Kwota wydatku przekracza 2 miejsca po przecinku. Amount={Amount}", SpendingRequest.Amount);
				return false;
			}

			_logger.LogInformation("Kwota wydatku jest poprawna. Amount={Amount}", SpendingRequest.Amount);
			return true;
		}

		private bool CheckAmountFilter(decimal amount)
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie kwoty filtra. Amount={Amount}", amount);

			if (amount <= 0)
			{
				_logger.LogWarning("Kwota filtra jest mniejsza lub równa zero. Amount={Amount}", amount);
				return false;
			}

			string amountStr = amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning("Kwota filtra przekracza 15 cyfr przed przecinkiem. Amount={Amount}", amount);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning("Kwota filtra przekracza 2 miejsca po przecinku. Amount={Amount}", amount);
				return false;
			}

			_logger.LogInformation("Kwota filtra jest poprawna. Amount={Amount}", amount);
			return true;
		}

		private bool CheckDateFilter()
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie filtra dat. DateFrom={DateFrom}, DateTo={DateTo}",
				SpendingCategoryFilterRequest.DateFrom,
				SpendingCategoryFilterRequest.DateTo
			);

			if (SpendingCategoryFilterRequest.DateFrom > SpendingCategoryFilterRequest.DateTo)
			{
				_logger.LogWarning("Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})",
					SpendingCategoryFilterRequest.DateFrom,
					SpendingCategoryFilterRequest.DateTo
				);
				return false;
			}

			_logger.LogInformation("Filtr dat jest poprawny. DateFrom={DateFrom}, DateTo={DateTo}",
				SpendingCategoryFilterRequest.DateFrom,
				SpendingCategoryFilterRequest.DateTo
			);

			return true;
		}

		private async Task ResetFilter()
		{
			_logger.LogInformation("Rozpoczynam czyszczenie filtrów kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingCategoryFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;

				await ShowCategoryList();

				_logger.LogInformation("Filtry kategorii wydatków zostały wyczyszczone.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas czyszczenia filtrów kategorii wydatków.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task SelectCategory(SpendingCategory category)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingCategoryRequest = _mapper.Map<SpendingCategoryRequest>(category);
				SpendingDifference = (decimal)(SpendingCategoryRequest.WeeklyLimit == null
					? Spending.Amount
					: SpendingCategoryRequest.WeeklyLimit - SpendingRequest.Amount);

				DifferenceColor = SpendingDifference > 0
					? (Color)Application.Current.Resources["Positive"]
					: (Color)Application.Current.Resources["Negative"];

				ShowCategories = false;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task DeleteCategory()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingCategoryRequest = new SpendingCategoryRequest();
				DifferenceColor = (Color)Application.Current.Resources["Positive"];
				SpendingDifference = Spending.Amount;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task CancelChoiceCategory()
		{
			ShowCategories = false;
		}

		private async Task ShowHideFilters()
		{
			_logger.LogInformation("Rozpoczynam pokazywanie/ukrywanie filtrów kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FiltersVisible = !FiltersVisible;
				_logger.LogInformation("Filtry są teraz widoczne: {FiltersVisible}", FiltersVisible);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pokazywania/ukrywania filtrów kategorii wydatków.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
				_logger.LogInformation("Zakończono proces pokazywania/ukrywania filtrów kategorii wydatków.");
			}
		}


		private async Task Filter()
		{
			_logger.LogInformation("Rozpoczynam filtrowanie kategorii wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				ShowFilterErrorMessage = false;
				string errorMessage = null;

				if (UseDateFilter)
				{
					if (!CheckDateFilter())
					{
						DateColor = (Color)Application.Current.Resources["Negative"];
						errorMessage = "Data 'Od' nie może być większa niż data 'Do'.";
						_logger.LogWarning(errorMessage + " DateFrom={DateFrom}, DateTo={DateTo}",
							SpendingCategoryFilterRequest.DateFrom, SpendingCategoryFilterRequest.DateTo);
					}
					else
					{
						DateColor = Colors.White;
					}
				}

				if (SpendingCategoryFilterRequest.WeeklyLimitFrom != null && !CheckAmountFilter((decimal)SpendingCategoryFilterRequest.WeeklyLimitFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " WeeklyLimitFrom={WeeklyLimitFrom}", SpendingCategoryFilterRequest.WeeklyLimitFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (SpendingCategoryFilterRequest.WeeklyLimitTo != null && !CheckAmountFilter((decimal)SpendingCategoryFilterRequest.WeeklyLimitTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " WeeklyLimitTo={WeeklyLimitTo}", SpendingCategoryFilterRequest.WeeklyLimitTo);
				}
				else
				{
					FilterEntryColorTo = Colors.White;
				}

				if (SpendingCategoryFilterRequest.MonthlyLimitFrom != null && !CheckAmountFilter((decimal)SpendingCategoryFilterRequest.MonthlyLimitFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' limitu miesięcznego jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " MonthlyLimitFrom={MonthlyLimitFrom}", SpendingCategoryFilterRequest.MonthlyLimitFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (SpendingCategoryFilterRequest.MonthlyLimitTo != null && !CheckAmountFilter((decimal)SpendingCategoryFilterRequest.MonthlyLimitTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' limitu miesięcznego jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " MonthlyLimitTo={MonthlyLimitTo}", SpendingCategoryFilterRequest.MonthlyLimitTo);
				}
				else
				{
					FilterEntryColorTo = Colors.White;
				}

				if (!string.IsNullOrEmpty(errorMessage))
				{
					FilterErrorText = errorMessage;
					ShowFilterErrorMessage = true;
					return;
				}

				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				FiltersVisible = false;

				var request = SpendingCategoryFilterRequest.Clone();

				var response = await _spendingCategoryService.Get10(request, useDatesFromToo: UseDateFilter);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie kategorii wydatków po filtrze nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);
					ShowErrorMessage = true;
					return;
				}

				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(spendingResponse, SpendingCategories);

				_logger.LogInformation("Filtrowanie kategorii wydatków zakończone sukcesem. Liczba kategorii: {Count}", SpendingCategories.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas filtrowania kategorii wydatków.");
				ShowErrorMessage = true;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation("Rozpoczynam anulowanie edycji wydatku. SpendingId={SpendingId}, Amount={Amount}", Spending?.Id, Spending?.Amount);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync("..");
				_logger.LogInformation("Nawigacja po anulowaniu edycji wydatku zakończona sukcesem. SpendingId={SpendingId}", Spending?.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas anulowania edycji wydatku. SpendingId={SpendingId}", Spending?.Id);
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
