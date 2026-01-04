using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages;
using System.ComponentModel;
using System.Windows.Input;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginViewModel> _logger;
    BaseHttpService _htttp {  get; set; }

    public UserRequest _userRequest {  get; set; }

    private bool _hidePassword = true;
    private string _passwordIcon = "hide_password.png";

    private bool _showErrorMessage = false;
    private string _errorMessage;

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
    public ICommand GoToCreateAccountPageCommand {  get; }
    public ICommand GoToResetPasswordPageCommand {  get; }

    public LoginViewModel(
        BaseHttpService http,
        IUserService userService,
		ILogger<LoginViewModel> logger)
    {
        _userRequest = new UserRequest() { Email ="test@test.pl", Password="testte"};
        _htttp = http;
        _userService = userService;
        _logger = logger;

        TogglePasswordCommand = new Command(async () => await TogglePassword());
		LoginUserCommand = new Command(async () => await LoginUser());
        GoToCreateAccountPageCommand = new Command(async () => await GoToCreateAccountPage());
        GoToResetPasswordPageCommand = new Command(async () => await GoToResetPasswordPage());
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
			"Zakończono przełączanie widoczności hasła. HidePassword: {HidePassword}, Icon: {Icon}",
			HidePassword,
			PasswordIcon
		);
	}


	private async Task LoginUser()
	{
		_logger.LogInformation(
			"Rozpoczynam logowanie użytkownika (UI). Email: {Email}",
			_userRequest.Email
		);

		KeyboardHelper.HideKeyboard();

		ShowLoadingIcon = true;
		RunLoadingIcon = true;
		BlockInteraction = true;

		try
		{
			var response = await _userService.LoginUser(_userRequest);

			_logger.LogInformation(
				"Wynik logowania użytkownika (UI) {Email}: {StatusCode}",
				_userRequest.Email,
				response.StatusCode
			);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning(
					"Logowanie użytkownika (UI) nie powiodło się. Email: {Email}, StatusCode: {StatusCode}",
					_userRequest.Email,
					response.StatusCode
				);

				ErrorMessage = "Coś poszło nie tak podczas logowania. Spróbuj później.";
				ShowErrorMessage = true;
				return;
			}

			_logger.LogInformation(
				"Logowanie użytkownika (UI) zakończone sukcesem. Ustawiam token. Email: {Email}",
				_userRequest.Email
			);

			_htttp.SetToken(await response.Content.ReadAsStringAsync());

			await Shell.Current.GoToAsync(nameof(LoadingDataPage));
		}
		catch (HttpRequestException httpEx)
		{
			_logger.LogError(
				httpEx,
				"Błąd HTTP podczas logowania użytkownika (UI). Email: {Email}",
				_userRequest.Email
			);
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(
				ex,
				"Nieoczekiwany błąd podczas logowania użytkownika (UI). Email: {Email}",
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
				"Zakończono proces logowania użytkownika (UI). Email: {Email}",
				_userRequest.Email
			);
		}
	}



	public async Task GoToCreateAccountPage()
	{
		_logger.LogInformation("Rozpoczynam nawigację do strony tworzenia konta.");

		try
		{
			await Shell.Current.GoToAsync(nameof(CreateAcountPage));

			_logger.LogInformation(
				"Nawigacja do strony tworzenia konta zakończona sukcesem."
			);
		}
		catch (Exception ex)
		{
			_logger.LogError(
				ex,
				"Błąd podczas nawigacji do strony tworzenia konta."
			);
			throw;
		}
	}

	public async Task GoToResetPasswordPage()
	{
		_logger.LogInformation("Rozpoczynam nawigację do strony resetu hasła.");

		try
		{
			await Shell.Current.GoToAsync(nameof(ResetPasswordPage));

			_logger.LogInformation(
				"Nawigacja do strony resetu hasła zakończona sukcesem."
			);
		}
		catch (Exception ex)
		{
			_logger.LogError(
				ex,
				"Błąd podczas nawigacji do strony resetu hasła."
			);
			throw;
		}
	}


	public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
