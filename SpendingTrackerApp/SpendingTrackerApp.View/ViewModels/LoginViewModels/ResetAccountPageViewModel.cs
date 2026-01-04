using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class ResetAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _service;
        private readonly ILogger<ResetAccountPageViewModel> _logger;
        public UserRequest _userRequest { get; set; }

        private bool _hidePassword = true;
        private string _passwordIcon = "hide_password.png";

        private string _message = "Wprować swój e-mail i nowe hasło.";
        private Color _messageCollor = Colors.Green;

        public bool _showLoadingIcon = false;
        public bool _runLoadingIcon = false;

        public bool _blockInteraction = false;

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
            get => _messageCollor;
            set
            {
                if(_messageCollor != value)
                {
                    _messageCollor = value;
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
        public ICommand ResetAccountCommand { get;}

        public ResetAccountPageViewModel(
            IUserService service,
			ILogger<ResetAccountPageViewModel> logger)
        {
            _userRequest = new UserRequest();

            _service = service;

            _logger = logger;

            ResetAccountCommand = new Command(async () => await ResetAccount());

        }

		private async Task ResetAccount()
		{
			_logger.LogInformation(
				"Rozpoczynam reset hasła użytkownika (UI). Email: {Email}",
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

				if (response.StatusCode != HttpStatusCode.OK)
				{
					_logger.LogWarning(
						"Reset hasła użytkownika (UI) nie powiódł się. Email: {Email}, StatusCode: {StatusCode}",
						_userRequest.Email,
						response.StatusCode
					);

					Message ="Reset hasła nie powiódł się. Spróbuj póxniej.";
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
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
