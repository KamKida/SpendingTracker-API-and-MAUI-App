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
	public class AddSpendingPageViewModel : INotifyPropertyChanged
	{
		private User _user;
		private SpendingRequest _spendingRequest = new SpendingRequest();
		private ObservableCollection<SpendingCategory> _spendingCategories = new ObservableCollection<SpendingCategory>();
		private SpendingCategoryRequest _spendingCategoryRequest = new SpendingCategoryRequest();
		private SpendingCategoryFilterRequest _spendingCategoryFilterRequest = new SpendingCategoryFilterRequest();
		private JsonService _jsonService;
		private ISpendingService _spendingService;
		private ISpendingCategoryService _spendingCategoryService;
		private IMapper _mapper;
		private ILogger<AddSpendingPageViewModel> _logger;

		private string _message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
		private Color _messageColor = (Color)Application.Current.Resources["Positive"];
		private Color _amountEntryColor = Colors.White;
		private bool _showLoadingIcon = false;
		private bool _runLoadingIcon = false;

		private bool _showCategories = false;
		private bool _blockInteraction = false;
		private bool _enableShowMore = false;
		private bool _showErrorMessage = false;
		private bool _filtersVisible = false;
		private bool _useDateFilter = false;

		private Color _filterEntryColorFrom = Colors.White;
		private Color _filterEntryColorTo = Colors.White;
		private Color _dateColor = Colors.White;

		private bool _showFilterErrorMessage = false;
		private string _filterErrorText;

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

		public int DescriptionCount => SpendingRequest.Description?.Length ?? 0;

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

		public AddSpendingPageViewModel(
			User user,
			JsonService jsonService,
			ISpendingService spendingService,
			ISpendingCategoryService spendingCategoryService,
			IMapper mapper,
			ILogger<AddSpendingPageViewModel> logger)
		{
			_user = user;
			_jsonService = jsonService;
			_spendingService = spendingService;
			_spendingCategoryService = spendingCategoryService;
			_mapper = mapper;
			_logger = logger;

			AddSpendingCommand = new Command(async () => await AddSpending());
			CancelAddCommand = new Command(async () => await CancelAdd());
			ShowCategoryListCommand = new Command(async () => await ShowCategoryList());
			ShowMoreCategoriesCommand = new Command(async () => await ShowMoreCategories());
			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			CancelChoiceCategoryCommand = new Command(async () => await CancelChoiceCategory());
			SelectCategoryCommand = new Command<SpendingCategory>(async (sCategory) => await SelectCategory(sCategory));
			DeleteCategoryCommand = new Command(async () => await DeleteCategory());
		}

		public ICommand AddSpendingCommand { get; }
		public ICommand CancelAddCommand { get; }
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

			SpendingRequest = new SpendingRequest();
			Description = string.Empty;
			SpendingCategoryRequest = new SpendingCategoryRequest();
			Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
			MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Positive"];

			_logger.LogInformation("Zakończono resetowanie stanu wydatku i błędów UI.");
		}

		public async Task AddSpending()
		{
			_logger.LogInformation("Rozpoczynam dodawanie wydatku. Amount={Amount}", SpendingRequest.Amount);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!CheckAmount())
				{
					_logger.LogWarning("Niepoprawna kwota wydatku. Amount={Amount}", SpendingRequest.Amount);
					MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				SpendingRequest.SpendingCategoryId = !string.IsNullOrEmpty(SpendingCategoryRequest.Name)
					? SpendingCategoryRequest.Id
					: null;

				var response = await _spendingService.AddSpending(SpendingRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Dodawanie wydatku nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				Message = "Wydatek dodany pomyślnie.";
				_user.ThisMonthSpendings += SpendingRequest.Amount;

				_logger.LogInformation("Wydatek dodany pomyślnie. Amount={Amount}", SpendingRequest.Amount);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas dodawania wydatku. Amount={Amount}", SpendingRequest.Amount);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas dodawania wydatku. Amount={Amount}", SpendingRequest.Amount);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
				_logger.LogInformation("Zakończono proces dodawania wydatku.");
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

		public async Task ShowCategoryList()
		{
			var request = SpendingCategoryFilterRequest.Clone();
			var result = await _spendingCategoryService.Get10(request);

			var categoryResult = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await result.Content.ReadAsStringAsync());
			SpendingCategoryFilterRequest = new SpendingCategoryFilterRequest();

			SpendingCategories = _mapper.Map<ObservableCollection<SpendingCategory>>(categoryResult);
			EnableShowMore = SpendingCategories.Count % 10 == 0;
			ShowCategories = true;
		}

		public async Task ShowMoreCategories()
		{
			_logger.LogInformation("Rozpoczynam ładowanie kolejnych wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!SpendingCategories.Any())
				{
					_logger.LogWarning("Brak wydatków do załadowania kolejnych.");
					return;
				}

				var request = SpendingCategoryFilterRequest.Clone();
				request.LastDate = SpendingCategories.Last().CreationDate;

				var response = await _spendingCategoryService.Get10(request);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie kolejnych wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					ShowErrorMessage = true;
					return;
				}

				int oldCount = SpendingCategories.Count;
				var categoryResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await response.Content.ReadAsStringAsync());
				var nextCategories = _mapper.Map<ObservableCollection<SpendingCategory>>(categoryResponse);

				foreach (var cat in nextCategories.OrderByDescending(f => f.CreationDate))
					SpendingCategories.Add(cat);

				EnableShowMore = !(SpendingCategories.Count % 10 != 0 || oldCount == SpendingCategories.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas pobierania kolejnych wydatków.");
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task CancelAdd()
		{
			_logger.LogInformation("Rozpoczynam anulowanie dodawania wydatku.");
			try
			{
				await Shell.Current.GoToAsync("..");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas anulowania dodawania wydatku.");
				throw;
			}
		}

		private async Task ShowHideFilters()
		{
			FiltersVisible = !FiltersVisible;
		}

		private async Task ResetFilter()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				SpendingCategoryFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;
				await ShowCategoryList();
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task Filter()
		{
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
					_logger.LogWarning(errorMessage + " WeeklyLimitTo)={WeeklyLimitTo)}", SpendingCategoryFilterRequest.WeeklyLimitTo);
				}
				else
				{
					FilterEntryColorTo = Colors.White;
				}


				if (SpendingCategoryFilterRequest.MonthlyLimitFrom != null && !CheckAmountFilter((decimal)SpendingCategoryFilterRequest.MonthlyLimitFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od'  limitu miesięcznego jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
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

		private bool CheckDateFilter()
		{
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie filtra dat. DateFrom={DateFrom}, DateTo={DateTo}",
				SpendingCategoryFilterRequest.DateFrom,
				SpendingCategoryFilterRequest.DateTo
			);

			if (SpendingCategoryFilterRequest.DateFrom > SpendingCategoryFilterRequest.DateTo)
			{
				_logger.LogWarning(
					"Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})",
					SpendingCategoryFilterRequest.DateFrom,
					SpendingCategoryFilterRequest.DateTo
				);
				return false;
			}

			_logger.LogInformation(
				"Filtr dat jest poprawny. DateFrom={DateFrom}, DateTo={DateTo}",
				SpendingCategoryFilterRequest.DateFrom,
				SpendingCategoryFilterRequest.DateTo
			);

			return true;
		}

		private bool CheckAmountFilter(decimal amount)
		{
			_logger.LogInformation(
					"Rozpoczynam sprawdzanie kwoty wydatku. Amount={Amount}",
					amount
				);

			if (amount <= 0)
			{
				_logger.LogWarning(
					"Kwota wydatku jest mniejsza lub równa zero. Amount={Amount}",
					amount
				);
				return false;
			}

			string amountStr = amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota wydatku przekracza 15 cyfr przed przecinkiem. Amount={Amount}",
					amount
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota wydatku przekracza 2 miejsca po przecinku. Amount={Amount}",
					amount
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota wydatku jest poprawna. Amount={Amount}",
				amount
			);

			return true;
		}

		private async Task SelectCategory(SpendingCategory category)
		{
			SpendingCategoryRequest = _mapper.Map<SpendingCategoryRequest>(category);
			ShowCategories = false;
		}

		private async Task DeleteCategory()
		{
			SpendingCategoryRequest = new SpendingCategoryRequest();
		}

		private async Task CancelChoiceCategory()
		{
			ShowCategories = false;
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
