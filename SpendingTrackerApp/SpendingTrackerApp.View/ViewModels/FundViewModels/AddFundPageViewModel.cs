using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.HelpModels;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	public class AddFundPageViewModel : INotifyPropertyChanged
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


		public AddFundPageViewModel(
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

			AddFundCommand = new Command(async () => await AddFund());
			CancelAddCommand = new Command(async () => await CancelAdd());
		}
		public ICommand AddFundCommand { get; }
		public ICommand CancelAddCommand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie stanu funduszu i błędów UI.");

			FundRequest = new FundRequest();
			Message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne.";
			MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Positive"];

			_logger.LogInformation("Zakończono resetowanie stanu funduszu i błędów UI.");
		}

		public async Task AddFund()
		{
			_logger.LogInformation(
				"Rozpoczynam dodawanie funduszu. Amount={Amount}",
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
						"Niepoprawna kwota funduszu. Amount={Amount}",
						FundRequest.Amount
					);

					MessageColor = AmountEntryColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				var response = await _fundService.AddFund(FundRequest);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Dodawanie funduszu nie powiodło się. StatusCode={StatusCode}, Content={Content}",
						response.StatusCode,
						content
					);

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak, zresetuj aplikację i spróbuj ponownie.";
					return;
				}

				Message = "Fundusz dodany pomyślnie.";
				_user.ThisMonthFund += FundRequest.Amount;
				Fund newFund = _mapper.Map<Fund>(FundRequest);

				_logger.LogInformation(
					"Fundusz dodany pomyślnie. Amount={Amount}",
					FundRequest.Amount
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania funduszu. Amount={Amount}",
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
					"Nieoczekiwany błąd podczas dodawania funduszu. Amount={Amount}",
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

				_logger.LogInformation("Zakończono proces dodawania funduszu.");
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

		public async Task CancelAdd()
		{
			_logger.LogInformation("Rozpoczynam anulowanie dodawania funduszu (powrót do histori funduszy).");

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation("Nawigacja po anulowaniu dodawania funduszu zakończona sukcesem.");
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas anulowania dodawania funduszu."
				);
				throw;
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
