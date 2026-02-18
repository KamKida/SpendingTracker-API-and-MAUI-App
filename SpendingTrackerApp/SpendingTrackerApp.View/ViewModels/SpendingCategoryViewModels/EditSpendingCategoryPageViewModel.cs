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

namespace SpendingTrackerApp.ViewModels.SpendingCategoryViewModels
{
	[QueryProperty(nameof(SpendingCategory), nameof(SpendingCategory))]
	public class EditSpendingCategoryPageViewModel : INotifyPropertyChanged
	{
		private SpendingCategoryRequest _spendingCategoryRequest;
		private SpendingFilterRequest _filterRequest;
		private SpendingCategory _spendingCategory;
		private JsonService _jsonService;
		private ISpendingCategoryService _spendingCategoryService;
		private ISpendingService _spendingService;
		private IMapper _mapper;
		private ILogger<EditSpendingCategoryPageViewModel> _logger;

		private string _message;
		private Color _messageColor;
		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

		private ObservableCollection<Spending> _spendings;

		private bool _showErrorMessage;
		private bool _filtersVisible;
		private Color _dateColor;
		private Color _filterEntryColorFrom;
		private Color _filterEntryColorTo;
		private Color _nameEntryColor;
		private Color _weeklyLimitEntryColor;
		private Color _monthlyLimitEntryColor;
		private bool _useDateFilter;
		private bool _enableShowMore;
		private bool _enableFilters;
		private bool _showFilterErrorMessage;
		private string _filterErrorText;

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

		public bool EnableFilters
		{
			get => _enableFilters;
			set
			{
				if (_enableFilters != value)
				{
					_enableFilters = value;
					OnPropertyChanged(nameof(EnableFilters));
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

		public SpendingFilterRequest SpendingFilterRequest
		{
			get => _filterRequest;
			set
			{
				if (_filterRequest != value)
				{
					_filterRequest = value;
					OnPropertyChanged(nameof(SpendingFilterRequest));
				}
			}
		}

		public SpendingCategory SpendingCategory
		{
			get => _spendingCategory;
			set
			{
				if (_spendingCategory != value)
				{
					_spendingCategory = value;
					OnPropertyChanged(nameof(SpendingCategory));
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

		public int DescriptionCount => SpendingCategoryRequest.Description?.Length ?? 0;

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

		public EditSpendingCategoryPageViewModel(
			JsonService jsonService,
			ISpendingCategoryService spendingCategoryService,
			ISpendingService spendingService,
			IMapper mapper,
			ILogger<EditSpendingCategoryPageViewModel> logger)
		{
			_logger = logger;

			_jsonService = jsonService;
			_spendingCategoryService = spendingCategoryService;
			_spendingService = spendingService;
			_mapper = mapper;

			_spendingCategoryRequest = new SpendingCategoryRequest();
			_filterRequest = new SpendingFilterRequest();
			_spendings = new ObservableCollection<Spending>();

			EditSpendingCategoryCommand = new Command(async () => await EditSpendingCategory());
			CancelEditCommand = new Command(async () => await CancelEdit());

			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			ShowMoreCommand = new Command(async () => await ShowMore());
		}

		public ICommand EditSpendingCategoryCommand { get; }
		public ICommand CancelEditCommand { get; }
		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand DeleteSpendingCommand { get; }
		public ICommand ShowMoreCommand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie formularza kategorii wydatku oraz stanu błędów UI.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingCategoryRequest.Name = SpendingCategory?.Name;
				SpendingCategoryRequest.Description = SpendingCategory?.Description;
				SpendingFilterRequest.SpendingCategoryId = SpendingCategory.Id;

				Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby dodatnie. Nazwa wymagana.";
				MessageColor = WeeklyLimitEntryColor = MonthlyLimitEntryColor = (Color)Application.Current.Resources["Positive"];

				ShowErrorMessage = false;
				FiltersVisible = false;
				DateColor = Colors.White;
				FilterEntryColorFrom = Colors.White;
				FilterEntryColorTo = Colors.White;
				NameEntryColor = Colors.White;
				UseDateFilter = false;
				EnableShowMore = true;
				EnableFilters = true;
				ShowFilterErrorMessage = false;
				FilterErrorText = string.Empty;

				await SetBaseSpendingsInfo();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas resetowania formularza kategorii wydatku.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono resetowanie formularza kategorii wydatku oraz stanu błędów UI.");
			}
		}


		public async Task EditSpendingCategory()
		{
			_logger.LogInformation(
				"Rozpoczynam edycję kategorii wydatku. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
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
					_logger.LogWarning("Brak nazwy kategorii. Name={Name}", SpendingCategoryRequest.Name);
					Message = "Nazwa kategorii jest wymagana.";
					MessageColor = NameEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				if (SpendingCategoryRequest.WeeklyLimit.HasValue && !CheckLimit((decimal)SpendingCategoryRequest.WeeklyLimit))
				{
					_logger.LogWarning("Niepoprawny limit tygodniowy. WeeklyLimit={WeeklyLimit}", SpendingCategoryRequest.WeeklyLimit);
					Message = "Limit tygodniowy jest niepoprawny.";
					MessageColor = WeeklyLimitEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				if (SpendingCategoryRequest.MonthlyLimit.HasValue && !CheckLimit((decimal)SpendingCategoryRequest.MonthlyLimit))
				{
					_logger.LogWarning("Niepoprawny limit miesięczny. MonthlyLimit={MonthlyLimit}", SpendingCategoryRequest.MonthlyLimit);
					Message = "Limit miesięczny jest niepoprawny.";
					MessageColor = MonthlyLimitEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				if (!CheckLimits())
				{
					_logger.LogWarning("Limit tygodniowy nie może być większy od miesięcznego. WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
						SpendingCategoryRequest.WeeklyLimit,
						SpendingCategoryRequest.MonthlyLimit);
					Message = "Limit tygodniowy nie może być większy od limitu miesięcznego.";
					MessageColor = WeeklyLimitEntryColor = MonthlyLimitEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				SpendingCategoryRequest.Id = SpendingCategory.Id;

				var response = await _spendingCategoryService.EditSpendingCategory(SpendingCategoryRequest);

				_logger.LogInformation(
					"Wynik edycji kategorii wydatku: SpendingCategoryId={SpendingCategoryId}, StatusCode={StatusCode}",
					SpendingCategory.Id,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Edycja kategorii wydatku nie powiodła się. Content={Content}",
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak. Spróbuj ponownie.";
					return;
				}

				SpendingCategory.WeeklyLimit = SpendingCategoryRequest.WeeklyLimit;
				SpendingCategory.MonthlyLimit = SpendingCategoryRequest.MonthlyLimit;

				MessageColor = WeeklyLimitEntryColor = MonthlyLimitEntryColor = (Color)Application.Current.Resources["Positive"];
				Message = "Kategoria wydatku została zaktualizowana pomyślnie.";

				_logger.LogInformation(
					"Edycja kategorii wydatku zakończona sukcesem. SpendingCategoryId={SpendingCategoryId}, NewWeeklyLimit={WeeklyLimit}, NewMonthlyLimit={MonthlyLimit}",
					SpendingCategory.Id,
					SpendingCategoryRequest.WeeklyLimit,
					SpendingCategoryRequest.MonthlyLimit
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas edycji kategorii wydatku. SpendingCategoryId={SpendingCategoryId}", SpendingCategory.Id);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas edycji kategorii wydatku. SpendingCategoryId={SpendingCategoryId}", SpendingCategory.Id);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces edycji kategorii wydatku. SpendingCategoryId={SpendingCategoryId}", SpendingCategory.Id);
			}
		}

		private bool CheckLimit(decimal limit)
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie limitu kategorii wydatków. Limit={Limit}", limit);

			if (limit <= 0)
			{
				_logger.LogWarning("Limit kategorii wydatków jest mniejszy lub równy zero. Limit={Limit}", limit);
				return false;
			}

			string amountStr = limit.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning("Limit kategorii wydatków przekracza 15 cyfr przed przecinkiem. Limit={Limit}", limit);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning("Limit kategorii wydatków przekracza 2 miejsca po przecinku. Limit={Limit}", limit);
				return false;
			}

			_logger.LogInformation("Limit kategorii wydatków jest poprawny. Limit={Limit}", limit);
			return true;
		}

		private bool CheckLimits()
		{
			_logger.LogInformation(
				"Sprawdzam zgodność limitów tygodniowego i miesięcznego. WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
				SpendingCategoryRequest.WeeklyLimit,
				SpendingCategoryRequest.MonthlyLimit
			);

			if (SpendingCategoryRequest.WeeklyLimit.HasValue && SpendingCategoryRequest.MonthlyLimit.HasValue)
			{
				if (SpendingCategoryRequest.WeeklyLimit > SpendingCategoryRequest.MonthlyLimit)
					return false;
			}
			else if (SpendingCategoryRequest.WeeklyLimit.HasValue && SpendingCategory.MonthlyLimit.HasValue)
			{
				if (SpendingCategoryRequest.WeeklyLimit > SpendingCategory.MonthlyLimit)
					return false;
			}
			else if (SpendingCategoryRequest.MonthlyLimit.HasValue && SpendingCategory.WeeklyLimit.HasValue)
			{
				if (SpendingCategory.WeeklyLimit > SpendingCategoryRequest.MonthlyLimit)
					return false;
			}

			_logger.LogInformation("Limity są poprawne.");
			return true;
		}

		public async Task CancelAdd()
		{
			_logger.LogInformation("Rozpoczynam anulowanie dodawania kategorii wydatków.");

			try
			{
				await Shell.Current.GoToAsync("..");
				_logger.LogInformation("Nawigacja po anulowaniu dodawania zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas anulowania dodawania kategorii wydatków.");
				throw;
			}
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation(
				"Rozpoczynam anulowanie edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
				SpendingCategory?.Id
			);

			try
			{
				await Shell.Current.GoToAsync("..");
				_logger.LogInformation(
					"Nawigacja po anulowaniu edycji zakończona sukcesem. SpendingCategoryId={SpendingCategoryId}",
					SpendingCategory?.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas anulowania edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}", SpendingCategory?.Id);
				throw;
			}
		}

		private async Task ShowHideFilters()
		{
			_logger.LogInformation("Rozpoczynam pokazywanie/ukrywanie filtrów wydatków.");

			FiltersVisible = !FiltersVisible;

			_logger.LogInformation("Filtry widoczne: {FiltersVisible}", FiltersVisible);
		}

		private async Task ResetFilter()
		{
			_logger.LogInformation("Rozpoczynam czyszczenie filtrów wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;

				await SetBaseSpendingsInfo();
				FiltersVisible = false;

				_logger.LogInformation("Filtry wydatków zostały wyczyszczone.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas czyszczenia filtrów wydatków.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task SetBaseSpendingsInfo()
		{
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o wydatkach.");

			try
			{
				SpendingFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;

				var response = await _spendingService.Get10(SpendingFilterRequest);

				_logger.LogInformation("Wynik pobierania podstawowych informacji o wydatkach: StatusCode={StatusCode}", response.StatusCode);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);

					ShowErrorMessage = true;
					EnableFilters = false;
					EnableShowMore = false;
					return;
				}

				ShowErrorMessage = false;

				var contentStr = await response.Content.ReadAsStringAsync();
				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingResponse>>(contentStr);

				_mapper.Map(spendingResponse, Spendings);

				EnableShowMore = Spendings.Count % 10 == 0;

				_logger.LogInformation("Pobrano i zmapowano {Count} wydatków.", spendingResponse.Count);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania podstawowych informacji o wydatkach.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania podstawowych informacji o wydatkach.");
				throw;
			}
			finally
			{
				_logger.LogInformation("Zakończono pobieranie podstawowych informacji o wydatkach.");
			}
		}

		private async Task Filter()
		{
			_logger.LogInformation("Rozpoczynam filtrowanie wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				ShowFilterErrorMessage = false;
				string errorMessage = null;

				if (UseDateFilter && !CheckDateFilter())
				{
					DateColor = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Data 'Od' nie może być większa niż data 'Do'.";
					_logger.LogWarning(errorMessage + " DateFrom={DateFrom}, DateTo={DateTo}", SpendingFilterRequest.DateFrom, SpendingFilterRequest.DateTo);
				}
				else
				{
					DateColor = Colors.White;
				}

				if (SpendingFilterRequest.AmountFrom.HasValue && SpendingFilterRequest.AmountTo.HasValue &&
					SpendingFilterRequest.AmountFrom > SpendingFilterRequest.AmountTo)
				{
					FilterEntryColorFrom = FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' nie może być większa niż kwota 'Do'.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}, AmountTo={AmountTo}", SpendingFilterRequest.AmountFrom, SpendingFilterRequest.AmountTo);
				}
				else
				{
					FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				}

				if (SpendingFilterRequest.AmountFrom.HasValue && !CheckAmountFilter((decimal)SpendingFilterRequest.AmountFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}", SpendingFilterRequest.AmountFrom);
				}

				if (SpendingFilterRequest.AmountTo.HasValue && !CheckAmountFilter((decimal)SpendingFilterRequest.AmountTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountTo={AmountTo}", SpendingFilterRequest.AmountTo);
				}

				if (!string.IsNullOrEmpty(errorMessage))
				{
					FilterErrorText = errorMessage;
					ShowFilterErrorMessage = true;
					return;
				}

				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				FiltersVisible = false;

				var request = SpendingFilterRequest.Clone();
				var response = await _spendingService.Get10(request, useDatesFromToo: UseDateFilter);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie wydatków po filtrze nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					ShowErrorMessage = true;
					return;
				}

				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(spendingResponse, Spendings);

				_logger.LogInformation("Filtrowanie wydatków zakończone sukcesem. Pobrano {Count} wydatków.", spendingResponse.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas filtrowania wydatków.");
				ShowErrorMessage = true;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private bool CheckDateFilter()
		{
			_logger.LogInformation("Sprawdzam filtr dat. DateFrom={DateFrom}, DateTo={DateTo}", SpendingFilterRequest.DateFrom, SpendingFilterRequest.DateTo);

			if (SpendingFilterRequest.DateFrom > SpendingFilterRequest.DateTo)
			{
				_logger.LogWarning("Niepoprawny zakres dat. DateFrom ({DateFrom}) > DateTo ({DateTo})", SpendingFilterRequest.DateFrom, SpendingFilterRequest.DateTo);
				return false;
			}

			_logger.LogInformation("Filtr dat jest poprawny.");
			return true;
		}

		private bool CheckAmountFilter(decimal amount)
		{
			_logger.LogInformation("Sprawdzam kwotę wydatku. Amount={Amount}", amount);

			if (amount <= 0)
			{
				_logger.LogWarning("Kwota wydatku jest mniejsza lub równa zero. Amount={Amount}", amount);
				return false;
			}

			string amountStr = amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning("Kwota wydatku przekracza 15 cyfr przed przecinkiem. Amount={Amount}", amount);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning("Kwota wydatku przekracza 2 miejsca po przecinku. Amount={Amount}", amount);
				return false;
			}

			_logger.LogInformation("Kwota wydatku jest poprawna.");
			return true;
		}

		private async Task ShowMore()
		{
			_logger.LogInformation("Rozpoczynam ładowanie kolejnych wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!Spendings.Any())
				{
					_logger.LogWarning("Brak wydatków do załadowania kolejnych.");
					return;
				}

				var request = SpendingFilterRequest.Clone();
				request.LastDate = Spendings.Last().CreationDate;

				var response = await _spendingService.Get10(request);

				_logger.LogInformation("Wynik pobierania kolejnych wydatków: StatusCode={StatusCode}", response.StatusCode);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie kolejnych wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					ShowErrorMessage = true;
					return;
				}

				int oldCount = Spendings.Count;
				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingResponse>>(await response.Content.ReadAsStringAsync());
				var nextSpendings = _mapper.Map<ObservableCollection<Spending>>(spendingResponse);

				foreach (var spending in nextSpendings.OrderByDescending(s => s.CreationDate))
				{
					Spendings.Add(spending);
				}

				EnableShowMore = !(Spendings.Count % 10 != 0 || oldCount == Spendings.Count);

				_logger.LogInformation("Pobrano {NewSpendings} kolejnych wydatków. Łącznie: {TotalSpendings}. EnableShowMore={EnableShowMore}",
					nextSpendings.Count, Spendings.Count, EnableShowMore);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania kolejnych wydatków.");
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania kolejnych wydatków.");
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces ładowania kolejnych wydatków.");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}