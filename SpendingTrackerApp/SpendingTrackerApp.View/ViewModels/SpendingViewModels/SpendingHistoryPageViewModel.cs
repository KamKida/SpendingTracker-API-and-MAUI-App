using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.SpendingPages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.SpendingViewModels
{
	public class SpendingHistoryPageViewModel : INotifyPropertyChanged
	{
		private SpendingFilterRequest _filterRequest = new SpendingFilterRequest();
		private User _user;

		private ObservableCollection<Spending> _spendings = new ObservableCollection<Spending>();

		private JsonService _jsonService;
		private ISpendingService _spendingService;
		private IMapper _mapper;
		private ILogger<SpendingHistoryPageViewModel> _logger;

		private bool _filtersVisible = false;
		private bool _showErrorMessage = false;
		private bool _enableFilters = true;
		private bool _enableShowMore = true;
		private bool _useDateFilter = false;
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

		private Color _filterEntryColorFrom = Colors.White;
		private Color _filterEntryColorTo = Colors.White;

		private bool _showLoadingIcon = false;
		private bool _runLoadingIcon = false;
		private bool _blockInteraction = false;

		private Color _dateColor = Colors.White;

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
			User user,
			JsonService jsonService,
			ISpendingService spendingService,
			IMapper mapper,
			ILogger<SpendingHistoryPageViewModel> logger)
		{
			_user = user;
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

		private async Task DeleteSpending(Spending spending)
		{
			_logger.LogInformation(
				"Rozpoczynam usuwanie wydatku. SpendingId={SpendingId}, Amount={Amount}",
				spending.Id,
				spending.Amount
			);

			bool answer = await Application.Current.MainPage.DisplayAlert(
				"",
				$"Czy na pewno usunąć wydatek o kwocie {spending.Amount} zł z dnia {spending.CreationDate}?",
				"Tak",
				"Nie"
			);

			if (!answer) return;

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _spendingService.DeleteSpending(spending.Id);

				if (!response.IsSuccessStatusCode)
				{
					ShowErrorMessage = true;
					return;
				}
				else
				{
					ShowErrorMessage = false;
				}

				_user.ThisMonthSpendings -= spending.Amount;
				Spendings.Remove(spending);
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}
		private async Task GoToAddSpendingPage()
		{
			_logger.LogInformation("Rozpoczynam nawigację do strony dodawania wydatku.");

			try
			{
				await Shell.Current.GoToAsync(nameof(AddSpendingPage));
				_logger.LogInformation("Nawigacja do strony dodawania wydatku zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas nawigacji do strony dodawania wydatku.");
				throw;
			}
		}

		private async Task GoToEditSpendingPage(Spending spending)
		{
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
		}


		private async Task ShowMore()
		{
			if (!Spendings.Any()) return;

			var request = SpendingFilterRequest.Clone();
			request.LastDate = Spendings.Last().CreationDate;

			var response = await _spendingService.Get10(request);
			var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingReponse>>(
				await response.Content.ReadAsStringAsync());

			var nextSpendings = _mapper.Map<ObservableCollection<Spending>>(spendingResponse);

			foreach (var spending in nextSpendings.OrderByDescending(e => e.CreationDate))
			{
				Spendings.Add(spending);
			}
		}

		private async Task ShowHideFilters()
		{
			FiltersVisible = !FiltersVisible;
		}

		private async Task ResetFilter()
		{
			SpendingFilterRequest.Reset();
			await SetBaseInfo();
		}

		public async Task SetBaseInfo()
		{
			SpendingFilterRequest.Reset();
			var response = await _spendingService.Get10(SpendingFilterRequest);
			var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingReponse>>(
				await response.Content.ReadAsStringAsync());

			_mapper.Map(spendingResponse, Spendings);
		}

		private async Task Filter()
		{
			var request = SpendingFilterRequest.Clone();
			var response = await _spendingService.Get10(request, UseDateFilter);
			var spendingResponse = _jsonService.Deserialize<ObservableCollection<SpendingReponse>>(
				await response.Content.ReadAsStringAsync());

			_mapper.Map(spendingResponse, Spendings);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
