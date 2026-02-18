using System.ComponentModel;

namespace SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest
{
	public class SpendingCategoryFilterRequest : INotifyPropertyChanged
	{
		private string? _name;
		private decimal? _weeklyeLimitFrom;
		private decimal? _weeklyeLimitTo;
		private decimal? _monthlyLimitFrom;
		private decimal? _monthlyLimitTo;
		private DateTime? _dateFrom;
		private DateTime? _dateTo;
		private DateTime? _lastDate;


		public string? Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
				}
			}
		}

		public decimal? WeeklyLimitFrom
		{
			get => _weeklyeLimitFrom;
			set
			{
				if (_weeklyeLimitFrom != value)
				{
					_weeklyeLimitFrom = value;
					OnPropertyChanged(nameof(WeeklyLimitFrom));
				}
			}
		}

		public decimal? WeeklyLimitTo
		{
			get => _weeklyeLimitTo;
			set
			{
				if (_weeklyeLimitTo != value)
				{
					_weeklyeLimitTo = value;
					OnPropertyChanged(nameof(WeeklyLimitTo));
				}
			}
		}

		public decimal? MonthlyLimitFrom
		{
			get => _monthlyLimitFrom;
			set
			{
				if (_monthlyLimitFrom != value)
				{
					_monthlyLimitFrom = value;
					OnPropertyChanged(nameof(MonthlyLimitFrom));
				}
			}
		}

		public decimal? MonthlyLimitTo
		{
			get => _monthlyLimitTo;
			set
			{
				if (_monthlyLimitTo != value)
				{
					_monthlyLimitTo = value;
					OnPropertyChanged(nameof(MonthlyLimitTo));
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
			Name = null;
			WeeklyLimitFrom = null;
			WeeklyLimitTo = null;
			MonthlyLimitFrom = null;
			MonthlyLimitTo = null;
			DateFrom = DateTime.Now;
			DateTo = DateTime.Now;
		}

		public SpendingCategoryFilterRequest Clone()
		{
			return new SpendingCategoryFilterRequest
			{
				Name = Name,
				DateFrom = DateFrom,
				DateTo = DateTo,
				WeeklyLimitFrom = WeeklyLimitFrom,
				WeeklyLimitTo = WeeklyLimitTo,
				MonthlyLimitFrom = MonthlyLimitFrom,
				MonthlyLimitTo = MonthlyLimitTo,
				LastDate = LastDate
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

