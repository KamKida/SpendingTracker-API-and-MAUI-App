using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	public class FundsHistoryPageViewModel : INotifyPropertyChanged
	{
		private User _user;
		private ObservableCollection<Fund> _funds { get; set; }

		private IFundService _fundService { get; }
		private IMapper _mapper { get; }
		private JsonService _jsonService { get; }

		public ObservableCollection<Fund> Funds
		{
			get => _funds;
			set
			{
				if (_funds != value)
				{
					_funds = value;
					OnPropertyChanged(nameof(Funds));
				}
			}
		}
		public FundsHistoryPageViewModel(
		User user,
				JsonService jsonService,
			IFundService fundService,
			IMapper mapper
		)
		{
			_user = user;
			_fundService = fundService;
			_jsonService = jsonService;
			_mapper = mapper;

			DeleteFundCommand = new Command<Fund>(async (fund) => await DeleteFund(fund));
		}

		public ICommand DeleteFundCommand { get; }

		public async Task GetTop10Funds()
		{
			var response = await _fundService.GetTop10();

			if (response.StatusCode != 200)
			{

			}
			else
			{
				ObservableCollection<FundResponse> fundResponses = _jsonService.Deserialize<ObservableCollection<FundResponse>>(response.Content);

				Funds = _mapper.Map<ObservableCollection<Fund>>(fundResponses);
			}
		}

		private async Task DeleteFund(Fund fund)
		{
			bool answare = await Application.Current.MainPage.DisplayAlert("", $"Czy napewno usunąć fundusz z kwotą: {fund.Amount} zł. \n Dodanego: {fund.CreationDate} ?", "Tak", "Nie");

			if (answare)
			{
				var response = await _fundService.DeleteFund(fund.Id);

				if (response.StatusCode != 200)
				{

				}
				else
				{
					Funds.Remove(fund);
					_user.ThisMonthFund -= fund.Amount;

					await Application.Current.MainPage.DisplayAlert("", "Fundusz został usunięty", "Ok");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
