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

namespace SpendingTrackerApp.ViewModels.FundCategoryViewModels
{
	[QueryProperty(nameof(Domain.Models.FundCategory), nameof(Domain.Models.FundCategory))]
	public class EditFundCategoryPageViewModel : INotifyPropertyChanged
	{
		private readonly JsonService _jsonService;
		private readonly IFundCategoryService _fundCategoryService;
		private readonly IFundService _fundService;
		private readonly IMapper _mapper;
		private readonly ILogger<EditFundCategoryPageViewModel> _logger;

		private FundCategoryRequest _fundCategoryRequest = new FundCategoryRequest();
		private FundFilterRequest _filterRequest = new FundFilterRequest();
		private FundCategory _fundCategory;

		private string _message;

		private Color _messageColor;
		private Color _shouldBeEntryColor;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

		private bool _showErrorMessage;
		private bool _filtersVisible;
		private bool _useDateFilter;
		private bool _enableShowMore;
		private bool _enableFilters;

		private bool _showFilterErrorMessage;
		private string _filterErrorText;

		private Color _dateColor = Colors.White;
		private Color _filterEntryColorFrom = Colors.White;
		private Color _filterEntryColorTo = Colors.White;

		private ObservableCollection<Fund> _funds = new ObservableCollection<Fund>();

		public FundCategory FundCategory
		{
			get => _fundCategory;
			set
			{
				if (_fundCategory != value)
				{
					_fundCategory = value;
					OnPropertyChanged(nameof(FundCategory));
				}
			}
		}

		public FundCategoryRequest FundCategoryRequest
		{
			get => _fundCategoryRequest;
			set
			{
				if (_fundCategoryRequest != value)
				{
					_fundCategoryRequest = value;
					OnPropertyChanged(nameof(FundCategoryRequest));
					OnPropertyChanged(nameof(Description));
					OnPropertyChanged(nameof(DescriptionCount));
				}
			}
		}

		public FundFilterRequest FundFilterRequest
		{
			get => _filterRequest;
			set
			{
				if (_filterRequest != value)
				{
					_filterRequest = value;
					OnPropertyChanged(nameof(FundFilterRequest));
				}
			}
		}

		public ObservableCollection<Fund> Funds
		{
			get => _funds;
			set
			{
				if (_funds != value)
				{
					_funds = value;
					OnPropertyChanged(nameof(Funds));
				}
			}
		}

		public string Description
		{
			get => FundCategoryRequest.Description;
			set
			{
				if (FundCategoryRequest.Description == value)
					return;

				FundCategoryRequest.Description = value;
				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(DescriptionCount));
			}
		}

		public int DescriptionCount => FundCategoryRequest.Description?.Length ?? 0;

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

		public Color ShouldBeEntryColor
		{
			get => _shouldBeEntryColor;
			set
			{
				if (_shouldBeEntryColor != value)
				{
					_shouldBeEntryColor = value;
					OnPropertyChanged(nameof(ShouldBeEntryColor));
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

		public ICommand EditFundCategoryCommand { get; }
		public ICommand CancelEditCommand { get; }
		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand DeleteFundCommand { get; }
		public ICommand ShowMoreCommand { get; }

		public EditFundCategoryPageViewModel(
			JsonService jsonService,
			IFundCategoryService fundCategoryService,
			IFundService fundService,
			IMapper mapper,
			ILogger<EditFundCategoryPageViewModel> logger)
		{
			_jsonService = jsonService;
			_fundCategoryService = fundCategoryService;
			_fundService = fundService;
			_mapper = mapper;
			_logger = logger;

			EditFundCategoryCommand = new Command(async () => await EditFundCategory());
			CancelEditCommand = new Command(async () => await CancelEdit());
			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			DeleteFundCommand = new Command<Fund>(async fund => await DeleteFund(fund));
			ShowMoreCommand = new Command(async () => await ShowMore());
		}

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie formularza edycji kategorii funduszu.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundCategoryRequest = new FundCategoryRequest
				{
					Name = FundCategory?.Name,
					Description = FundCategory?.Description
				};

				FundFilterRequest = new FundFilterRequest
				{
					FundCategoryId = FundCategory?.Id
				};

				Message =
					"Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku. Jedynie liczby dodatnie. Nazwa wymagana.";
				MessageColor = (Color)Application.Current.Resources["Positive"];
				ShouldBeEntryColor = Colors.White;

				ShowErrorMessage = false;
				ShowFilterErrorMessage = false;
				FilterErrorText = string.Empty;

				FiltersVisible = false;
				UseDateFilter = false;
				EnableShowMore = true;
				EnableFilters = true;

				DateColor = Colors.White;
				FilterEntryColorFrom = Colors.White;
				FilterEntryColorTo = Colors.White;

				Funds.Clear();

				await SetBaseFundsInfo();

				_logger.LogInformation("Reset formularza edycji kategorii funduszu zakończony pomyślnie.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas resetowania formularza edycji kategorii funduszu.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}


		public async Task EditFundCategory()
		{
			_logger.LogInformation(
				"Rozpoczynam edycję kategorii funduszu. FundCategoryId={FundCategoryId}",
				FundCategory.Id
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (string.IsNullOrWhiteSpace(FundCategoryRequest.Name))
				{
					_logger.LogWarning("Walidacja nie powiodła się: brak nazwy kategorii.");
					Message = "Nazwa jest wymagana.";
					MessageColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				if (!CheckShouldBe())
				{
					_logger.LogWarning(
						"Walidacja wartości docelowej kwoty nie powiodła się. ShouldBe={ShouldBe}",
						FundCategoryRequest.ShouldBe
					);

					Message = "Wartość docelowa jest niepoprawna.";
					MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				FundCategoryRequest.Id = FundCategory.Id;

				var response = await _fundCategoryService.EditFundCategory(FundCategoryRequest);

				_logger.LogInformation(
					"Odpowiedź z serwera po edycji kategorii funduszu. StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Edycja kategorii funduszu nie powiodła się. Content={Content}",
						content
					);

					Message = "Coś poszło nie tak. Spróbuj ponownie.";
					MessageColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				FundCategory.ShouldBe = FundCategoryRequest.ShouldBe;

				Message = "Kategoria funduszu została zaktualizowana pomyślnie.";
				MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Positive"];

				_logger.LogInformation(
					"Edycja kategorii funduszu zakończona sukcesem. FundCategoryId={FundCategoryId}",
					FundCategory.Id
				);
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Błąd sieci podczas edycji kategorii funduszu.");
				Message = "Błąd sieci. Spróbuj ponownie.";
				MessageColor = (Color)Application.Current.Resources["Negative"];
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas edycji kategorii funduszu.");
				Message = "Wystąpił nieoczekiwany błąd.";
				MessageColor = (Color)Application.Current.Resources["Negative"];
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces edycji kategorii funduszu. FundCategoryId={FundCategoryId}",
					FundCategory.Id
				);
			}
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation(
				"Anulowanie edycji kategorii funduszu. FundCategoryId={FundCategoryId}",
				FundCategory?.Id
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation("Powrót do poprzedniego widoku zakończony pomyślnie.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas anulowania edycji kategorii funduszu.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task DeleteFund(Fund fund)
		{
			_logger.LogInformation(
				"Rozpoczynam usuwanie funduszu. FundId={FundId}, Kwota={Amount}, DataUtworzenia={CreationDate}",
				fund.Id,
				fund.Amount,
				fund.CreationDate
			);

			bool answer = await Application.Current.MainPage.DisplayAlert(
				string.Empty,
				$"Czy na pewno usunąć fundusz z kwotą: {fund.Amount} zł?\nDodany: {fund.CreationDate}",
				"Tak",
				"Nie"
			);

			if (!answer)
			{
				_logger.LogInformation("Usuwanie funduszu zostało anulowane przez użytkownika.");
				return;
			}

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _fundService.DeleteFund(fund.Id);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Usuwanie funduszu nie powiodło się. StatusCode={StatusCode}",
						response.StatusCode
					);

					ShowErrorMessage = true;
					return;
				}

				Funds.Remove(fund);

				_logger.LogInformation(
					"Fundusz został usunięty pomyślnie. FundId={FundId}",
					fund.Id
				);
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Błąd sieci podczas usuwania funduszu.");
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas usuwania funduszu.");
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

		private async Task ShowHideFilters()
		{
			_logger.LogInformation("Zmiana widoczności panelu filtrów.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FiltersVisible = !FiltersVisible;

				_logger.LogInformation(
					"Widoczność filtrów ustawiona na: {FiltersVisible}",
					FiltersVisible
				);
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task ResetFilter()
		{
			_logger.LogInformation("Rozpoczynam resetowanie filtrów funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;

				await SetBaseFundsInfo();

				_logger.LogInformation("Filtry funduszy zostały zresetowane.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas resetowania filtrów funduszy.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task SetBaseFundsInfo()
		{
			_logger.LogInformation("Pobieranie podstawowych danych funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;

				var response = await _fundService.Get10(FundFilterRequest);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie funduszy nie powiodło się. StatusCode={StatusCode}",
						response.StatusCode
					);

					ShowErrorMessage = true;
					EnableFilters = false;
					EnableShowMore = false;
					return;
				}

				ShowErrorMessage = false;

				var content = await response.Content.ReadAsStringAsync();
				var fundResponse =
					_jsonService.Deserialize<ObservableCollection<FundResponse>>(content);

				_mapper.Map(fundResponse, Funds);

				EnableShowMore = Funds.Count % 10 == 0;

				_logger.LogInformation(
					"Pobrano {Count} funduszy. EnableShowMore={EnableShowMore}",
					Funds.Count,
					EnableShowMore
				);
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Błąd sieci podczas pobierania funduszy.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania funduszy.");
				throw;
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
			_logger.LogInformation("Rozpoczynam filtrowanie funduszy.");

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
					errorMessage = "Data „Od” nie może być późniejsza niż data „Do”.";
				}

				if (FundFilterRequest.AmountFrom != null &&
					FundFilterRequest.AmountTo != null &&
					FundFilterRequest.AmountFrom > FundFilterRequest.AmountTo)
				{
					FilterEntryColorFrom = FilterEntryColorTo =
						(Color)Application.Current.Resources["Negative"];

					errorMessage = "Kwota „Od” nie może być większa niż kwota „Do”.";
				}

				if (FundFilterRequest.AmountFrom != null &&
					!CheckAmountFilter((decimal)FundFilterRequest.AmountFrom))
				{
					FilterEntryColorFrom =
						(Color)Application.Current.Resources["Negative"];

					errorMessage =
						"Kwota „Od” ma niepoprawny format (00.00, max 15 cyfr przed przecinkiem).";
				}

				if (FundFilterRequest.AmountTo != null &&
					!CheckAmountFilter((decimal)FundFilterRequest.AmountTo))
				{
					FilterEntryColorTo =
						(Color)Application.Current.Resources["Negative"];

					errorMessage =
						"Kwota „Do” ma niepoprawny format (00.00, max 15 cyfr przed przecinkiem).";
				}

				if (!string.IsNullOrEmpty(errorMessage))
				{
					_logger.LogWarning("Błąd walidacji filtrów: {ErrorMessage}", errorMessage);

					FilterErrorText = errorMessage;
					ShowFilterErrorMessage = true;
					return;
				}

				FiltersVisible = false;
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;

				var request = FundFilterRequest.Clone();

				var response = await _fundService.Get10(
					request,
					useDatesFromToo: UseDateFilter
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie funduszy po filtrze nie powiodło się. StatusCode={StatusCode}",
						response.StatusCode
					);

					ShowErrorMessage = true;
					return;
				}

				var fundResponse =
					_jsonService.Deserialize<ObservableCollection<FundResponse>>(
						await response.Content.ReadAsStringAsync()
					);

				_mapper.Map(fundResponse, Funds);

				_logger.LogInformation(
					"Filtrowanie zakończone sukcesem. Liczba funduszy: {Count}",
					Funds.Count
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas filtrowania funduszy.");
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

		private async Task ShowMore()
		{
			_logger.LogInformation("Pobieranie kolejnych funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!Funds.Any())
				{
					_logger.LogWarning("Brak funduszy – pomijam ładowanie kolejnych.");
					return;
				}

				var request = FundFilterRequest.Clone();
				request.LastDate = Funds.Last().CreationDate;

				var response = await _fundService.Get10(request);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie kolejnych funduszy nie powiodło się. StatusCode={StatusCode}",
						response.StatusCode
					);

					ShowErrorMessage = true;
					return;
				}

				int oldCount = Funds.Count;

				var fundResponse =
					_jsonService.Deserialize<ObservableCollection<FundResponse>>(
						await response.Content.ReadAsStringAsync()
					);

				var nextFunds = _mapper.Map<ObservableCollection<Fund>>(fundResponse);

				foreach (var fund in nextFunds.OrderByDescending(f => f.CreationDate))
				{
					Funds.Add(fund);
				}

				EnableShowMore = !(Funds.Count % 10 != 0 || Funds.Count == oldCount);

				_logger.LogInformation(
					"Dodano {Added} funduszy. Łącznie: {Total}. EnableShowMore={EnableShowMore}",
					nextFunds.Count,
					Funds.Count,
					EnableShowMore
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas pobierania kolejnych funduszy.");
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


		private bool CheckDateFilter()
		{
			_logger.LogInformation(
				"Walidacja zakresu dat. DateFrom={DateFrom}, DateTo={DateTo}",
				FundFilterRequest.DateFrom,
				FundFilterRequest.DateTo
			);

			return FundFilterRequest.DateFrom <= FundFilterRequest.DateTo;
		}

		private bool CheckAmountFilter(decimal amount)
		{
			if (amount <= 0)
				return false;

			string value = amount.ToString(CultureInfo.InvariantCulture);
			var parts = value.Split('.');

			return parts[0].Length <= 15 && (parts.Length == 1 || parts[1].Length <= 2);
		}

		private bool CheckShouldBe()
		{
			return CheckAmountFilter((decimal)FundCategoryRequest.ShouldBe);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}