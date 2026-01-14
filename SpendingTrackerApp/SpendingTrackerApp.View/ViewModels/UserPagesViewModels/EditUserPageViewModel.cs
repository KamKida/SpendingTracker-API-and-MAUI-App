using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.AddShells;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.UserPagesViewModels
{
	public class EditUserPageViewModel : INotifyPropertyChanged
	{
		private User User { get; set; }

		private UserRequest _userRequest;

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

		private readonly IUserService _service;
		private readonly IMapper _mapper;
		private readonly ILogger<EditUserPageViewModel> _logger;

		private bool _hidePassword = true;
		private string _passwordIcon = "hide_password.png";

		private string _message;
		private Color _messageColor = Colors.Green;
		private bool _showMessage = false;

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

		public bool ShowMessage
		{
			get => _showMessage;
			set
			{
				if (_showMessage != value)
				{
					_showMessage = value;
					OnPropertyChanged(nameof(ShowMessage));
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
		public ICommand EditAccountCommand { get; }
		public ICommand DeleteAccountCommand { get; }

		public EditUserPageViewModel(
			User user,
			IUserService service,
			IMapper mapper,
			ILogger<EditUserPageViewModel> logger)
		{
			User = user;
			_service = service;
			_mapper = mapper;
			_logger = logger;

			TogglePasswordCommand = new Command(async () => await TogglePassword());
			EditAccountCommand = new Command(async () => await EditUser());
			DeleteAccountCommand = new Command(async () => await DeleteAccount());

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

		public async Task Reset()
		{
			UserRequest = _mapper.Map<UserRequest>(User);
			ShowLoadingIcon = false;
			RunLoadingIcon = false;
			Message = "Wypełnij wymagane pola.";
			MessageColor = Colors.Green;
			BlockInteraction = false;
			ShowMessage = false;

		}

		private async Task EditUser()
		{
			_logger.LogInformation(
				"Rozpoczynam edycji konta użytkownika (UI). Email: {Email}",
				UserRequest.Email
			);

			KeyboardHelper.HideKeyboard();

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				var response = await _service.EditUser(UserRequest);

				_logger.LogInformation(
					"Wynik edycji konta użytkownika (UI) {Email}: {StatusCode}",
					UserRequest.Email,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Edycja konta użytkownika (UI) nie powiodło się. Email: {Email}, StatusCode: {StatusCode}, Content: {Content}",
						UserRequest.Email,
						response.StatusCode,
						response.Content
					);

					Message = "Edycja konta nie powiodło się. Spróbuj później.";
					MessageColor = Colors.Red;
					ShowMessage = true;
					return;
				}

				_logger.LogInformation(
					"Edycja konta użytkownika (UI) zakończone sukcesem. Email: {Email}",
					UserRequest.Email
				);

				MessageColor = Colors.Green;
				ShowMessage = true;
				Message = "Edycja konta powiodła się.";
				User.FirstName = UserRequest.FirstName;
				User.LastName = UserRequest.LastName;
				User.Email = UserRequest.Email;

			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji konta użytkownika (UI). Email: {Email}",
					UserRequest.Email
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji konta użytkownika (UI). Email: {Email}",
					UserRequest.Email
				);
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces edycji konta użytkownika (UI). Email: {Email}",
					UserRequest.Email
				);
			}
		}

		private async Task DeleteAccount()
		{
			KeyboardHelper.HideKeyboard();

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				bool answer = await Application.Current.MainPage.DisplayAlert(
				"",
				$"Czy na pewno usunąc konto? Ta operacja nie może być odwołana!!!",
				"Tak",
				"Nie"
			);

				if (!answer)
				{
					return;
				}

				var result = await _service.DeleteUser();

				if (!result.IsSuccessStatusCode)
				{
					ShowMessage = true;
					MessageColor = Colors.Red;
					Message = "Usuwanie konta nie powiodło się. Spróbuj póxniej.";

				}

				await Application.Current.MainPage.DisplayAlert(
				   "",
				   $"Konto zostało usunięte.",
				   "Ok"
			   );

				Application.Current.MainPage = new AppShellLogin();
			}



			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces edycji konta użytkownika (UI). Email: {Email}",
					UserRequest.Email
				);
			}

		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



	}
}
