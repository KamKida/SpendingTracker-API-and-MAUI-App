using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	[QueryProperty(nameof(Fund), nameof(Fund))]
	public class EditFundPageViewModel : INotifyPropertyChanged
	{
		private User _user;
		private FundRequest _fundRequest;
		private IFundService _fundService;
		private IMapper _mapper;
		private ILogger<AddFundPageViewModel> _logger;

		private string _message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
		private Color _messageColor = (Color)Application.Current.Resources["Positive"];
		private Color _amountEntryColor = Colors.White;
		public bool _showLoadingIcon = false;
		public bool _runLoadingIcon = false;

		public bool _blockInteraction = false;

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
				if (_fundRequest != null)
				{
					_fundRequest = value;
					OnPropertyChanged(nameof(FundFilterRequest));
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


		public EditFundPageViewModel(
		User user,
		IFundService fundService,
		IMapper mapper,
		ILogger<AddFundPageViewModel> logger)
		{
			_user = user;
			_fundService = fundService;
			_mapper = mapper;
			_logger = logger;
			_fundRequest = new FundRequest();

			EditFundCommand = new Command(async () => await EditFund());
			CancelEditCommand = new Command(async () => await CancelEdit());
		}
		public ICommand EditFundCommand { get; }
		public ICommand CancelEditCommand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu funduszu i błędów UI.");

			FundRequest = new FundRequest();
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
				_user.ThisMonthFund = _user.ThisMonthFund - Fund.Amount + FundRequest.Amount;

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



		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}

	

