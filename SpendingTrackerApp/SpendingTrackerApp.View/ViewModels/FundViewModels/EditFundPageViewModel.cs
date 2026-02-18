using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	[QueryProperty(nameof(Fund), nameof(Fund))]
	public class EditFundPageViewModel : INotifyPropertyChanged
	{

		private readonly JsonService _jsonService;
		private readonly IFundService _fundService;
		private readonly IFundCategoryService _fundCategoryService;
		private readonly IMapper _mapper;
		private readonly ILogger<AddFundPageViewModel> _logger;

		private Fund _fund;
		private FundRequest _fundRequest = new FundRequest();
		private FundCategoryRequest _fundCategoryRequest = new FundCategoryRequest();
		private FundCategoryFilterRequest _fundCategoryFilterRequest = new FundCategoryFilterRequest();
		private ObservableCollection<FundCategory> _fundCategories = new ObservableCollection<FundCategory>();

		private string _message;
		private string _filterErrorText;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

		private bool _showCategories;
		private bool _enableShowMore;
		private bool _showErrorMessage;
		private bool _filtersVisible;
		private bool _useDateFilter;
		private bool _showFilterErrorMessage;

		private decimal _fundDifference;

		private Color _messageColor;
		private Color _amountEntryColor;
		private Color _differenceColor;
		private Color _filterEntryColorFrom;
		private Color _filterEntryColorTo;
		private Color _dateColor;

		public Fund Fund
		{
			get => _fund;
			set
			{
				_fund = value;
				OnPropertyChanged(nameof(Fund));
				OnPropertyChanged(nameof(Fund.Amount));
			}
		}

		public FundRequest FundRequest
		{
			get => _fundRequest;
			set
			{
				if (_fundRequest != value)
				{
					_fundRequest = value;
					OnPropertyChanged(nameof(FundRequest));
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
				}
			}
		}

		public FundCategoryFilterRequest FundCategoryFilterRequest
		{
			get => _fundCategoryFilterRequest;
			set
			{
				if (_fundCategoryFilterRequest != value)
				{
					_fundCategoryFilterRequest = value;
					OnPropertyChanged(nameof(FundCategoryFilterRequest));
				}
			}
		}

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

		public decimal FundDifference
		{
			get => _fundDifference;
			set
			{
				if (_fundDifference != value)
				{
					_fundDifference = value;
					OnPropertyChanged(nameof(FundDifference));
				}
			}
		}

		public string Description
		{
			get => FundRequest.Description;
			set
			{
				if (FundRequest.Description == value) return;

				FundRequest.Description = value;
				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(DescriptionCount));
			}
		}

		public int DescriptionCount => FundRequest.Description?.Length ?? 0;

		public EditFundPageViewModel(
			JsonService jsonService,
			IFundService fundService,
			IFundCategoryService fundCategoryService,
			IMapper mapper,
			ILogger<AddFundPageViewModel> logger)
		{
			_jsonService = jsonService;
			_fundService = fundService;
			_fundCategoryService = fundCategoryService;
			_mapper = mapper;
			_logger = logger;

			EditFundCommand = new Command(async () => await EditFund());
			CancelEditCommand = new Command(async () => await CancelEdit());
			ShowCategoryListCommand = new Command(async () => await ShowCategoryList());
			ShowMoreCategoriesCommand = new Command(async () => await ShowMoreCategories());
			ShowHideFiltersCommand = new Command(async () => await ShowHideFilters());
			ResetFilterCommand = new Command(async () => await ResetFilter());
			FilterCommand = new Command(async () => await Filter());
			CancelChoiceCategoryCommand = new Command(async () => await CancelChoiceCategory());
			SelectCategoryCommand = new Command<FundCategory>(async (fCategory) => await SelectCategory(fCategory));
			DeleteCategoryCommand = new Command(async () => await DeleteCategory());
		}

		public ICommand EditFundCommand { get; }
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
			_logger.LogInformation("Rozpoczynam resetowanie stanu funduszu i błędów UI.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundRequest = _mapper.Map<FundRequest>(Fund);

				if (Fund.FundCategory != null)
				{
					FundCategoryRequest = _mapper.Map<FundCategoryRequest>(Fund.FundCategory);
				}
				else
				{
					FundCategoryRequest = new FundCategoryRequest();
				}

				FundCategoryFilterRequest = new FundCategoryFilterRequest();
				FundCategories = new ObservableCollection<FundCategory>();

				FundDifference = (decimal)(FundCategoryRequest.ShouldBe == null ? FundRequest.Amount : FundRequest.Amount - FundCategoryRequest.ShouldBe);
				DifferenceColor = FundDifference > 0 ? (Color)Application.Current.Resources["Positive"] : (Color)Application.Current.Resources["Negative"];

				ShowCategories = false;
				EnableShowMore = false;
				ShowErrorMessage = false;
				FiltersVisible = false;
				UseDateFilter = false;
				ShowFilterErrorMessage = false;

				Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
				FilterErrorText = string.Empty;

				MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Positive"];
				FilterEntryColorFrom = Colors.White;
				FilterEntryColorTo = Colors.White;
				DateColor = Colors.White;

				_logger.LogInformation("Zakończono resetowanie stanu funduszu i błędów UI.");
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}


		public async Task EditFund()
		{
			_logger.LogInformation("Rozpoczynam edycję funduszu. FundId={FundId}, OldAmount={OldAmount}, NewAmount={NewAmount}", Fund.Id, Fund.Amount, FundRequest.Amount);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				bool goodAmount = CheckAmount();
				if (!goodAmount)
				{
					_logger.LogWarning("Niepoprawna kwota funduszu. FundId={FundId}, Amount={Amount}", Fund.Id, FundRequest.Amount);

					MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Negative"];
					Message = "Kwota funduszu jest niepoprawna.";
					return;
				}

				FundRequest.Id = Fund.Id;

				FundRequest.FundCategoryId = string.IsNullOrEmpty(FundCategoryRequest.Name) ? null : FundCategoryRequest.Id;

				var response = await _fundService.EditFund(FundRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning("Edycja funduszu nie powiodła się. FundId={FundId}, StatusCode={StatusCode}, Content={Content}", Fund.Id, response.StatusCode, content);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				Fund.Amount = FundRequest.Amount;
				MessageColor = (Color)Application.Current.Resources["Positive"];
				Message = "Fundusz zaktualizowany pomyślnie.";

				_logger.LogInformation("Edycja funduszu zakończona sukcesem. FundId={FundId}, NewAmount={NewAmount}", Fund.Id, FundRequest.Amount);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas edycji funduszu. FundId={FundId}, Amount={Amount}", Fund.Id, FundRequest.Amount);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas edycji funduszu. FundId={FundId}, Amount={Amount}", Fund.Id, FundRequest.Amount);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces edycji funduszu. FundId={FundId}", Fund.Id);
			}
		}

		public async Task ShowCategoryList()
		{
			_logger.LogInformation("Rozpoczynam wyświetlanie listy kategorii funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var request = FundCategoryFilterRequest.Clone();
				var result = await _fundCategoryService.Get10(request);

				var categoryResult = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(await result.Content.ReadAsStringAsync());

				FundCategoryFilterRequest = new FundCategoryFilterRequest();

				FundCategories = _mapper.Map<ObservableCollection<FundCategory>>(categoryResult);

				EnableShowMore = FundCategories.Count % 10 == 0 && FundCategories.Any();

				ShowCategories = true;

				_logger.LogInformation("Wyświetlanie listy kategorii zakończone. Liczba kategorii={CategoryCount}", FundCategories.Count);
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
			_logger.LogInformation("Rozpoczynam ładowanie kolejnych kategorii funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!FundCategories.Any())
				{
					_logger.LogWarning("Brak kategorii do załadowania kolejnych.");
					return;
				}

				var request = FundCategoryFilterRequest.Clone();
				request.LastDate = FundCategories.Last().CreationDate;

				var response = await _fundCategoryService.Get10(request);

				_logger.LogInformation("Wynik pobierania kolejnych kategorii: StatusCode={StatusCode}", response.StatusCode);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie kolejnych kategorii nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					ShowErrorMessage = true;
					return;
				}

				int oldCount = FundCategories.Count;
				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(await response.Content.ReadAsStringAsync());
				var next10Funds = _mapper.Map<ObservableCollection<FundCategory>>(fundResponse);

				foreach (var fund in next10Funds.OrderByDescending(f => f.CreationDate))
					FundCategories.Add(fund);

				EnableShowMore = !(FundCategories.Count % 10 != 0 || oldCount == FundCategories.Count);

				_logger.LogInformation("Pobrano {NewFunds} kolejnych kategorii. Łącznie={TotalFunds}, EnableShowMore={EnableShowMore}", next10Funds.Count, FundCategories.Count, EnableShowMore);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas ładowania kolejnych kategorii.");
				ShowErrorMessage = true;
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono proces ładowania kolejnych kategorii.");
			}
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation("Rozpoczynam anulowanie edycji funduszu. FundId={FundId}, Amount={Amount}", Fund?.Id, Fund?.Amount);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation("Nawigacja po anulowaniu edycji zakończona sukcesem. FundId={FundId}", Fund?.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas anulowania edycji funduszu. FundId={FundId}", Fund?.Id);
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		private bool CheckAmount()
		{
			_logger.LogInformation("Rozpoczynam sprawdzanie kwoty funduszu. Amount={Amount}", FundRequest.Amount);

			if (FundRequest.Amount <= 0)
			{
				_logger.LogWarning("Kwota funduszu jest mniejsza lub równa zero. Amount={Amount}", FundRequest.Amount);
				return false;
			}

			string amountStr = FundRequest.Amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning("Kwota funduszu przekracza 15 cyfr przed przecinkiem. Amount={Amount}", FundRequest.Amount);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning("Kwota funduszu przekracza 2 miejsca po przecinku. Amount={Amount}", FundRequest.Amount);
				return false;
			}

			_logger.LogInformation("Kwota funduszu jest poprawna. Amount={Amount}", FundRequest.Amount);
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
			_logger.LogInformation("Rozpoczynam sprawdzanie filtra dat. DateFrom={DateFrom}, DateTo={DateTo}", FundCategoryFilterRequest.DateFrom, FundCategoryFilterRequest.DateTo);

			if (FundCategoryFilterRequest.DateFrom > FundCategoryFilterRequest.DateTo)
			{
				_logger.LogWarning("Niepoprawny zakres dat. DateFrom ({DateFrom}) jest późniejsza niż DateTo ({DateTo})", FundCategoryFilterRequest.DateFrom, FundCategoryFilterRequest.DateTo);
				return false;
			}

			_logger.LogInformation("Filtr dat jest poprawny. DateFrom={DateFrom}, DateTo={DateTo}", FundCategoryFilterRequest.DateFrom, FundCategoryFilterRequest.DateTo);
			return true;
		}

		private async Task SelectCategory(FundCategory fundCategory)
		{
			_logger.LogInformation("Rozpoczynam wybór kategorii funduszu. CategoryId={CategoryId}, Name={Name}", fundCategory.Id, fundCategory.Name);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundCategoryRequest = _mapper.Map<FundCategoryRequest>(fundCategory);
				FundDifference = (decimal)(FundCategoryRequest.ShouldBe == null ? FundRequest.Amount : FundRequest.Amount - FundCategoryRequest.ShouldBe);
				DifferenceColor = FundDifference > 0 ? (Color)Application.Current.Resources["Positive"] : (Color)Application.Current.Resources["Negative"];
				ShowCategories = false;

				_logger.LogInformation("Kategoria wybrana pomyślnie. CategoryId={CategoryId}, FundDifference={FundDifference}", fundCategory.Id, FundDifference);
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
			_logger.LogInformation("Rozpoczynam usuwanie kategorii funduszu.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundCategoryRequest = new FundCategoryRequest();
				DifferenceColor = (Color)Application.Current.Resources["Positive"];
				FundDifference = FundRequest.Amount;

				_logger.LogInformation("Kategoria funduszu została usunięta. FundDifference={FundDifference}", FundDifference);
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
			_logger.LogInformation("Anulowanie wyboru kategorii funduszu.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				ShowCategories = false;

				_logger.LogInformation("Wybór kategorii został anulowany.");
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
			_logger.LogInformation("Rozpoczynam pokazywanie/ukrywanie filtrów funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

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
			_logger.LogInformation("Rozpoczynam resetowanie filtrów funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				FundCategoryFilterRequest.Reset();
				DateColor = FilterEntryColorFrom = FilterEntryColorTo = Colors.White;
				UseDateFilter = false;
				await ShowCategoryList();

				_logger.LogInformation("Filtry funduszy zostały wyczyszczone.");
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
					_logger.LogWarning(errorMessage + " DateFrom={DateFrom}, DateTo={DateTo}", FundCategoryFilterRequest.DateFrom, FundCategoryFilterRequest.DateTo);
				}

				if (FundCategoryFilterRequest.ShouldBeFrom != null && FundCategoryFilterRequest.ShouldBeTo != null &&
					FundCategoryFilterRequest.ShouldBeFrom > FundCategoryFilterRequest.ShouldBeTo)
				{
					FilterEntryColorFrom = FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' nie może być większa od kwoty 'Do'.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}, AmountTo={AmountTo}", FundCategoryFilterRequest.ShouldBeFrom, FundCategoryFilterRequest.ShouldBeTo);
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
				var response = await _fundCategoryService.Get10(request, useDatesFromToo: UseDateFilter);
				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("Pobieranie funduszy po filtrze nie powiodło się. StatusCode={StatusCode}, Content={Content}", response.StatusCode, content);
					ShowErrorMessage = true;
					return;
				}

				var fundResponse = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(await response.Content.ReadAsStringAsync());
				_mapper.Map(fundResponse, FundCategories);

				_logger.LogInformation("Filtrowanie funduszy zakończone pomyślnie. Liczba funduszy={FundCount}", FundCategories.Count);
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

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	}
}