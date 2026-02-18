using Microsoft.Extensions.Logging;
using SpendingTrackerApp.AddShells;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.LoginPages;
using System.ComponentModel;
using System.Windows.Input;

public class LoginViewModel : INotifyPropertyChanged
{
	private readonly IUserService _userService;
	private readonly ILogger<LoginViewModel> _logger;
	private BaseHttpService _http;

	private UserRequest _userRequest;

	private bool _hidePassword;
	private string _passwordIcon;

	private bool _showErrorMessage;
	private string _errorMessage;

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

	public string ErrorMessage
	{
		get => _errorMessage;
		set
		{
			if (_errorMessage != value)
			{
				_errorMessage = value;
				OnPropertyChanged(nameof(ErrorMessage));
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
	public ICommand LoginUserCommand { get; }
	public ICommand GoToCreateAccountPageCommand { get; }
	public ICommand GoToResetPasswordPageCommand { get; }

	public LoginViewModel(
		BaseHttpService http,
		IUserService userService,
		ILogger<LoginViewModel> logger)
	{
		_http = http;
		_userService = userService;
		_logger = logger;

		TogglePasswordCommand = new Command(async () => await TogglePassword());
		LoginUserCommand = new Command(async () => await LoginUser());
		GoToCreateAccountPageCommand = new Command(async () => await GoToCreateAccountPage());
		GoToResetPasswordPageCommand = new Command(async () => await GoToResetPasswordPage());
	}

	public async Task Reset()
	{
		_logger.LogInformation("Rozpoczynam reset formularza logowania (UI).");

		ShowLoadingIcon = true;
		RunLoadingIcon = true;
		BlockInteraction = true;

		try
		{
			UserRequest = new UserRequest() { Email = "test@test.pl", Password = "inne22" };

			HidePassword = true;
			PasswordIcon = "show_password.png";
			ShowErrorMessage = false;
			ErrorMessage = string.Empty;
		}
		finally
		{
			ShowLoadingIcon = false;
			RunLoadingIcon = false;
			BlockInteraction = false;

			_logger.LogInformation("Zakończono reset formularza logowania (UI).");
		}
	}


	public async Task GoToCreateAccountPage()
	{
		_logger.LogInformation("Rozpoczynam nawigację do strony tworzenia konta.");

		ShowLoadingIcon = true;
		RunLoadingIcon = true;
		BlockInteraction = true;

		try
		{
			await Shell.Current.GoToAsync(nameof(CreateAcountPage));

			_logger.LogInformation("Nawigacja do strony tworzenia konta zakończona sukcesem.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas nawigacji do strony tworzenia konta.");
			throw;
		}
		finally
		{
			ShowLoadingIcon = false;
			RunLoadingIcon = false;
			BlockInteraction = false;
		}
	}

	public async Task GoToResetPasswordPage()
	{
		_logger.LogInformation("Rozpoczynam nawigację do strony resetu hasła.");

		ShowLoadingIcon = true;
		RunLoadingIcon = true;
		BlockInteraction = true;

		try
		{
			await Shell.Current.GoToAsync(nameof(ResetPasswordPage));

			_logger.LogInformation("Nawigacja do strony resetu hasła zakończona sukcesem.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas nawigacji do strony resetu hasła.");
			throw;
		}
		finally
		{
			ShowLoadingIcon = false;
			RunLoadingIcon = false;
			BlockInteraction = false;
		}
	}

	private async Task LoginUser()
	{
		_logger.LogInformation(
			"Rozpoczynam logowanie użytkownika (UI). Email: {Email}",
			UserRequest.Email
		);

		KeyboardHelper.HideKeyboard();

		ShowLoadingIcon = true;
		RunLoadingIcon = true;
		BlockInteraction = true;

		try
		{
			var response = await _userService.LoginUser(UserRequest);

			_logger.LogInformation(
				"Wynik logowania użytkownika (UI) {Email}: {StatusCode}",
				UserRequest.Email,
				response.StatusCode
			);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning(
					"Logowanie użytkownika (UI) nie powiodło się. Email: {Email}, StatusCode: {StatusCode}",
					UserRequest.Email,
					response.StatusCode
				);

				ErrorMessage = "Nie znaleziono konta lub hasło jest błędne. Spróbuj jeszcze raz lub później";
				ShowErrorMessage = true;
				return;
			}

			_logger.LogInformation(
				"Logowanie użytkownika (UI) zakończone sukcesem. Ustawiam token. Email: {Email}",
				UserRequest.Email
			);

			_http.SetToken(await response.Content.ReadAsStringAsync());

			Application.Current.MainPage = new AppShellMain();
		}
		catch (HttpRequestException httpEx)
		{
			_logger.LogError(
				httpEx,
				"Błąd HTTP podczas logowania użytkownika (UI). Email: {Email}",
				UserRequest.Email
			);
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(
				ex,
				"Nieoczekiwany błąd podczas logowania użytkownika (UI). Email: {Email}",
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
				"Zakończono proces logowania użytkownika (UI). Email: {Email}",
				UserRequest.Email
			);
		}
	}


	private async Task TogglePassword()
	{
		_logger.LogInformation(
			"Rozpoczynam przełączanie widoczności hasła. HidePassword: {HidePassword}",
			HidePassword
		);

		ShowLoadingIcon = true;
		RunLoadingIcon = true;
		BlockInteraction = true;

		try
		{
			HidePassword = !HidePassword;
			PasswordIcon = HidePassword ? "show_password.png" : "hide_password.png";
		}
		finally
		{
			ShowLoadingIcon = false;
			RunLoadingIcon = false;
			BlockInteraction = false;

			_logger.LogInformation(
				"Zakończono przełączanie widoczności hasła. HidePassword: {HidePassword}, Icon: {Icon}",
				HidePassword,
				PasswordIcon
			);
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}