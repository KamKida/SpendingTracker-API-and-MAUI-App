using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.FundsPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	public class FundsHistoryPageViewModel : INotifyPropertyChanged
	{
		private FundFilterRequest _filterRequest = new FundFilterRequest();
		private ObservableCollection<Fund> _funds = new ObservableCollection<Fund>();

		private readonly JsonService _jsonService;
		private readonly IFundService _fundService;
		private readonly IMapper _mapper;
		private readonly ILogger<FundsHistoryPageViewModel> _logger;

		private bool _filtersVisible;
		private bool _showErrorMessage;
		private bool _enableFilters;
		private bool _enableShowMore;
		private bool _useDateFilter;
		private bool _showFilterErrorMessage;

		private string _filterErrorText;
		private Color _filterEntryColorFrom;
		private Color _filterEntryColorTo;
		private Color _dateColor;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

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

		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand DeleteFundCommand { get; }
		public ICommand ShowMoreCommand { get; }
		public ICommand GoToAddFundPageCommand { get; }
		public ICommand GoToEditFundPageCommand { get; }

		public FundsHistoryPageViewModel(
			JsonService jsonService,
			IFundService fundService,
			IMapper mapper,
			ILogger<FundsHistoryPageViewModel> logger)
		{
			_jsonService = jsonService;
			_fundService = fundService;
			_mapper = mapper;
			_logger = logger;

			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			DeleteFundCommand = new Command<Fund>(async (fund) => await DeleteFund(fund));
			ShowMoreCommand = new Command(async () => await ShowMore());
			GoToAddFundPageCommand = new Command(async () => await GoToAddFundPage());
			GoToEditFundPageCommand = new Command<Fund>(async (fund) => await GoToEditFundPage(fund));
		}

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o funduszach.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			FundFilterRequest.Reset();
			DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
			EnableFilters = true;
			UseDateFilter = false;

			FiltersVisible = false;
			ShowFilterErrorMessage = false;
			FilterErrorText = string.Empty;

			try
			{
				var response = await _fundService.Get10(FundFilterRequest);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji o funduszach: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie funduszy nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					EnableFilters = false;
					EnableShowMore = false;
					return;
				}

				ShowErrorMessage = false;

				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(fundResponse, Funds);

				EnableShowMore = (Funds.Count % 10 == 0);

				_logger.LogInformation(
					"Pobrano i zmapowano {Count} funduszy do kolekcji Funds.",
					fundResponse.Count
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania podstawowych informacji o funduszach.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania podstawowych informacji o funduszach.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces pobierania podstawowych informacji o funduszach.");
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

				Funds.Remove(fund);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas usuwania funduszu. FundId={FundId}", fund.Id);
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas usuwania funduszu. FundId={FundId}", fund.Id);
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces usuwania funduszu. FundId={FundId}", fund.Id);
			}
		}

		private async Task GoToAddFundPage()
		{
			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania funduszu.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync(nameof(AddFundPage));

				_logger.LogInformation("Nawigacja do strony dodawania funduszu zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas nawigacji do strony dodawania funduszu.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task GoToEditFundPage(Fund fund)
		{
			_logger.LogInformation(
				"Rozpoczynam nawigację do strony edycji funduszu. FundId={FundId}, Amount={Amount}, CreationDate={CreationDate}",
				fund.Id,
				fund.Amount,
				fund.CreationDate
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync($"{nameof(EditFundPage)}", new Dictionary<string, object>
				{
					[nameof(Fund)] = fund
				});

				_logger.LogInformation(
					"Nawigacja do strony edycji funduszu zakończona sukcesem. FundId={FundId}",
					fund.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas nawigacji do strony edycji funduszu. FundId={FundId}", fund.Id);
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

				_logger.LogInformation("Wynik pobierania kolejnych funduszy: StatusCode={StatusCode}", response.StatusCode);

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

		private async Task ShowHideFilters()
		{
			_logger.LogInformation("Rozpoczynam pokazywanie/ukrywanie filtrów.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FiltersVisible = !FiltersVisible;

				_logger.LogInformation("Filtry są widoczne: {FiltersVisible}", FiltersVisible);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pokazywania/ukrywania filtrów.");
				throw;
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
			_logger.LogInformation("Rozpoczynam czyszczenie filtrów funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;
				await Reset();

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
					errorMessage = "Data 'Od' nie może być większa niż data 'Do'.";
					_logger.LogWarning(errorMessage + " DateFrom={DateFrom}, DateTo={DateTo}",
						FundFilterRequest.DateFrom, FundFilterRequest.DateTo);
				}
				else
				{
					DateColor = Colors.White;
				}

				if (FundFilterRequest.AmountFrom != null && FundFilterRequest.AmountTo != null && FundFilterRequest.AmountFrom > FundFilterRequest.AmountTo)
				{
					FilterEntryColorFrom = FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' nie może być większa od kwoty 'Do'.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}, AmountTo={AmountTo}",
						FundFilterRequest.AmountFrom, FundFilterRequest.AmountTo);
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

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
 