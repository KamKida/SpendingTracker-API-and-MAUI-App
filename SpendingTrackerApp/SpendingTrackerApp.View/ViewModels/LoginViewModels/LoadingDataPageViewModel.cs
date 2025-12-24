using AutoMapper;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class LoadingDataPageViewModel : INotifyPropertyChanged
    {
        public User User { get; set; }
        private readonly IUserService Service;
        private readonly IMapper _mapper;

        public string _message { get; set; } = "Ładowanie danych....";
        public string _textColor { get; set; } = "Black";
        public bool _isRunning { get; set; } = true;


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

		public string TextColor
		{
			get => _textColor;
			set
			{
				if (_textColor != value)
				{
					_textColor = value;
					OnPropertyChanged(nameof(TextColor));
				}
			}
		}

		public bool IsRunning
		{
			get => _isRunning;
			set
			{
				if (_isRunning != value)
				{
					_isRunning = value;
					OnPropertyChanged(nameof(IsRunning));
				}
			}
		}


		public LoadingDataPageViewModel(
            User user,
            IUserService _service,
            IMapper mapper)
        {
            User = user;
            Service = _service;
            _mapper = mapper;

            GetBaseUserData();
        }

        private async void GetBaseUserData()
        {
            var response = await Service.GetBaseInfo();

            if (response.StatusCode != 200)
            {
				IsRunning = false;
                TextColor = "Red";
                Message = "Coś poszło nie tak. Spróbuj później.";
			}
            else
            {
                User = _mapper.Map<User>(response);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
