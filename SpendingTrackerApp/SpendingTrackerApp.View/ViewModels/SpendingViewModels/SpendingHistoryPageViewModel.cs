using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.SpendingPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.SpendingViewModels
{
	public class SpendingHistoryPageViewModel : INotifyPropertyChanged
	{
		private SpendingFilterRequest _filterRequest = new SpendingFilterRequest();
		private ObservableCollection<Spending> _spendings = new ObservableCollection<Spending>();

		private readonly JsonService _jsonService;
		private readonly ISpendingService _spendingService;
		private readonly IMapper _mapper;
		private readonly ILogger<SpendingHistoryPageViewModel> _logger;

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

		public SpendingHistoryPageViewModel(
			JsonService jsonService,
			ISpendingService spendingService,
			IMapper mapper,
			ILogger<SpendingHistoryPageViewModel> logger)
		{
			_jsonService = jsonService;
			_spendingService = spendingService;
			_mapper = mapper;
			_logger = logger;

			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			DeleteSpendingCommand = new Command<Spending>(async (spending) => await DeleteSpending(spending));
			ShowMoreCommand = new Command(async () => await ShowMore());
			GoToAddSpendingPageCommand = new Command(async () => await GoToAddSpendingPage());
			GoToEditSpendingPageCommand = new Command<Spending>(async (spending) => await GoToEditSpendingPage(spending));
		}

		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand DeleteSpendingCommand { get; }
		public ICommand ShowMoreCommand { get; }
		public ICommand GoToAddSpendingPageCommand { get; }
		public ICommand GoToEditSpendingPageCommand { get; }

		public async Task Reset()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o wydatkach.");

			SpendingFilterRequest.Reset();
			DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
			UseDateFilter = false;

			FiltersVisible = false;
			ShowErrorMessage = false;
			EnableFilters = true;
			EnableShowMore = true;
			ShowFilterErrorMessage = false;
			FilterErrorText = string.Empty;

			try
			{
				var response = await _spendingService.Get10(SpendingFilterRequest);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji o wydatkach: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					EnableFilters = false;
					EnableShowMore = false;
					return;
				}

				ShowErrorMessage = false;

				var contentString = await response.Content.ReadAsStringAsync();
				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingResponse>>(contentString);

				_mapper.Map(spendingResponse, Spendings);

				EnableShowMore = Spendings.Count % 10 == 0;

				_logger.LogInformation(
					"Pobrano i zmapowano {Count} wydatków do kolekcji Spendings.",
					spendingResponse.Count
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania podstawowych informacji o wydatkach."
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania podstawowych informacji o wydatkach."
				);
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces pobierania podstawowych informacji o wydatkach.");
			}
		}


		private async Task DeleteSpending(Spending spending)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam proces usuwania wydatku. SpendingId={SpendingId}, Amount={Amount}, CreationDate={CreationDate}",
				spending.Id,
				spending.Amount,
				spending.CreationDate
			);

			bool answer = await Application.Current.MainPage.DisplayAlert(
				"",
				$"Czy na pewno usunąć wydatek z kwotą: {spending.Amount} zł. \n Dodanego: {spending.CreationDate} ?",
				"Tak",
				"Nie"
			);

			if (!answer)
			{
				_logger.LogInformation(
					"Usuwanie wydatku anulowane przez użytkownika. SpendingId={SpendingId}",
					spending.Id
				);

				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
				return;
			}

			try
			{
				var response = await _spendingService.DeleteSpending(spending.Id);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Usuwanie wydatku nie powiodło się. SpendingId={SpendingId}, StatusCode={StatusCode}, Content={Content}",
						spending.Id,
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					return;
				}

				_logger.LogInformation(
					"Wydatki usunięte pomyślnie. SpendingId={SpendingId}, Amount={Amount}",
					spending.Id,
					spending.Amount
				);

				Spendings.Remove(spending);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania wydatku. SpendingId={SpendingId}",
					spending.Id
				);
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania wydatku. SpendingId={SpendingId}",
					spending.Id
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
					"Zakończono proces usuwania wydatku. SpendingId={SpendingId}",
					spending.Id
				);
			}
		}

		private async Task GoToAddSpendingPage()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania wydatku.");

			try
			{
				await Shell.Current.GoToAsync(nameof(AddSpendingPage));

				_logger.LogInformation("Nawigacja do strony dodawania wydatku zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony dodawania wydatku."
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

		private async Task ShowMore()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam ładowanie kolejnych wydatków.");

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

				_logger.LogInformation(
					"Wynik pobierania kolejnych wydatków: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie kolejnych wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					return;
				}

				int oldSpendingCount = Spendings.Count;

				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingResponse>>(await response.Content.ReadAsStringAsync());
				var next10Spendings = _mapper.Map<ObservableCollection<Spending>>(spendingResponse);

				foreach (var spending in next10Spendings.OrderByDescending(f => f.CreationDate))
				{
					Spendings.Add(spending);
				}

				EnableShowMore = !(Spendings.Count % 10 != 0 || oldSpendingCount == Spendings.Count);

				_logger.LogInformation(
					"Pobrano {NewSpendings} kolejnych wydatków. Łączna liczba wydatków: {TotalSpendings}. EnableShowMore={EnableShowMore}",
					next10Spendings.Count,
					Spendings.Count,
					EnableShowMore
				);
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

		private async Task GoToEditSpendingPage(Spending spending)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam nawigację do strony edycji wydatku. SpendingId={SpendingId}, Amount={Amount}, CreationDate={CreationDate}",
				spending.Id,
				spending.Amount,
				spending.CreationDate
			);

			try
			{
				await Shell.Current.GoToAsync($"{nameof(EditSpendingPage)}", new Dictionary<string, object>
				{
					[nameof(Spending)] = spending
				});

				_logger.LogInformation(
					"Nawigacja do strony edycji wydatku zakończona sukcesem. SpendingId={SpendingId}",
					spending.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony edycji wydatku. SpendingId={SpendingId}",
					spending.Id
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

		private async Task ShowHideFilters()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam pokazywanie filtrów.");

			try
			{
				FiltersVisible = !FiltersVisible;
				_logger.LogInformation("Filtry są widoczne: {FiltersVisible}", FiltersVisible);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pokazywania filtrów.");
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
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam czyszczenie filtrów wydatków.");

			try
			{
				SpendingFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;
				await Reset();

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

		private async Task Filter()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam filtrowanie wydatków.");

			try
			{
				ShowFilterErrorMessage = false;
				string errorMessage = null;

				if (UseDateFilter && !CheckDateFilter())
				{
					DateColor = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Data 'Od' nie może być większa niż data 'Do'.";
					_logger.LogWarning(errorMessage + " DateFrom={DateFrom}, DateTo={DateTo}",
						SpendingFilterRequest.DateFrom, SpendingFilterRequest.DateTo);
				}
				else
				{
					DateColor = Colors.White;
				}

				if (SpendingFilterRequest.AmountFrom != null && SpendingFilterRequest.AmountTo != null)
				{
					if (SpendingFilterRequest.AmountFrom > SpendingFilterRequest.AmountTo)
					{
						FilterEntryColorFrom = FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
						errorMessage = "Kwota 'Od' nie może być większa od kwoty 'Do'.";
						_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}, AmountTo={AmountTo}",
							SpendingFilterRequest.AmountFrom, SpendingFilterRequest.AmountTo);
					}
					else
					{
						FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
					}
				}

				if (SpendingFilterRequest.AmountFrom != null && !CheckAmountFilter((decimal)SpendingFilterRequest.AmountFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}", SpendingFilterRequest.AmountFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (SpendingFilterRequest.AmountTo != null && !CheckAmountFilter((decimal)SpendingFilterRequest.AmountTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountTo={AmountTo}", SpendingFilterRequest.AmountTo);
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

				var request = SpendingFilterRequest.Clone();
				var response = await _spendingService.Get10(request, useDatesFromToo: UseDateFilter);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie wydatków po filtrze nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);
					ShowErrorMessage = true;
					return;
				}

				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(spendingResponse, Spendings);

				_logger.LogInformation("Filtrowanie wydatków zakończone sukcesem. Liczba wydatków: {Count}", Spendings.Count);
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
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie filtra dat. DateFrom={DateFrom}, DateTo={DateTo}",
				SpendingFilterRequest.DateFrom,
				SpendingFilterRequest.DateTo
			);

			if (SpendingFilterRequest.DateFrom > SpendingFilterRequest.DateTo)
			{
				_logger.LogWarning(
					"Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})",
					SpendingFilterRequest.DateFrom,
					SpendingFilterRequest.DateTo
				);
				return false;
			}

			_logger.LogInformation(
				"Filtr dat jest poprawny. DateFrom={DateFrom}, DateTo={DateTo}",
				SpendingFilterRequest.DateFrom,
				SpendingFilterRequest.DateTo
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

			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
