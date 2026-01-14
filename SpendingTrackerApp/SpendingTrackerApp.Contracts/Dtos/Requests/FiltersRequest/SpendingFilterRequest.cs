using System.ComponentModel;

namespace SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest
{
	public class SpendingFilterRequest : INotifyPropertyChanged
	{
		private decimal? _amountFrom;
		private decimal? _amountTo;
		private DateTime? _dateFrom = DateTime.Now;
		private DateTime? _dateTo = DateTime.Now;
		private DateTime? _lastDate;
		private Guid? _spendingCategoryId;


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


		public void Reset()
		{
			AmountFrom = null;
			AmountTo = null;
			DateFrom = DateTime.Now;
			DateTo = DateTime.Now;
		}

		public Guid? SpendingCategoryId
		{
			get => _spendingCategoryId;
			set
			{
				if (_spendingCategoryId != value)
				{
					_spendingCategoryId = value;
					OnPropertyChanged(nameof(SpendingCategoryId));
				}
			}
		}

		public SpendingFilterRequest Clone()
		{
			return new SpendingFilterRequest
			{
				DateFrom = DateFrom,
				DateTo = DateTo,
				AmountFrom = AmountFrom,
				AmountTo = AmountTo,
				LastDate = LastDate,
				SpendingCategoryId = SpendingCategoryId
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
