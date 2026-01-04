using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class CreateAccountViewModel : INotifyPropertyChanged
    {
        public UserRequest _userRequest { get; set; }

        private readonly IUserService _service;
        private readonly ILogger<CreateAccountViewModel> _logger;

        private bool _hidePassword = true;
        private string _passwordIcon = "hide_password.png";

        private string _message = "Wypełnij wymagane pola.";
        private Color _messageColor = Colors.Green;

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
        public ICommand TogglePasswordCommand { get; }
        public ICommand CreateAccountCommand { get; }

        public CreateAccountViewModel(
            IUserService service,
			ILogger<CreateAccountViewModel> logger)
        {
            _userRequest = new UserRequest();
            _service = service;
            _logger = logger;

            TogglePasswordCommand = new Command(async () => await TogglePassword());
            CreateAccountCommand = new Command(async () => await CreateUser());
        }

		private async Task TogglePassword()
		{
			_logger.LogInformation(
				"Rozpoczynam przełączanie widoczności hasła. HidePassword: {HidePassword}",
				HidePassword
			);

			HidePassword = !HidePassword;
			PasswordIcon = HidePassword ? "hide_password.png" : "show_password.png";

			_logger.LogInformation(
				"Zakończono przełączanie widoczności hasła. HidePassword: {HidePassword}, Ikona: {Icon}",
				HidePassword,
				PasswordIcon
			);
		}


		private async Task CreateUser()
		{
			_logger.LogInformation(
				"Rozpoczynam tworzenie konta użytkownika (UI). Email: {Email}",
				_userRequest.Email
			);

			KeyboardHelper.HideKeyboard();

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _service.CreateUser(_userRequest);

				_logger.LogInformation(
					"Wynik tworzenia konta użytkownika (UI) {Email}: {StatusCode}",
					_userRequest.Email,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Tworzenie konta użytkownika (UI) nie powiodło się. Email: {Email}, StatusCode: {StatusCode}, Content: {Content}",
						_userRequest.Email,
						response.StatusCode,
						response.Content
					);

					Message = "Tworzenie konta nie powiodło się. Spróbuj później.";
					MessageColor = Colors.Red;
					return;
				}

				_logger.LogInformation(
					"Tworzenie konta użytkownika (UI) zakończone sukcesem. Email: {Email}",
					_userRequest.Email
				);

				MessageColor = Colors.Green;
				Message = "Konto zostało utworzone.";
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas tworzenia konta użytkownika (UI). Email: {Email}",
					_userRequest.Email
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas tworzenia konta użytkownika (UI). Email: {Email}",
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
					"Zakończono proces tworzenia konta użytkownika (UI). Email: {Email}",
					_userRequest.Email
				);
			}
		}



		public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        
    }
}
