using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.LoginPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundCategoryViewModels
{
	[QueryProperty(nameof(Domain.Models.FundCategory), nameof(Domain.Models.FundCategory))]
	public class EditFundCategoryPageViewModel : INotifyPropertyChanged
	{
		private User _user;
		private FundCategoryRequest _fundCategoryRequest = new FundCategoryRequest();
		private JsonService _jsonService;
		private IFundCategoryService _fundCategoryService;
		private IFundService _fundService;
		private IMapper _mapper;
		private ILogger<EditFundCategoryPageViewModel> _logger;

		private string _message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa Wymagana";
		private Color _messageColor = (Color)Application.Current.Resources["Positive"];
		private Color _shouldBeEntryColor = Colors.White;
		public bool _showLoadingIcon = false;
		public bool _runLoadingIcon = false;

		public bool _blockInteraction = false;

		public ObservableCollection<Fund> _funds = new ObservableCollection<Fund>();

		private bool _showErrorMessage = false;
		private bool _filtersVisible = false;
		public Color _dateColor = Colors.White;
		private Color _filterEntryColorFrom = Colors.White;
		private Color _filterEntryColorTo = Colors.White;
		private bool _useDateFilter = false;
		private bool _enableShowMore = true;
		private bool _enableFilters = true;

		private bool _showFilterErrorMessage = false;
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

		private FundFilterRequest _filterRequest = new FundFilterRequest();

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

		private FundCategory _fundCategory;
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

		public int DescriptionCount
		{
			get => FundCategoryRequest.Description?.Length ?? 0;
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


		public EditFundCategoryPageViewModel(
		User user,
		JsonService jsonService,
		IFundCategoryService fundCategoryService,
		IFundService fundService,
		IMapper mapper,
		ILogger<EditFundCategoryPageViewModel> logger)
		{
			_user = user;
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
			DeleteFundCommand = new Command<Fund>(async (fund) => await DeleteFund(fund));
			ShowMoreCommand = new Command(async () => await ShowMore());
		}
		public ICommand EditFundCategoryCommand { get; }
		public ICommand CancelEditCommand { get; }

		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand DeleteFundCommand { get; }
		public ICommand ShowMoreCommand { get; }
		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie formularza kategorii funduszu oraz stanu błędów UI.");

			FundCategoryRequest = new FundCategoryRequest() { Name = FundCategory.Name, Description = FundCategory.Description };
			Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby dodatnie. Nazwa wymagana.";
			MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Positive"];
			FundFilterRequest.FundCategoryId = FundCategory.Id;
			await SetBaseFundsInfo();
			_logger.LogInformation("Zakończono resetowanie formularza kategorii funduszu oraz stanu błędów UI.");
		}

		public async Task EditFundCategory()
		{
			_logger.LogInformation(
				"Rozpoczynam proces edycji kategorii funduszu. FundCategoryId={FundCategoryId}, OldShouldBe={OldShouldBe}, NewShouldBe={NewShouldBe}",
				FundCategory.Id,
				FundCategory.ShouldBe,
				FundCategoryRequest.ShouldBe
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (String.IsNullOrEmpty(FundCategoryRequest.Name))
				{
					_logger.LogWarning(
						"Brak nazwy kategori. Name={Name}, ShouldBe={ShouldBe}",
						FundCategoryRequest.Name,
						FundCategoryRequest.ShouldBe
					);
					Message = "Nazwa jest wymagana.";
					MessageColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				bool isShouldBeValid = CheckShouldBe();
				if (!isShouldBeValid)
				{
					_logger.LogWarning(
						"Walidacja wartości ShouldBe dla kategorii funduszu nie powiodła się. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}",
						FundCategory.Id,
						FundCategoryRequest.ShouldBe
					);

					MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Negative"];
					Message = "Wartość powinno być jest niepoprawna.";
					return;
				}

				FundCategoryRequest.Id = FundCategory.Id;

				var response = await _fundCategoryService.EditFundCategory(FundCategoryRequest);

				_logger.LogInformation(
					"Wynik edycji kategorii funduszu: FundCategoryId={FundCategoryId}, StatusCode={StatusCode}",
					FundCategory.Id,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Edycja kategorii funduszu nie powiodła się. FundCategoryId={FundCategoryId}, StatusCode={StatusCode}, Content={Content}",
						FundCategory.Id,
						response.StatusCode,
						content
					);

					FundCategory.Name = FundCategoryRequest.Name;
					if (FundCategory.ShouldBe != FundCategoryRequest.ShouldBe)
					{
						FundCategory.ShouldBe = FundCategoryRequest.ShouldBe;
					}

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak. Spróbuj ponownie.";
					return;
				}

				FundCategory.ShouldBe = FundCategoryRequest.ShouldBe;

				MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Positive"];
				Message = "Kategoria funduszu została zaktualizowana pomyślnie.";

				_logger.LogInformation(
					"Edycja kategorii funduszu zakończona sukcesem. FundCategoryId={FundCategoryId}, NewShouldBe={NewShouldBe}",
					FundCategory.Id,
					FundCategoryRequest.ShouldBe
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}",
					FundCategory.Id,
					FundCategoryRequest.ShouldBe
				);

				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}",
					FundCategory.Id,
					FundCategoryRequest.ShouldBe
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
					"Zakończono proces edycji kategorii funduszu. FundCategoryId={FundCategoryId}",
					FundCategory.Id
				);
			}
		}

		private bool CheckShouldBe()
		{
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie kwoty kategorii funduszy. ShouldBe={ShouldBe}",
				FundCategoryRequest.ShouldBe
			);

			if (FundCategoryRequest.ShouldBe <= 0)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy jest mniejsza lub równa zero. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			string amountStr = FundCategoryRequest.ShouldBe.ToString();
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy przekracza 15 cyfr przed przecinkiem. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy przekracza 2 miejsca po przecinku. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota kategorii funduszy jest poprawna. ShouldBe={ShouldBe}",
				FundCategoryRequest.ShouldBe
			);

			return true;
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation(
				"Rozpoczynam anulowanie edycji funduszu (powrót do historii funduszy). FundId={FundId}, Amount={Amount}",
				FundCategory?.Id,
				FundCategory?.ShouldBe
			);

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation(
					"Nawigacja po anulowaniu edycji funduszu zakończona sukcesem. FundId={FundId}",
					FundCategory?.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas anulowania edycji funduszu. FundId={FundId}",
					FundCategory?.Id
				);
				throw;
			}
		}

		private async Task DeleteFund(Fund fund)
		{
			_logger.LogInformation(
				"Rozpoczynam proces usuwania funduszu. FundId={FundId}, Amount={Amount}, CreationDate={CreationDate}",
				fund.Id,
				fund.Amount,
				fund.CreationDate
			);

			bool answer = await Application.Current.MainPage.DisplayAlert(
				"",
				$"Czy na pewno usunąć fundusz z kwotą: {fund.Amount} zł. \n Dodanego: {fund.CreationDate} ?",
				"Tak",
				"Nie"
			);

			if (!answer)
			{
				_logger.LogInformation(
					"Usuwanie funduszu anulowane przez użytkownika. FundId={FundId}",
					fund.Id
				);
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
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Usuwanie funduszu nie powiodło się. FundId={FundId}, StatusCode={StatusCode}, Content={Content}",
						fund.Id,
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					return;
				}

				_logger.LogInformation(
					"Fundusz usunięty pomyślnie. FundId={FundId}, Amount={Amount}",
					fund.Id,
					fund.Amount
				);

				_user.ThisMonthFund -= fund.Amount;
				Funds.Remove(fund);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania funduszu. FundId={FundId}",
					fund.Id
				);
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania funduszu. FundId={FundId}",
					fund.Id
				);
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces usuwania funduszu. FundId={FundId}",
					fund.Id
				);
			}
		}
		private async Task ShowHideFilters()
		{
			_logger.LogInformation("Rozpoczynam pokazywanie filtrów.");

			if (!FiltersVisible)
				FiltersVisible = true;

			else FiltersVisible = false;

			_logger.LogInformation("Filtry są widoczne. {FiltersVisible}", FiltersVisible);
		}
		private async Task GoToAddFundPage()
		{
			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania funduszu.");

			try
			{
				await Shell.Current.GoToAsync(nameof(AddFundPage));

				_logger.LogInformation("Nawigacja do strony dodawania funduszu zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony dodawania funduszu."
				);
				throw;
			}
		}
		private async Task ResetFilter()
		{
			_logger.LogInformation("Rozpoczynam czyszczenie filtrów funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;
			try
			{
				FundFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;
				await SetBaseFundsInfo();

				_logger.LogInformation("Filtry funduszy zostały wyczyszczone.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas czyszczenia filtrów funduszy.");
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
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o funduszach.");

			FundFilterRequest.Reset();
			DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
			UseDateFilter = false;

			try
			{
				var response = await _fundService.Get10(FundFilterRequest);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji o funduszach: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie funduszy nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						await response.Content.ReadAsStringAsync()
					);

					ShowErrorMessage = true;
					EnableFilters = false;
					EnableShowMore = false;
					return;
				}

				ShowErrorMessage = false;

				var content = await response.Content.ReadAsStringAsync();
				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundResponse>>(content);

				_mapper.Map(fundResponse, Funds);

				if (Funds.Count % 10 != 0)
				{
					EnableShowMore = false;
				}
				else
				{
					EnableShowMore = true;
				}

				_logger.LogInformation(
					"Pobrano i zmapowano {Count} funduszy do kolekcji Funds.",
					fundResponse.Count
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania podstawowych informacji o funduszach."
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania podstawowych informacji o funduszach."
				);
				throw;
			}
			finally
			{
				_logger.LogInformation("Zakończono proces pobierania podstawowych informacji o funduszach.");
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
							FundFilterRequest.DateFrom, FundFilterRequest.DateTo);
					}
					else
					{
						DateColor = Colors.White;
					}
				}


				if (FundFilterRequest.AmountFrom != null && FundFilterRequest.AmountTo != null)
				{
					if (FundFilterRequest.AmountFrom > FundFilterRequest.AmountTo)
					{
						FilterEntryColorFrom = FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
						errorMessage = "Kwota 'Od' nie może być większa od kwoty 'Do'.";
						_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}, AmountTo={AmountTo}",
							FundFilterRequest.AmountFrom, FundFilterRequest.AmountTo);
					}
					else
					{
						FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
					}
				}

				if (FundFilterRequest.AmountFrom != null && !CheckAmountFilter((decimal)FundFilterRequest.AmountFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}", FundFilterRequest.AmountFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (FundFilterRequest.AmountTo != null && !CheckAmountFilter((decimal)FundFilterRequest.AmountTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountTo={AmountTo}", FundFilterRequest.AmountTo);
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

				var request = FundFilterRequest.Clone();

				var response = await _fundService.Get10(request, useDatesFromToo: UseDateFilter);
				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie funduszy po filtrze nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);
					ShowErrorMessage = true;
					return;
				}

				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(fundResponse, Funds);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas filtrowania funduszy.");
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
				FundFilterRequest.DateFrom,
				FundFilterRequest.DateTo
			);


			if (FundFilterRequest.DateFrom > FundFilterRequest.DateTo)
			{
				_logger.LogWarning(
					"Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})",
					FundFilterRequest.DateFrom,
					FundFilterRequest.DateTo
				);
				return false;
			}

			_logger.LogInformation(
				"Filtr dat jest poprawny. DateFrom={DateFrom}, DateTo={DateTo}",
				FundFilterRequest.DateFrom,
				FundFilterRequest.DateTo
			);

			return true;
		}


		private bool CheckAmountFilter(decimal amount)
		{
			_logger.LogInformation(
					"Rozpoczynam sprawdzanie kwoty funduszu. Amount={Amount}",
					amount
				);

			if (amount <= 0)
			{
				_logger.LogWarning(
					"Kwota funduszu jest mniejsza lub równa zero. Amount={Amount}",
					amount
				);
				return false;
			}

			string amountStr = amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota funduszu przekracza 15 cyfr przed przecinkiem. Amount={Amount}",
					amount
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota funduszu przekracza 2 miejsca po przecinku. Amount={Amount}",
					amount
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota funduszu jest poprawna. Amount={Amount}",
				amount
			);

			return true;
		}

		private async Task ShowMore()
		{
			_logger.LogInformation("Rozpoczynam ładowanie kolejnych funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!Funds.Any())
				{
					_logger.LogWarning("Brak funduszy do załadowania kolejnych.");
					return;
				}

				var request = FundFilterRequest.Clone();
				request.LastDate = Funds.Last().CreationDate;

				var response = await _fundService.Get10(request);

				_logger.LogInformation(
					"Wynik pobierania kolejnych funduszy: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie kolejnych funduszy nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					return;
				}

				int oldFundCount = Funds.Count;

				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());
				var next10Funds = _mapper.Map<ObservableCollection<Fund>>(fundResponse);

				foreach (var fund in next10Funds.OrderByDescending(f => f.CreationDate))
				{
					Funds.Add(fund);
				}

				EnableShowMore = !(Funds.Count % 10 != 0 || oldFundCount == Funds.Count);

				_logger.LogInformation(
					"Pobrano {NewFunds} kolejnych funduszy. Łączna liczba funduszy: {TotalFunds}. EnableShowMore={EnableShowMore}",
					next10Funds.Count,
					Funds.Count,
					EnableShowMore
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania kolejnych funduszy.");
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania kolejnych funduszy.");
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces ładowania kolejnych funduszy.");
			}
		}

		
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}

