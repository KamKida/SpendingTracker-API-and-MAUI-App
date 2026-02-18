using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.AddShells;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Infrastructure.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.UserPagesViewModels
{
	public class EditUserPageViewModel : INotifyPropertyChanged
	{
		private User _user = new User();
		private UserRequest _userRequest = new UserRequest();

		private readonly IUserService _userService;
		private readonly JsonService _jsonService;
		private readonly IMapper _mapper;
		private readonly ILogger<EditUserPageViewModel> _logger;

		private bool _hidePassword;
		private string _passwordIcon;

		private string _message;
		private Color _messageColor;
		private bool _showMessage;

		private bool _showLoadingIcon;
		private bool _runLoadingIcon;
		private bool _blockInteraction;

		public User User
		{
			get => _user;
			set
			{
				if (_user != value)
				{
					_user = value;
					OnPropertyChanged(nameof(User));
				}
			}
		}

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

		public EditUserPageViewModel(
			User user,
			JsonService service,
			IUserService userService,
			IMapper mapper,
			ILogger<EditUserPageViewModel> logger)
		{
			_user = user;
			_jsonService = service;
			_userService = userService;
			_mapper = mapper;
			_logger = logger;

			TogglePasswordCommand = new Command(async () => await TogglePassword());
			EditAccountCommand = new Command(async () => await EditUser());
			DeleteAccountCommand = new Command(async () => await DeleteAccount());
		}

		public ICommand TogglePasswordCommand { get; }
		public ICommand EditAccountCommand { get; }
		public ICommand DeleteAccountCommand { get; }

		private async Task TogglePassword()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam przełączanie widoczności hasła. Aktualny stan HidePassword: {HidePassword}",
				HidePassword
			);

			try
			{
				HidePassword = !HidePassword;
				PasswordIcon = !HidePassword ? "hide_password.png" : "show_password.png";

				_logger.LogInformation(
					"Zakończono przełączanie widoczności hasła. Nowy stan HidePassword: {HidePassword}, Ikona: {Icon}",
					HidePassword,
					PasswordIcon
				);
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task Reset()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation("Rozpoczynam reset formularza użytkownika.");

			try
			{
				var response = await _userService.GetUserBaseData();

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobranie danych użytkownika nie powiodło się. StatusCode: {StatusCode}",
						response.StatusCode
					);
					return;
				}

				var userResponse = _jsonService.Deserialize<UserResponse>(
					await response.Content.ReadAsStringAsync()
				);

				User = _mapper.Map<User>(userResponse);

				UserRequest = _mapper.Map<UserRequest>(User);

				Message = "Wypełnij wymagane pola.";
				MessageColor = Colors.Green;
				ShowMessage = false;

				HidePassword = true;
				PasswordIcon = "show_password.png";
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation("Zakończono reset formularza użytkownika.");
			}
		}


		private async Task EditUser()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam edycję konta użytkownika. Email: {Email}",
				UserRequest.Email
			);

			KeyboardHelper.HideKeyboard();

			try
			{
				var response = await _userService.EditUser(UserRequest);

				_logger.LogInformation(
					"Wynik edycji konta użytkownika. Email: {Email}, StatusCode: {StatusCode}",
					UserRequest.Email,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Edycja konta użytkownika nie powiodła się. Email: {Email}, StatusCode: {StatusCode}, Content: {Content}",
						UserRequest.Email,
						response.StatusCode,
						response.Content
					);

					Message = "Edycja konta nie powiodła się. Spróbuj później.";
					MessageColor = Colors.Red;
					ShowMessage = true;
					return;
				}

				User.FirstName = UserRequest.FirstName;
				User.LastName = UserRequest.LastName;
				User.Email = UserRequest.Email;

				Message = "Edycja konta powiodła się.";
				MessageColor = Colors.Green;
				ShowMessage = true;

				_logger.LogInformation(
					"Edycja konta użytkownika zakończona sukcesem. Email: {Email}",
					UserRequest.Email
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji konta użytkownika. Email: {Email}",
					UserRequest.Email
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji konta użytkownika. Email: {Email}",
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
					"Zakończono proces edycji konta użytkownika. Email: {Email}",
					UserRequest.Email
				);
			}
		}

		private async Task DeleteAccount()
		{
			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			_logger.LogInformation(
				"Rozpoczynam proces usuwania konta użytkownika. Email: {Email}",
				UserRequest.Email
			);

			KeyboardHelper.HideKeyboard();

			try
			{
				bool answer = await Application.Current.MainPage.DisplayAlert(
					"",
					"Czy na pewno usunąć konto? Ta operacja nie może być odwołana!",
					"Tak",
					"Nie"
				);

				if (!answer)
				{
					_logger.LogInformation("Użytkownik anulował usuwanie konta. Email: {Email}", UserRequest.Email);
					return;
				}

				var result = await _userService.DeleteUser();

				if (!result.IsSuccessStatusCode)
				{
					Message = "Usuwanie konta nie powiodło się. Spróbuj później.";
					MessageColor = Colors.Red;
					ShowMessage = true;

					_logger.LogWarning(
						"Usuwanie konta użytkownika nie powiodło się. Email: {Email}, StatusCode: {StatusCode}",
						UserRequest.Email,
						result.StatusCode
					);

					return;
				}

				await Application.Current.MainPage.DisplayAlert(
					"",
					"Konto zostało usunięte.",
					"Ok"
				);

				_logger.LogInformation("Konto użytkownika zostało usunięte. Email: {Email}", UserRequest.Email);

				Application.Current.MainPage = new AppShellLogin();
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces usuwania konta użytkownika. Email: {Email}",
					UserRequest.Email
				);
			}
		}
		
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}