using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages;
using System.ComponentModel;

namespace SpendingTrackerApp.ViewModels.LoginViewModels
{
    public class LoadingDataPageViewModel : INotifyPropertyChanged
    {
        public User _user { get;}
		public JsonService _jsonService { get; set;}
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
		private readonly ILogger<LoadingDataPageViewModel> _logger;

        public string _message { get; set; } = "Ładowanie danych....";
        public Color _textColor { get; set; } = Colors.Green;
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

		public Color TextColor
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
			IUserService service,
            IMapper mapper,
			ILogger<LoadingDataPageViewModel> logger)
        {
            _user = user;
			_jsonService = jsonService;
            _userService = service;
            _mapper = mapper;
			_logger = logger;

            GetBaseUserData();
        }

		private async Task GetBaseUserData()
		{
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych danych użytkownika (UI).");

			IsRunning = true;

			try
			{
				var response = await _userService.GetBaseInfo();

				_logger.LogInformation(
					"Wynik pobierania podstawowych danych użytkownika (UI): {StatusCode}",
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning(
						"Pobieranie danych użytkownika (UI) nie powiodło się. StatusCode: {StatusCode}, Content: {Content}",
						response.StatusCode,
						response.Content
					);

					IsRunning = false;
					TextColor = Colors.Red;
					Message = "Coś poszło nie tak. Spróbuj później.";
					return;
				}

				UserResponse userResponse = _jsonService.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync());
				_mapper.Map(userResponse, _user);

				_logger.LogInformation("Pobieranie danych użytkownika (UI) zakończone sukcesem. Przechodzę do MainPage.");

				await Shell.Current.GoToAsync(nameof(MainPage));
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania danych użytkownika (UI).");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania danych użytkownika (UI).");
				throw;
			}
			finally
			{
				IsRunning = false;
				_logger.LogInformation("Zakończono proces pobierania podstawowych danych użytkownika (UI).");
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
