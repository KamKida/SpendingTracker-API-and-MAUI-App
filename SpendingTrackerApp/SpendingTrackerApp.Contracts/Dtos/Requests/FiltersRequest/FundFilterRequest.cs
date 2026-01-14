using System.ComponentModel;

namespace SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest
{
	public class FundFilterRequest : INotifyPropertyChanged
	{
		private decimal? _amountFrom;
		private decimal? _amountTo;
		private DateTime? _dateFrom = DateTime.Now;
		private DateTime? _dateTo = DateTime.Now;
		private DateTime? _lastDate;
		private Guid? _fundCategoryId;


		public decimal? AmountFrom
		{
			get => _amountFrom;
			set
			{
				if (_amountFrom != value)
				{
					_amountFrom = value;
					OnPropertyChanged(nameof(AmountFrom));
				}
			}
		}

		public decimal? AmountTo
		{
			get => _amountTo;
			set
			{
				if (_amountTo != value)
				{
					_amountTo = value;
					OnPropertyChanged(nameof(AmountTo));
				}
			}
		}

		public DateTime? DateFrom
		{
			get => _dateFrom;
			set
			{
				if (_dateFrom != value)
				{
					_dateFrom = value;
					OnPropertyChanged(nameof(DateFrom));
				}
			}
		}

		public DateTime? DateTo
		{
			get => _dateTo;
			set
			{
				if (_dateTo != value)
				{
					_dateTo = value;
					OnPropertyChanged(nameof(DateTo));
				}
			}
		}

		public DateTime? LastDate
		{
			get => _lastDate;
			set
			{
				if (_lastDate != value)
				{
					_lastDate = value;
					OnPropertyChanged(nameof(LastDate));
				}
			}
		}

		public Guid? FundCategoryId
		{
			get => _fundCategoryId;
			set
			{
				if (_fundCategoryId != value)
				{
					_fundCategoryId = value;
					OnPropertyChanged(nameof(FundCategoryId));
				}
			}
		}
		public void Reset()
		{
			AmountFrom = null;
			AmountTo = null;
			DateFrom = DateTime.Now;
			DateTo = DateTime.Now;
		}

		public FundFilterRequest Clone()
		{
			return new FundFilterRequest
			{
				DateFrom = DateFrom,
				DateTo = DateTo,
				AmountFrom = AmountFrom,
				AmountTo = AmountTo,
				LastDate = LastDate
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
