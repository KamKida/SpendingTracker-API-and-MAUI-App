using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.FundCategorysPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundCategoryViewModels
{
	public class FundCategoryListViewModel : INotifyPropertyChanged
	{
		private FundCategoryFilterRequest _filterRequest;
		private ObservableCollection<FundCategory> _fundCategories;
		private JsonService _jsonService;
		private IFundCategoryService _fundService;
		private IMapper _mapper;
		private ILogger<FundCategoryListViewModel> _logger;

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

		public ObservableCollection<FundCategory> FundCategories
		{
			get => _fundCategories;
			set
			{
				if (_fundCategories != value)
				{
					_fundCategories = value;
					OnPropertyChanged(nameof(FundCategories));
				}
			}
		}

		public FundCategoryFilterRequest FundCategoryFilterRequest
		{
			get => _filterRequest;
			set
			{
				if (_filterRequest != value)
				{
					_filterRequest = value;
					OnPropertyChanged(nameof(FundCategoryFilterRequest));
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

		public ICommand ShowHideFiltersCommand { get; }
		public ICommand ResetFilterCommand { get; }
		public ICommand FilterCommand { get; }
		public ICommand OpenDateCommand { get; }
		public ICommand DeleteFundCategoryCommand { get; }
		public ICommand ShowMoreCommand { get; }
		public ICommand GoToAddFundCategoryPageCommand { get; }
		public ICommand GoToEditFundCategoryPageCommand { get; }

		public FundCategoryListViewModel(
			JsonService jsonService,
			IFundCategoryService fundService,
			IMapper mapper,
			ILogger<FundCategoryListViewModel> logger)
		{
			_jsonService = jsonService;
			_fundService = fundService;
			_mapper = mapper;
			_logger = logger;

			_filterRequest = new FundCategoryFilterRequest();

			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			OpenDateCommand = new Command<DatePicker>(async (picker) => await OpenDate(picker));
			DeleteFundCategoryCommand = new Command<FundCategory>(async (fCategory) => await DeleteFundCategory(fCategory));
			ShowMoreCommand = new Command(async () => await ShowMore());
			GoToAddFundCategoryPageCommand = new Command(async () => await GoToAddFundCategoryPage());
			GoToEditFundCategoryPageCommand = new Command<FundCategory>(async (fCategory) => await GoToEditFundCategoryPage(fCategory));
		}

		public async Task Reset()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			FundCategories = new ObservableCollection<FundCategory>();
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji o kategoriach funduszy.");

			FundCategoryFilterRequest.Reset();
			DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
			UseDateFilter = false;

			FiltersVisible = false;
			ShowFilterErrorMessage = false;
			FilterErrorText = string.Empty;
			ShowCategories = false;

			try
			{
				var response = await _fundService.Get10(FundCategoryFilterRequest);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji o kategoriach funduszy: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie kategorii funduszy nie powiodło się. StatusCode={StatusCode}, Content={Content}",
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
				var fundCategoryResponse = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(content);

				_mapper.Map(fundCategoryResponse, FundCategories);

				EnableShowMore = FundCategories.Count % 10 == 0;

				_logger.LogInformation(
					"Pobrano i zmapowano {Count} kategorii funduszy do kolekcji Funds.",
					fundCategoryResponse.Count
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania podstawowych informacji o kategoriach funduszy."
				);
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania podstawowych informacji o kategoriach funduszy."
				);
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
				_logger.LogInformation("Zakończono proces pobierania podstawowych informacji o kategoriach funduszy.");
			}
		}


		private async Task DeleteFundCategory(FundCategory fundCategory)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam proces usuwania kategorii funduszu. FundCategoryId={FundCategoryId}, Name={Name}, CreationDate={CreationDate}",
				fundCategory.Id,
				fundCategory.Name,
				fundCategory.CreationDate
			);

			try
			{
				bool answer = await Application.Current.MainPage.DisplayAlert(
					"",
					$"Czy na pewno usunąć kategorię funduszu o nazwie: {fundCategory.Name}?\nDodana: {fundCategory.CreationDate}",
					"Tak",
					"Nie"
				);

				if (!answer)
				{
					_logger.LogInformation(
						"Usuwanie kategorii funduszu anulowane przez użytkownika. FundCategoryId={FundCategoryId}",
						fundCategory.Id
					);
					return;
				}

				var response = await _fundService.DeleteFundCategory(fundCategory.Id);

				_logger.LogInformation(
					"Wynik usuwania kategorii funduszu: StatusCode={StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning(
						"Usuwanie kategorii funduszu nie powiodło się. FundCategoryId={FundCategoryId}, StatusCode={StatusCode}, Content={Content}",
						fundCategory.Id,
						response.StatusCode,
						content
					);
					ShowErrorMessage = true;
					return;
				}

				_logger.LogInformation(
					"Kategoria funduszu została pomyślnie usunięta. FundCategoryId={FundCategoryId}, Name={Name}",
					fundCategory.Id,
					fundCategory.Name
				);

				FundCategories.Remove(fundCategory);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania kategorii funduszu. FundCategoryId={FundCategoryId}",
					fundCategory.Id
				);
				ShowErrorMessage = true;
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania kategorii funduszu. FundCategoryId={FundCategoryId}",
					fundCategory.Id
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
					"Zakończono proces usuwania kategorii funduszu. FundCategoryId={FundCategoryId}",
					fundCategory.Id
				);
			}
		}

		private async Task ShowMore()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam ładowanie kolejnych funduszy.");

			try
			{
				if (!FundCategories.Any())
				{
					_logger.LogWarning("Brak funduszy do załadowania kolejnych.");
					return;
				}

				var request = FundCategoryFilterRequest.Clone();
				request.LastDate = FundCategories.Last().CreationDate;

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

				int oldFundCount = FundCategories.Count;

				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(await response.Content.ReadAsStringAsync());
				var next10Funds = _mapper.Map<ObservableCollection<FundCategory>>(fundResponse);

				foreach (var fund in next10Funds.OrderByDescending(f => f.CreationDate))
				{
					FundCategories.Add(fund);
				}

				EnableShowMore = !(FundCategories.Count % 10 != 0 || oldFundCount == FundCategories.Count);

				_logger.LogInformation(
					"Pobrano {NewFunds} kolejnych funduszy. Łączna liczba funduszy: {TotalFunds}. EnableShowMore={EnableShowMore}",
					next10Funds.Count,
					FundCategories.Count,
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

		private async Task GoToAddFundCategoryPage()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania kategorii funduszy.");

			try
			{
				await Shell.Current.GoToAsync(nameof(AddFundCategoryPage));
				_logger.LogInformation("Nawigacja do strony dodawania kategorii funduszy zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony dodawania kategorii funduszy."
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

		private async Task GoToEditFundCategoryPage(FundCategory fundCategory)
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam nawigację do strony edycji kategorii funduszu. FundCategoryId={FundCategoryId}, Name={Name}, ShouldBe={ShouldBe}, CreationDate={CreationDate}",
				fundCategory.Id,
				fundCategory.Name,
				fundCategory.ShouldBe,
				fundCategory.CreationDate
			);

			try
			{
				await Shell.Current.GoToAsync(
					nameof(EditFundCategoryPage),
					new Dictionary<string, object>
					{
						[nameof(FundCategory)] = fundCategory
					}
				);

				_logger.LogInformation(
					"Nawigacja do strony edycji kategorii funduszu zakończona sukcesem. FundCategoryId={FundCategoryId}",
					fundCategory.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas nawigacji do strony edycji kategorii funduszu. FundCategoryId={FundCategoryId}",
					fundCategory.Id
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

			_logger.LogInformation("Rozpoczynam czyszczenie filtrów funduszy.");

			try
			{
				FundCategoryFilterRequest.Reset();
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

		private async Task OpenDate(DatePicker picker)
		{
			if (picker == null)
				return;

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await MainThread.InvokeOnMainThreadAsync(() => picker.Focus());
				_logger.LogInformation("DatePicker został ustawiony w tryb focus.");
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

			_logger.LogInformation("Rozpoczynam filtrowanie funduszy.");

			try
			{
				ShowFilterErrorMessage = false;
				string errorMessage = null;

				if (UseDateFilter && !CheckDateFilter())
				{
					DateColor = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Data 'Od' nie może być większa niż data 'Do'.";
					_logger.LogWarning(errorMessage + " DateFrom={DateFrom}, DateTo={DateTo}",
						FundCategoryFilterRequest.DateFrom, FundCategoryFilterRequest.DateTo);
				}
				else
				{
					DateColor = Colors.White;
				}

				if (FundCategoryFilterRequest.ShouldBeFrom != null && FundCategoryFilterRequest.ShouldBeTo != null &&
					FundCategoryFilterRequest.ShouldBeFrom > FundCategoryFilterRequest.ShouldBeTo)
				{
					FilterEntryColorFrom = FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' nie może być większa od kwoty 'Do'.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}, AmountTo={AmountTo}",
						FundCategoryFilterRequest.ShouldBeFrom, FundCategoryFilterRequest.ShouldBeTo);
				}
				else
				{
					FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				}

				if (FundCategoryFilterRequest.ShouldBeFrom != null && !CheckAmountFilter((decimal)FundCategoryFilterRequest.ShouldBeFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}", FundCategoryFilterRequest.ShouldBeFrom);
				}

				if (FundCategoryFilterRequest.ShouldBeTo != null && !CheckAmountFilter((decimal)FundCategoryFilterRequest.ShouldBeTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountTo={AmountTo}", FundCategoryFilterRequest.ShouldBeTo);
				}

				if (!string.IsNullOrEmpty(errorMessage))
				{
					FilterErrorText = errorMessage;
					ShowFilterErrorMessage = true;
					return;
				}

				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				FiltersVisible = false;

				var request = FundCategoryFilterRequest.Clone();
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

				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(fundResponse, FundCategories);

				_logger.LogInformation("Filtrowanie funduszy zakończone sukcesem. Liczba funduszy: {Count}", FundCategories.Count);
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
				FundCategoryFilterRequest.DateFrom,
				FundCategoryFilterRequest.DateTo
			);

			if (FundCategoryFilterRequest.DateFrom > FundCategoryFilterRequest.DateTo)
			{
				_logger.LogWarning(
					"Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})",
					FundCategoryFilterRequest.DateFrom,
					FundCategoryFilterRequest.DateTo
				);
				return false;
			}

			_logger.LogInformation(
				"Filtr dat jest poprawny. DateFrom={DateFrom}, DateTo={DateTo}",
				FundCategoryFilterRequest.DateFrom,
				FundCategoryFilterRequest.DateTo
			);
			return true;
		}

		private bool CheckAmountFilter(decimal amount)
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie kwoty funduszu. Amount={Amount}", amount);

			if (amount <= 0)
			{
				_logger.LogWarning("Kwota funduszu jest mniejsza lub równa zero. Amount={Amount}", amount);
				return false;
			}

			string amountStr = amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning("Kwota funduszu przekracza 15 cyfr przed przecinkiem. Amount={Amount}", amount);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning("Kwota funduszu przekracza 2 miejsca po przecinku. Amount={Amount}", amount);
				return false;
			}

			_logger.LogInformation("Kwota funduszu jest poprawna. Amount={Amount}", amount);
			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

	}
}
