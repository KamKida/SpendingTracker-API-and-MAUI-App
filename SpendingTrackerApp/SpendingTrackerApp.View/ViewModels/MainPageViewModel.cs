using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.AddShells;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.FundsPages;
using SpendingTrackerApp.Pages.SpendingCategoryPages;
using SpendingTrackerApp.Pages.SpendingPages;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public User _user { get; set; }
		private readonly ILogger<MainPageViewModel> _logger;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly JsonService _jsonService;
		private bool _showLoadingIcon = false;
		private bool _runLoadingIcon = false;
		private bool _blockInteraction = false;

		private decimal _difference;
		private Color _differenceColor;

		private string _title;
		public string Title
		{
			get => _title;
			set
			{
				if (_title != value)
				{
					_title = value;
					OnPropertyChanged(nameof(Title));
				}
			}
		}

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

		private bool BlockInteraction
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

		public decimal Difference
		{
			get => _difference;
			set
			{
				if (_difference != value)
				{
					_difference = value;
					OnPropertyChanged(nameof(Difference));
				}
			}
		}

		public Color DifferenceColor
		{
			get => _differenceColor;
			set
			{
				if (_differenceColor != value)
				{
					_differenceColor = value;
					OnPropertyChanged(nameof(DifferenceColor));
				}
			}
		}

		public MainPageViewModel(
		IUserService userService,
		IMapper mapper,
		JsonService jsonService,
		ILogger<MainPageViewModel> logger)
		{
			_logger = logger;
			_userService = userService;
			_jsonService = jsonService;
			_mapper = mapper;
			LogOffCommand = new Command(async () => await LogOff());
			GoToFundsHistoryCommand = new Command(async () => await GoToFundsHistory());
		}
		public ICommand LogOffCommand { get; }
		public ICommand GoToFundsHistoryCommand { get; }


		public async Task Reset(){
			
		var response = await _userService.GetBaseInfo();

		if(!response.IsSuccessStatusCode){

		}
			var userResponse = _jsonService.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync());

			User = _mapper.Map<User>(userResponse);
			Title = $"Witaj {User.FirstName} {User.LastName}";
		}


		private async Task GoToFundsHistory()
		{
			_logger.LogInformation("Rozpoczynam nawigację do historii środków.");

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				await Shell.Current.GoToAsync($"///{nameof(FundsHistoryPage)}", true);

				_logger.LogInformation(
					"Nawigacja do historii środków zakończona sukcesem."
				);

			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Błąd podczas nawigacji do historii środków."
				);
				throw;
			}
			finally
			{

				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		}

		public async Task LogOff(){

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{


				Application.Current.MainPage = new AppShellLogin();

			}
			catch (Exception ex)
			{
				
				throw;
			}
			finally
			{

				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;
			}
		} 


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
