using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{

	public class AddFundPageViewModel : INotifyPropertyChanged
	{
		public User User { get; }
		private readonly IFundService _service;
		private readonly IMapper _mapper;

		private string _text = string.Empty;

		private Color _amountColorBorder = Colors.AliceBlue;
		private string _message = "Wprowadź kwotę funduszu w formacie 00.00.Przed kropką może być maksymalnie 15 cyfr, a po kropce 2.";
		private Color _messageColor = Colors.Green;
		private Color _amountColorText { get; set; } = Colors.Black;


		public bool _showLoadingIcon = false;
		public bool _runLoadingIcon = false;

		public bool _blockInteraction = false;

		public bool _enableButtons = true;

		public string Text
		{
			get => _text;

			set
			{
				if (_text != value)
				{
					_text = value;
					OnPropertyChanged(nameof(Text));

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

		public Color AmountColorBorder
		{
			get => _amountColorBorder;
			set
			{
				if (_amountColorBorder != value)
				{
					_amountColorBorder = value;
					OnPropertyChanged(nameof(AmountColorBorder));
				}
			}
		}

		public Color AmountColorText
		{
			get => _amountColorText;
			set
			{
				if (_amountColorText != value)
				{
					_amountColorText = value;
					OnPropertyChanged(nameof(AmountColorText));
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

		public bool EnableButtons
		{
			get => _enableButtons;
			set
			{
				if (_enableButtons != value)
					_enableButtons = value;
				OnPropertyChanged(nameof(EnableButtons));
			}
		}

		public AddFundPageViewModel(
			User user,
			IFundService service,
			IMapper mapper)
		{
			User = user;
			_service = service;
			_mapper = mapper;

			AddFundCommand = new Command(AddFund);
			CancelAddFundCommand = new Command(CancelAddFund);
		}

		public ICommand AddFundCommand { get; }
		public ICommand CancelAddFundCommand { get; }

		public async void AddFund()
		{

			KeyboardHelper.HideKeyboard();

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			bool goodAmount = CheckAmountValue();

			if (!goodAmount)
			{
				MessageColor = Colors.Red;
				AmountColorText = Colors.Red;
				AmountColorBorder = Colors.Red;
			}
			else
			{
				FundRequest newFund = new FundRequest()
				{
					Amount = Decimal.Parse(_text)
				};

				var result = await _service.AddFund(newFund);

				if (result.StatusCode != 200)
				{
					MessageColor = Colors.Red;
					AmountColorText = Colors.Red;
					AmountColorBorder = Colors.Red;
					Message = "Coś poszło nie tak. Spróbuj później.";


				}
				else
				{

					MessageColor = Colors.Green;
					AmountColorText = Colors.Green;
					AmountColorBorder = Colors.Green;
					Message = "Fundusz został dodany.";

					User.ThisMonthFund += newFund.Amount;
				}
			}

			ShowLoadingIcon = false;
			RunLoadingIcon = false;
			BlockInteraction = false;
		}

		private async void CancelAddFund()
		{
			await Shell.Current.GoToAsync("..");
		}

		private bool CheckAmountValue()
		{
			if (string.IsNullOrWhiteSpace(_text))
				return true;

			var parts = _text.Split('.');

			if (parts[0].Length > 15)
				return false;

			if (parts.Length > 1 && parts[1].Length > 2)
				return false;

			return true;
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
