using AutoMapper;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using SpendingTrackerApp.Pages.FundsPages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	public class FundsHistoryPageViewModel : INotifyPropertyChanged
	{
		private User _user;
		private ObservableCollection<Fund> _funds { get; set; }
		private FundFilterRequest _filterRequest { get; set; }

		private bool _isMoreActive { get; set; } = true;

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

		public FundFilterRequest FilterRequest
		{
			get => _filterRequest; set
			{
				if (_filterRequest != value)
				{
					_filterRequest = value;
					OnPropertyChanged(nameof(FilterRequest));
				}
			}
		}


		public bool IsMoreActive
		{
			get => _isMoreActive;
			set
			{

				if (_isMoreActive != value)
				{
					_isMoreActive = value;
					OnPropertyChanged(nameof(IsMoreActive));
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
			_filterRequest = new FundFilterRequest();

			DeleteFundCommand = new Command<Fund>(async (fund) => await DeleteFund(fund));
			GetTop10FilteredFundsCommand = new Command(async () => await GetTop10FilteredFunds());
			GetNext10FundCommand = new Command(async () => await GetNext10Fund());
			SelectFundCommand = new Command<Fund>(async (fund) => await SelectFund(fund));
			ResetFilterCommand = new Command(async () => await ResetFilter());
		}

		public ICommand DeleteFundCommand { get; }
		public ICommand GetTop10FilteredFundsCommand { get; }
		public ICommand GetNext10FundCommand { get; }
		public ICommand SelectFundCommand { get; }
		public ICommand ResetFilterCommand { get; }

		public async Task GetTop10Funds()
		{
			var response = await _fundService.Get10(FilterRequest);

			if (!response.IsSuccessStatusCode)
			{

			}
			else
			{
				ObservableCollection<FundResponse> fundResponses = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());

				Funds = _mapper.Map<ObservableCollection<Fund>>(fundResponses);


				if (Funds.Count % 10 != 0)
				{
					IsMoreActive = false;
				}
				else
				{
					IsMoreActive = true;
				}
			}
		}

		private async Task GetNext10Fund()
		{
			FilterRequest.LastDate = Funds.Last().CreationDate;

			var response = await _fundService.Get10(FilterRequest);

			if (!response.IsSuccessStatusCode)
			{

			}
			else
			{
				int oldFundsCount = Funds.Count;

				ObservableCollection<FundResponse> fundResponse = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());
				ObservableCollection<Fund> next10Funds = _mapper.Map<ObservableCollection<Fund>>(fundResponse);
				Funds = new ObservableCollection<Fund>(Funds.Concat(next10Funds).OrderByDescending(f => f.CreationDate));

				if (Funds.Count % 10 != 0 || Funds.Count == oldFundsCount)
				{
					IsMoreActive = false;
				}
				else
				{
					IsMoreActive = true;
				}
			}

			FilterRequest.LastDate = null;
		}

		private async Task GetTop10FilteredFunds()
		{
			var response = await _fundService.Get10(FilterRequest);
			if (!response.IsSuccessStatusCode)
			{

			}
			else
			{
				int oldFundsCount = Funds.Count;

				ObservableCollection<FundResponse> fundResponse = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());
				ObservableCollection<Fund> top10FilteredFunds = _mapper.Map<ObservableCollection<Fund>>(fundResponse);
				Funds = top10FilteredFunds;

				if (Funds.Count % 10 != 0 || Funds.Count == oldFundsCount)
				{
					IsMoreActive = false;
				}
				else
				{
					IsMoreActive = true;
				}
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

		private async Task ResetFilter()
		{
			FilterRequest.Reset();

			var response = await _fundService.Get10(FilterRequest);

			if (!response.IsSuccessStatusCode)
			{

			}
			else
			{
				ObservableCollection<FundResponse> fundResponses = _jsonService.Deserialize<ObservableCollection<FundResponse>>(await response.Content.ReadAsStringAsync());

				Funds = _mapper.Map<ObservableCollection<Fund>>(fundResponses);


				if (Funds.Count % 10 != 0)
				{
					IsMoreActive = false;
				}
				else
				{
					IsMoreActive = true;
				}
			}
		}

		public async Task SelectFund(Fund fund)
		{
			await Shell.Current.GoToAsync($"{nameof(EditFundPage)}", new Dictionary<string, object>
			{
				[nameof(Fund)] = fund
			});
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
