using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages;
using System.ComponentModel;
using System.Text.Json;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class LoadingDataPageViewModel : INotifyPropertyChanged
    {
        public User User { get;}
		public JsonService JsonService { get; set;}
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
			JsonService jsonService,
			IUserService _service,
            IMapper mapper)
        {
            User = user;
			JsonService = jsonService;
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
				UserResponse userResponse = JsonService.Deserialize<UserResponse>(response.Content);
                _mapper.Map(userResponse, User);
				await Shell.Current.GoToAsync(nameof(MainPage));
			}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
