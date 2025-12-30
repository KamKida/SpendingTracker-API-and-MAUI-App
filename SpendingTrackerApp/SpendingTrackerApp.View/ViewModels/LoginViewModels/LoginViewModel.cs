using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Extensions;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages;
using System.ComponentModel;
using System.Windows.Input;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IUserService _service;
    BaseHttpService Http {  get; set; }

    public UserRequest Request {  get; set; }

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
    public ICommand GoToResePasswordPageComand {  get; }

    public LoginViewModel(
        BaseHttpService http,
        IUserService service)
    {
        Request = new UserRequest() { Email ="test@test.pl", Password="testte"};
        Http = http;
        _service = service;

        TogglePasswordCommand = new Command(TogglePassword);
        LoginUserCommand = new Command(LoginUser);
        GoToCreateAccountPageCommand = new Command(GoToCreateAccountPage);
        GoToResePasswordPageComand = new Command(GoToResePasswordPage);
    }

    private void TogglePassword()
    {
        HidePassword = !HidePassword;
        PasswordIcon = HidePassword ? "hide_password.png" : "show_password.png";
    }

    private async void LoginUser()
    {
        KeyboardHelper.HideKeyboard();

        ShowLoadingIcon = true;
        RunLoadingIcon = true;
        BlockInteraction = true;

        var response = await _service.LoginUser(Request);

        if (response.StatusCode != 200)
        {
            ErrorMessage = response.Content;
            ShowErrorMessage = true;

            ShowLoadingIcon = false;
            RunLoadingIcon = false;
            BlockInteraction = false;
        }
        else
        {
            Http.SetToken(response.Content);

            await Shell.Current.GoToAsync(nameof(LoadingDataPage));
        }
    }

    public async void GoToCreateAccountPage()
    {
        await Shell.Current.GoToAsync(nameof(CreateAcountPage));
    }

    public async void GoToResePasswordPage()
    {
        await Shell.Current.GoToAsync(nameof(EditPasswordPage));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
