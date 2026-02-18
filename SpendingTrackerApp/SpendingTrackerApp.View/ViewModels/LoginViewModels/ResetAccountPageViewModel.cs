using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
	public class ResetAccountPageViewModel : INotifyPropertyChanged
	{
		private readonly IUserService _service;
		private readonly ILogger<ResetAccountPageViewModel> _logger;
		private UserRequest _userRequest;

		private bool _hidePassword;
		private string _passwordIcon;

		private string _message;
		private Color _messageColor;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

		public UserRequest UserRequest
		{
			get => _userRequest;
			set
			{
				if (_userRequest != value)
				{
					_userRequest = value;
					OnPropertyChanged(nameof(UserRequest));
				}
			}
		}

		public bool HidePassword
		{
			get => _hidePassword;
			set
			{
				if (_hidePassword != value)
				{
					_hidePassword = value;
					OnPropertyChanged(nameof(HidePassword));
				}
			}
		}

		public string PasswordIcon
		{
			get => _passwordIcon;
			set
			{
				if (_passwordIcon != value)
				{
					_passwordIcon = value;
					OnPropertyChanged(nameof(PasswordIcon));
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

		public ICommand ResetAccountCommand { get; }

		public ResetAccountPageViewModel(
			IUserService service,
			ILogger<ResetAccountPageViewModel> logger)
		{
			_userRequest = new UserRequest();
			_service = service;
			_logger = logger;

			ResetAccountCommand = new Command(async () => await ResetAccount());
		}

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam reset formularza resetu hasła (UI).");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				UserRequest = new UserRequest();

				HidePassword = true;
				PasswordIcon = "show_password.png";
				Message = "Podaj swój e-mail oraz nowe hasło. Hasło powinno składać się z co najmniej 6 znaków.";
				MessageColor = (Color)Application.Current.Resources["Positive"];
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono reset formularza resetu hasła (UI).");
			}
		}


		private async Task ResetAccount()
		{
			_logger.LogInformation(
				"Rozpoczynam proces resetu hasła użytkownika (UI). Email: {Email}",
				_userRequest.Email
			);

			KeyboardHelper.HideKeyboard();

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _service.ResetPassword(_userRequest);

				_logger.LogInformation(
					"Wynik resetu hasła użytkownika (UI) {Email}: {StatusCode}",
					_userRequest.Email,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Reset hasła użytkownika (UI) nie powiódł się. Email: {Email}, StatusCode: {StatusCode}",
						_userRequest.Email,
						response.StatusCode
					);

					Message = "Zmiana hasła nie powiodła się: nie znaleziono konta lub hasło nie spełnia wymagań (co najmniej 6 znaków). Spróbuj jeszcze raz lub później.";
					MessageColor = Colors.Red;
					return;
				}

				_logger.LogInformation(
					"Reset hasła użytkownika (UI) zakończony sukcesem. Email: {Email}",
					_userRequest.Email
				);

				Message = "Hasło zostało zresetowane.";
				MessageColor = Colors.Green;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas resetu hasła użytkownika (UI). Email: {Email}",
					_userRequest.Email
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas resetu hasła użytkownika (UI). Email: {Email}",
					_userRequest.Email
				);
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces resetu hasła użytkownika (UI). Email: {Email}",
					_userRequest.Email
				);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}



