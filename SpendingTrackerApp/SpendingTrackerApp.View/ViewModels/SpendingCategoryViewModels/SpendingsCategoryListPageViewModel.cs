using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.SpendingCategoryPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.SpendingCategoryViewModels
{
	public class SpendingCategoryListPageViewModel : INotifyPropertyChanged
	{
		private SpendingCategoryFilterRequest _filterRequest = new SpendingCategoryFilterRequest();
		private ObservableCollection<SpendingCategory> _spendingCategories;
		private JsonService _jsonService;
		private ISpendingCategoryService _spendingService;
		private IMapper _mapper;
		private ILogger<SpendingCategoryListPageViewModel> _logger;

		private bool _filtersVisible = false;

		private bool _showErrorMessage = false;
		private bool _enableFilters = true;
		private bool _enableShowMore = true;

		private bool _useDateFilter = false;

		private bool _showFilterErrorMessage = false;
		private string _filterErrorText;

		private Color _filterEntryColorFrom = Colors.White;
		private Color _filterEntryColorTo = Colors.White;

		private bool _showLoadingIcon = false;
		private bool _runLoadingIcon = false;

		private bool _blockInteraction = false;

		private Color _dateColor = Colors.White;

		private bool _showCategories = false;

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

		public SpendingCategoryFilterRequest SpendingCategoryFilterRequest
		{
			get => _filterRequest;
			set
			{
				if (_filterRequest != value)
				{
					_filterRequest = value;
					OnPropertyChanged(nameof(SpendingCategoryFilterRequest));
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

		public SpendingCategoryListPageViewModel(
			JsonService jsonService,
			ISpendingCategoryService spendingService,
			IMapper mapper,
			ILogger<SpendingCategoryListPageViewModel> logger)
		{
			_jsonService = jsonService;
			_spendingService = spendingService;
			_mapper = mapper;
			_logger = logger;

			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			OpenDateCommand = new Command<DatePicker>(async (picker) => await OpenDate(picker));
			DeleteSpendingCategoryCommand = new Command<SpendingCategory>(async (sCategory) => await DeleteSpendingCategory(sCategory));
			ShowMoreCommand = new Command(async () => await ShowMore());
			GoToAddSpendingCategoryPageCommand = new Command(async () => await GoToAddSpendingCategoryPage());
			GoToEditSpendingCategoryPageCommand = new Command<SpendingCategory>(async (sCategory) => await GoToEditSpendingCategoryPage(sCategory));
		}

		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand OpenDateCommand { get; }
		public ICommand DeleteSpendingCategoryCommand { get; }
		public ICommand ShowMoreCommand { get; }
		public ICommand GoToAddSpendingCategoryPageCommand { get; }
		public ICommand GoToEditSpendingCategoryPageCommand { get; }

		public async Task SetBaseInfo()
		{
			SpendingCategories = new ObservableCollection<SpendingCategory>();
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o kategoriach wydatków.");

			SpendingCategoryFilterRequest.Reset();
			DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
			UseDateFilter = false;

			try
			{
				var response = await _spendingService.Get10(SpendingCategoryFilterRequest);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji o kategoriach wydatków: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie kategorii wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}",
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
				var spendingCategoryResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(content);

				_mapper.Map(spendingCategoryResponse, SpendingCategories);

				if (SpendingCategories.Count % 10 != 0)
				{
					EnableShowMore = false;
				}
				else
				{
					EnableShowMore = true;
				}

				_logger.LogInformation(
					"Pobrano i zmapowano {Count} kategorii wydatków do kolekcji SpendingCategories.",
					spendingCategoryResponse.Count
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania podstawowych informacji o kategoriach wydatków."
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania podstawowych informacji o kategoriach wydatków."
				);
				throw;
			}
			finally
			{
				_logger.LogInformation("Zakończono proces pobierania podstawowych informacji o kategoriach wydatków.");
			}
		}

		private async Task DeleteSpendingCategory(SpendingCategory spendingCategory)
		{
			_logger.LogInformation(
				"Rozpoczynam proces usuwania kategorii wydatków. SpendingCategoryId={SpendingCategoryId}, Name={Name}, CreationDate={CreationDate}",
				spendingCategory.Id,
				spendingCategory.Name,
				spendingCategory.CreationDate
			);

			bool answer = await Application.Current.MainPage.DisplayAlert(
				"",
				$"Czy na pewno usunąć kategorię wydatków o nazwie: {spendingCategory.Name}?\nDodana: {spendingCategory.CreationDate}",
				"Tak",
				"Nie"
			);

			if (!answer)
			{
				_logger.LogInformation(
					"Usuwanie kategorii wydatków anulowane przez użytkownika. SpendingCategoryId={SpendingCategoryId}",
					spendingCategory.Id
				);
				return;
			}

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _spendingService.DeleteSpendingCategory(spendingCategory.Id);

				_logger.LogInformation(
					"Wynik usuwania kategorii wydatków: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Usuwanie kategorii wydatków nie powiodło się. SpendingCategoryId={SpendingCategoryId}, StatusCode={StatusCode}, Content={Content}",
						spendingCategory.Id,
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					return;
				}

				_logger.LogInformation(
					"Kategoria wydatków została pomyślnie usunięta. SpendingCategoryId={SpendingCategoryId}, Name={Name}",
					spendingCategory.Id,
					spendingCategory.Name
				);

				SpendingCategories.Remove(spendingCategory);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
					spendingCategory.Id
				);
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
					spendingCategory.Id
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
					"Zakończono proces usuwania kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
					spendingCategory.Id
				);
			}
		}

		private async Task ShowMore()
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

				int oldSpendingCount = SpendingCategories.Count;

				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await response.Content.ReadAsStringAsync());
				var next10Spendings = _mapper.Map<ObservableCollection<SpendingCategory>>(spendingResponse);

				foreach (var spending in next10Spendings.OrderByDescending(f => f.CreationDate))
				{
					SpendingCategories.Add(spending);
				}

				EnableShowMore = !(SpendingCategories.Count % 10 != 0 || oldSpendingCount == SpendingCategories.Count);

				_logger.LogInformation(
					"Pobrano {NewSpendings} kolejnych wydatków. Łączna liczba wydatków: {TotalSpendings}. EnableShowMore={EnableShowMore}",
					next10Spendings.Count,
					SpendingCategories.Count,
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

		private async Task GoToAddSpendingCategoryPage()
		{
			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania kategorii wydatków.");

			try
			{
				await Shell.Current.GoToAsync(nameof(AddSpendingCategoryPage));

				_logger.LogInformation("Nawigacja do strony dodawania kategorii wydatków zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony dodawania kategorii wydatków."
				);
				throw;
			}
		}

		private async Task GoToEditSpendingCategoryPage(SpendingCategory spendingCategory)
		{
			_logger.LogInformation(
				"Rozpoczynam nawigację do strony edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}, Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}, CreationDate={CreationDate}",
				spendingCategory.Id,
				spendingCategory.Name,
				spendingCategory.WeeklyLimit,
				spendingCategory.MonthlyLimit,
				spendingCategory.CreationDate
			);

			try
			{
				await Shell.Current.GoToAsync(
					nameof(EditSpendingCategoryPage),
					new Dictionary<string, object>
					{
						[nameof(SpendingCategory)] = spendingCategory
					}
				);

				_logger.LogInformation(
					"Nawigacja do strony edycji kategorii wydatków zakończona sukcesem. SpendingCategoryId={SpendingCategoryId}",
					spendingCategory.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
					spendingCategory.Id
				);
				throw;
			}
		}

		private async Task ShowHideFilters()
		{
			_logger.LogInformation("Rozpoczynam pokazywanie filtrów.");

			if (!FiltersVisible)
				FiltersVisible = true;
			else
				FiltersVisible = false;

			_logger.LogInformation("Filtry są widoczne. {FiltersVisible}", FiltersVisible);
		}

		private async Task ResetFilter()
		{
			_logger.LogInformation("Rozpoczynam czyszczenie filtrów wydatków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;
			try
			{
				SpendingCategoryFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;
				await SetBaseInfo();

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

		private async Task OpenDate(DatePicker picker)
		{
			if (picker == null)
				return;

			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				picker.Focus();
			});
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

				var response = await _spendingService.Get10(request, useDatesFromToo: UseDateFilter);
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

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
