using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class ResetAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserEditRequest Request { get; set; }

        private bool _hidePassword = true;
        private string _passwordIcon = "hide_password.png";

        private string _message = "Wprować swój e-mail i nowe hasło.";
        private string _messageCollor = "Green";

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
            User user,
            IUserService service,
            IMapper mapper)
        {
            Request = new UserEditRequest();

            _service = service;
            _mapper = mapper;

            ResetAccountCommand = new Command(ResetAccount);

        }

        private async void ResetAccount()
        {
            ShowLoadingIcon = true;
            RunLoadingIcon = true;
            BlockInteraction = true;


            var response = await _service.ResetPassword(Request);

            if (response.StatusCode != 200)
            {
                Message = response.Content;
                MessageColor = "Red";

            }
            else
            {
                Message = "Hasło zostało zresetowane.";
                MessageColor = "Green";
            }

            ShowLoadingIcon = false;
            RunLoadingIcon = false;
            BlockInteraction = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
