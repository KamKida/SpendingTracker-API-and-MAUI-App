using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class CreateAccountViewModel : INotifyPropertyChanged
    {
        public User User { get; set; }

        private readonly IUserService _service;
        private readonly IMapper _mapper;

        private bool _hidePassword = true;
        private string _passwordIcon = "hide_password.png";

        private string _message = "Wypełnij wymagane pola.";
        private string _messageColor = "Green";

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

        public string MessageColor
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
            User user,
            IUserService service,
            IMapper mapper)
        {
            User = user;
            _service = service;
            _mapper = mapper;

            TogglePasswordCommand = new Command(TogglePassword);
            CreateAccountCommand = new Command(CreateUser);
        }

        private void TogglePassword()
        {
            HidePassword = !HidePassword;
            PasswordIcon = HidePassword ? "hide_password.png" : "show_password.png";
        }

        private async void CreateUser()
        {
            ShowLoadingIcon = true;
            RunLoadingIcon = true;
            BlockInteraction = true;

            UserRequest request = _mapper.Map<UserRequest>(User);
            var response = await _service.CreateUser(request);

            if (response.StatusCode != 200)
            {
                Message = response.Content;
                MessageColor = "Red";
            }
            else
            {
                MessageColor = "Green";
                Message = "Konto zostało utworzone.";
            }

            ShowLoadingIcon = false;
            RunLoadingIcon = false;
            BlockInteraction = false;
        }

        public void Reset()
        {
            ShowLoadingIcon = false;
            RunLoadingIcon = false;
            BlockInteraction = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        
    }
}
