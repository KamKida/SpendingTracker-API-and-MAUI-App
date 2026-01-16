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
		private FundRequest _fundRequest = new FundRequest();
		private FundCategoryRequest _fundCategoryRequest = new FundCategoryRequest();
		private ObservableCollection<FundCategory> _fundCategorie = new ObservableCollection<FundCategory>();
		private FundCategoryFilterRequest _fundCategoryFilterRequest = new FundCategoryFilterRequest();
		private JsonService _jsonService;
		private IFundService _fundService;
		private IFundCategoryService _fundCategotuService;
		private IMapper _mapper;
		private ILogger<AddFundPageViewModel> _logger;

		private string _message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
		private Color _messageColor = (Color)Application.Current.Resources["Positive"];
		private Color _amountEntryColor = Colors.White;
		public bool _showLoadingIcon = false;
		public bool _runLoadingIcon = false;

		public bool _blockInteraction = false;

		private bool _showCategories = false;
		private decimal _fundDifrence;
		private Color _diffrenceColor;

		private bool _enableShowMore = false;
		private bool _showErrorMessage = false;
		private bool _filtersVisible = false;

		private bool _useDateFilter = false;

		private Color _filterEntryColorFrom = Colors.White;
		private Color _filterEntryColorTo = Colors.White;
		private Color _dateColor = Colors.White;

		private bool _showFilterErrorMessage = false;
		private string _filterErrorText;


		private Fund _fund;

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

		public string Description
		{
			get => FundRequest.Description;
			set
			{
				if (FundRequest.Description == value)
					return;

				FundRequest.Description = value;
				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(DescriptionCount));
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
			get => _fundCategorie;
			set
			{
				if (_fundCategorie != value)
				{
					_fundCategorie = value;
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

		public decimal FundDifrence
		{
			get => _fundDifrence;
			set
			{
				if (_fundDifrence != value)
				{
					_fundDifrence = value;
					OnPropertyChanged(nameof(FundDifrence));
				}
			}
		}

		public Color DiffrenceColor
		{
			get => _diffrenceColor;
			set
			{
				if (_diffrenceColor != value)
				{
					_diffrenceColor = value;
					OnPropertyChanged(nameof(DiffrenceColor));
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

		public int DescriptionCount
		{
			get => FundRequest.Description?.Length ?? 0;
		}

		public EditFundPageViewModel(
		JsonService jsonService,
		IFundService fundService,
		IFundCategoryService fundCategotuService,
		IMapper mapper,
		ILogger<AddFundPageViewModel> logger)
		{
			_jsonService = jsonService;
			_fundService = fundService;
			_fundCategotuService = fundCategotuService;
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
			DeleteCategoryComand = new Command(async () => await DeleteCategory());
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
		public ICommand DeleteCategoryComand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu funduszu i błędów UI.");

			FundRequest = _mapper.Map<FundRequest>(Fund);

			if (Fund.FundCategory != null)
			{
				FundCategoryRequest = _mapper.Map<FundCategoryRequest>(Fund.FundCategory);
			}
			else
			{
				FundCategoryRequest = new FundCategoryRequest();
			}

			FundDifrence = (decimal)(FundCategoryRequest.ShouldBe == null ? FundRequest.Amount : FundRequest.Amount - FundCategoryRequest.ShouldBe);
			DiffrenceColor = FundDifrence > 0 ? (Color)Application.Current.Resources["Positive"] : (Color)Application.Current.Resources["Negative"];
			ShowCategories = false;
			Message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
			MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Positive"];

			_logger.LogInformation("Zakończono resetowanie stanu funduszu i błędów UI.");
		}

		public async Task EditFund()
		{
			_logger.LogInformation(
				"Rozpoczynam edycję funduszu. FundId={FundId}, OldAmount={OldAmount}, NewAmount={NewAmount}",
				Fund.Id,
				Fund.Amount,
				FundRequest.Amount
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				bool goodAmount = CheckAmount();
				if (!goodAmount)
				{
					_logger.LogWarning(
						"Niepoprawna kwota funduszu. FundId={FundId}, Amount={Amount}",
						Fund.Id,
						FundRequest.Amount
					);

					MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Negative"];
					Message = "Kwota funduszu jest niepoprawna.";
					return;
				}

				FundRequest.Id = Fund.Id;

				if (!String.IsNullOrEmpty(FundCategoryRequest.Name))
				{
					FundRequest.FundCategoryId = FundCategoryRequest.Id;
				}
				else
				{
					FundRequest.FundCategoryId = null;
				}

				var response = await _fundService.EditFund(FundRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Edycja funduszu nie powiodła się. FundId={FundId}, StatusCode={StatusCode}, Content={Content}",
						Fund.Id,
						response.StatusCode,
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				Fund.Amount = FundRequest.Amount;

				MessageColor = (Color)Application.Current.Resources["Positive"];
				Message = "Fundusz zaktualizowany pomyślnie.";

				_logger.LogInformation(
					"Edycja funduszu zakończona sukcesem. FundId={FundId}, NewAmount={NewAmount}",
					Fund.Id,
					FundRequest.Amount
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji funduszu. FundId={FundId}, Amount={Amount}",
					Fund.Id,
					FundRequest.Amount
				);
				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji funduszu. FundId={FundId}, Amount={Amount}",
					Fund.Id,
					FundRequest.Amount
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
					"Zakończono proces edycji funduszu. FundId={FundId}",
					Fund.Id
				);
			}
		}

		public async Task ShowCategoryList()
		{
			var requset = FundCategoryFilterRequest.Clone();
			var result = await _fundCategotuService.Get10(requset);

			var categorResult = _jsonService.Deserialize<ObservableCollection<FundCategoryResponse>>(await result.Content.ReadAsStringAsync());

			FundCategoryFilterRequest = new FundCategoryFilterRequest();

			FundCategories = _mapper.Map<ObservableCollection<FundCategory>>(categorResult);

			if (FundCategories.Count % 10 != 0)
			{
				EnableShowMore = false;
			}
			else
			{
				EnableShowMore = true;
			}

			ShowCategories = true;
		}

		public async Task ShowMoreCategories()
		{
			_logger.LogInformation("Rozpoczynam ładowanie kolejnych funduszy.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (!FundCategories.Any())
				{
					_logger.LogWarning("Brak funduszy do załadowania kolejnych.");
					return;
				}

				var request = FundCategoryFilterRequest.Clone();
				request.LastDate = FundCategories.Last().CreationDate;

				var response = await _fundCategotuService.Get10(request);

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

		private bool CheckAmount()
		{
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie kwoty funduszu. Amount={Amount}",
				FundRequest.Amount
			);

			if (FundRequest.Amount <= 0)
			{
				_logger.LogWarning(
					"Kwota funduszu jest mniejsza lub równa zero. Amount={Amount}",
					FundRequest.Amount
				);
				return false;
			}

			string amountStr = FundRequest.Amount.ToString(CultureInfo.InvariantCulture);
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota funduszu przekracza 15 cyfr przed przecinkiem. Amount={Amount}",
					FundRequest.Amount
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota funduszu przekracza 2 miejsca po przecinku. Amount={Amount}",
					FundRequest.Amount
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota funduszu jest poprawna. Amount={Amount}",
				FundRequest.Amount
			);

			return true;
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation(
				"Rozpoczynam anulowanie edycji funduszu (powrót do historii funduszy). FundId={FundId}, Amount={Amount}",
				Fund?.Id,
				Fund?.Amount
			);

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation(
					"Nawigacja po anulowaniu edycji funduszu zakończona sukcesem. FundId={FundId}",
					Fund?.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas anulowania edycji funduszu. FundId={FundId}",
					Fund?.Id
				);
				throw;
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

		private async Task ResetFilter()
		{
			_logger.LogInformation("Rozpoczynam czyszczenie filtrów funduszy.");

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
							FundCategoryFilterRequest.DateFrom, FundCategoryFilterRequest.DateTo);
					}
					else
					{
						DateColor = Colors.White;
					}
				}


				if (FundCategoryFilterRequest.ShouldBeFrom != null && FundCategoryFilterRequest.ShouldBeTo != null)
				{
					if (FundCategoryFilterRequest.ShouldBeFrom > FundCategoryFilterRequest.ShouldBeTo)
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
				}

				if (FundCategoryFilterRequest.ShouldBeFrom != null && !CheckAmountFilter((decimal)FundCategoryFilterRequest.ShouldBeFrom))
				{
					FilterEntryColorFrom = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Od' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountFrom={AmountFrom}", FundCategoryFilterRequest.ShouldBeFrom);
				}
				else
				{
					FilterEntryColorFrom = Colors.White;
				}

				if (FundCategoryFilterRequest.ShouldBeTo != null && !CheckAmountFilter((decimal)FundCategoryFilterRequest.ShouldBeTo))
				{
					FilterEntryColorTo = (Color)Application.Current.Resources["Negative"];
					errorMessage = "Kwota 'Do' jest w złym formacie. Format: 00.00. Do 15 cyfr przed przecinkiem, 2 po przecinku.";
					_logger.LogWarning(errorMessage + " AmountTo={AmountTo}", FundCategoryFilterRequest.ShouldBeTo);
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

				var request = FundCategoryFilterRequest.Clone();

				var response = await _fundCategotuService.Get10(request, useDatesFromToo: UseDateFilter);
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

		private async Task SelectCategory(FundCategory fundCategory)
		{
			FundCategoryRequest = _mapper.Map<FundCategoryRequest>(fundCategory);
			FundDifrence = (decimal)(FundCategoryRequest.ShouldBe == null ? FundRequest.Amount : FundRequest.Amount - FundCategoryRequest.ShouldBe);
			DiffrenceColor = FundDifrence > 0 ? (Color)Application.Current.Resources["Positive"] : (Color)Application.Current.Resources["Negative"];
			ShowCategories = false;
		}

		private async Task DeleteCategory()
		{
			FundCategoryRequest = new FundCategoryRequest();
			DiffrenceColor = (Color)Application.Current.Resources["Positive"];
			FundDifrence = FundRequest.Amount;
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



