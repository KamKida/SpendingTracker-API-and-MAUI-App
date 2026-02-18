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
		private SpendingCategoryFilterRequest _filterRequest;
		private ObservableCollection<SpendingCategory> _spendingCategories;
		private readonly JsonService _jsonService;
		private readonly ISpendingCategoryService _spendingService;
		private readonly IMapper _mapper;
		private readonly ILogger<SpendingCategoryListPageViewModel> _logger;

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
		private bool _showCategories;

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

			_filterRequest = new SpendingCategoryFilterRequest();
			_spendingCategories = new ObservableCollection<SpendingCategory>();

			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			DeleteSpendingCategoryCommand = new Command<SpendingCategory>(async (sCategory) => await DeleteSpendingCategory(sCategory));
			ShowMoreCommand = new Command(async () => await ShowMore());
			GoToAddSpendingCategoryPageCommand = new Command(async () => await GoToAddSpendingCategoryPage());
			GoToEditSpendingCategoryPageCommand = new Command<SpendingCategory>(async (sCategory) => await GoToEditSpendingCategoryPage(sCategory));
		}

		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand DeleteSpendingCategoryCommand { get; }
		public ICommand ShowMoreCommand { get; }
		public ICommand GoToAddSpendingCategoryPageCommand { get; }
		public ICommand GoToEditSpendingCategoryPageCommand { get; }

		public async Task Reset()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o kategoriach wydatków.");

			SpendingCategoryFilterRequest.Reset();

			DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
			UseDateFilter = false;

			FiltersVisible = false;
			EnableFilters = true;
			EnableShowMore = true;
			ShowFilterErrorMessage = false;
			FilterErrorText = string.Empty;
			ShowCategories = false;

			try
			{
				var response = await _spendingService.Get10(SpendingCategoryFilterRequest);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji o kategoriach wydatków: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie kategorii wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);

					ShowErrorMessage = true;
					EnableFilters = false;
					EnableShowMore = false;
					return;
				}

				ShowErrorMessage = false;

				var contentResponse = await response.Content.ReadAsStringAsync();
				var spendingCategoryResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(contentResponse);

				_mapper.Map(spendingCategoryResponse, SpendingCategories);

				EnableShowMore = SpendingCategories.Count % 10 == 0;

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
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania podstawowych informacji o kategoriach wydatków."
				);
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces pobierania podstawowych informacji o kategoriach wydatków.");
			}
		}


		private async Task DeleteSpendingCategory(SpendingCategory spendingCategory)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

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
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
				return;
			}

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

				SpendingCategories.Remove(spendingCategory);

				_logger.LogInformation(
					"Kategoria wydatków została pomyślnie usunięta. SpendingCategoryId={SpendingCategoryId}, Name={Name}",
					spendingCategory.Id,
					spendingCategory.Name
				);
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
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam ładowanie kolejnych kategorii wydatków.");

			try
			{
				if (!SpendingCategories.Any())
				{
					_logger.LogWarning("Brak kategorii wydatków do załadowania kolejnych.");
					return;
				}

				var request = SpendingCategoryFilterRequest.Clone();
				request.LastDate = SpendingCategories.Last().CreationDate;

				var response = await _spendingService.Get10(request);

				_logger.LogInformation(
					"Wynik pobierania kolejnych kategorii wydatków: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Pobieranie kolejnych kategorii wydatków nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);
					ShowErrorMessage = true;
					return;
				}

				int oldCount = SpendingCategories.Count;

				var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingCategoryResponse>>(await response.Content.ReadAsStringAsync());
				var next10Spendings = _mapper.Map<ObservableCollection<SpendingCategory>>(spendingResponse);

				foreach (var spending in next10Spendings.OrderByDescending(f => f.CreationDate))
				{
					SpendingCategories.Add(spending);
				}

				EnableShowMore = !(SpendingCategories.Count % 10 != 0 || oldCount == SpendingCategories.Count);

				_logger.LogInformation(
					"Pobrano {NewSpendings} kolejnych kategorii wydatków. Łączna liczba: {TotalSpendings}. EnableShowMore={EnableShowMore}",
					next10Spendings.Count,
					SpendingCategories.Count,
					EnableShowMore
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania kolejnych kategorii wydatków.");
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania kolejnych kategorii wydatków.");
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono ładowanie kolejnych kategorii wydatków.");
			}
		}

		private async Task GoToAddSpendingCategoryPage()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania kategorii wydatków.");

			try
			{
				await Shell.Current.GoToAsync(nameof(AddSpendingCategoryPage));

				_logger.LogInformation("Nawigacja do strony dodawania kategorii wydatków zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas nawigacji do strony dodawania kategorii wydatków.");
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private async Task GoToEditSpendingCategoryPage(SpendingCategory spendingCategory)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam nawigację do strony edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}, Name={Name}",
				spendingCategory.Id,
				spendingCategory.Name
			);

			try
			{
				await Shell.Current.GoToAsync(
					$"{nameof(EditSpendingCategoryPage)}",
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
				_logger.LogError(ex, "Nieoczekiwany błąd podczas nawigacji do strony edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}", spendingCategory.Id);
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

			_logger.LogInformation("Rozpoczynam pokazywanie/ukrywanie filtrów.");

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
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam czyszczenie filtrów.");

			try
			{
				SpendingCategoryFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;

				await Reset();

				_logger.LogInformation("Filtry zostały wyczyszczone.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas czyszczenia filtrów.");
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

			_logger.LogInformation("Rozpoczynam filtrowanie kategorii wydatków.");

			try
			{
				ShowFilterErrorMessage = false;
				string errorMessage = null;

				if (UseDateFilter && !CheckDateFilter())
				{
					DateColor = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Data 'Od' nie może być większa niż data 'Do'.";
					_logger.LogWarning("{Error} DateFrom={DateFrom}, DateTo={DateTo}",
						errorMessage,
						SpendingCategoryFilterRequest.DateFrom,
						SpendingCategoryFilterRequest.DateTo);
				}
				else
				{
					DateColor = Colors.White;
				}

				if (SpendingCategoryFilterRequest.WeeklyLimitFrom.HasValue && !CheckAmountFilter(SpendingCategoryFilterRequest.WeeklyLimitFrom.Value))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning("{Error} WeeklyLimitFrom={WeeklyLimitFrom}", errorMessage, SpendingCategoryFilterRequest.WeeklyLimitFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (SpendingCategoryFilterRequest.WeeklyLimitTo.HasValue && !CheckAmountFilter(SpendingCategoryFilterRequest.WeeklyLimitTo.Value))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning("{Error} WeeklyLimitTo={WeeklyLimitTo}", errorMessage, SpendingCategoryFilterRequest.WeeklyLimitTo);
				}
				else
				{
					FilterEntryColorTo = Colors.White;
				}

				if (SpendingCategoryFilterRequest.MonthlyLimitFrom.HasValue && !CheckAmountFilter(SpendingCategoryFilterRequest.MonthlyLimitFrom.Value))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' limitu miesięcznego jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning("{Error} MonthlyLimitFrom={MonthlyLimitFrom}", errorMessage, SpendingCategoryFilterRequest.MonthlyLimitFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (SpendingCategoryFilterRequest.MonthlyLimitTo.HasValue && !CheckAmountFilter(SpendingCategoryFilterRequest.MonthlyLimitTo.Value))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' limitu miesięcznego jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning("{Error} MonthlyLimitTo={MonthlyLimitTo}", errorMessage, SpendingCategoryFilterRequest.MonthlyLimitTo);
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

				_logger.LogInformation("Filtrowanie kategorii wydatków zakończone sukcesem. Pobrano {Count} kategorii.", SpendingCategories.Count);
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
			_logger.LogInformation("Rozpoczynam sprawdzanie filtra dat. DateFrom={DateFrom}, DateTo={DateTo}", SpendingCategoryFilterRequest.DateFrom, SpendingCategoryFilterRequest.DateTo);

			if (SpendingCategoryFilterRequest.DateFrom > SpendingCategoryFilterRequest.DateTo)
			{
				_logger.LogWarning("Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})",
					SpendingCategoryFilterRequest.DateFrom,
					SpendingCategoryFilterRequest.DateTo);
				return false;
			}

			_logger.LogInformation("Filtr dat jest poprawny.");
			return true;
		}

		private bool CheckAmountFilter(decimal amount)
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie kwoty wydatku. Amount={Amount}", amount);

			if (amount <= 0)
			{
				_logger.LogWarning("Kwota wydatku jest mniejsza lub równa zero. Amount={Amount}", amount);
				return false;
			}

			string amountStr = amount.ToString(CultureInfo.InvariantCulture);
			var parts = amountStr.Split('.');

			if (parts[0].Length > 15)
			{
				_logger.LogWarning("Kwota wydatku przekracza 15 cyfr przed przecinkiem. Amount={Amount}", amount);
				return false;
			}

			if (parts.Length == 2 && parts[1].Length > 2)
			{
				_logger.LogWarning("Kwota wydatku przekracza 2 miejsca po przecinku. Amount={Amount}", amount);
				return false;
			}

			_logger.LogInformation("Kwota wydatku jest poprawna. Amount={Amount}", amount);
			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
