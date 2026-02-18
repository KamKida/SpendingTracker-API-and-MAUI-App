using System.ComponentModel;

namespace SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest
{
	public class FundCategoryFilterRequest : INotifyPropertyChanged
	{
		public string? _name { get; set; }
		public decimal? _shouldBeFrom { get; set; }
		public decimal? _shouldBeTo { get; set; }
		public DateTime? _dateFrom { get; set; }
		public DateTime? _dateTo { get; set; }
		public DateTime? _lastDate { get; set; }

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

		public decimal? ShouldBeFrom
		{
			get => _shouldBeFrom;
			set
			{
				if (_shouldBeFrom != value)
				{
					_shouldBeFrom = value;
					OnPropertyChanged(nameof(ShouldBeFrom));
				}
			}
		}

		public decimal? ShouldBeTo
		{
			get => _shouldBeTo;
			set
			{
				if (_shouldBeTo != value)
				{
					_shouldBeTo = value;
					OnPropertyChanged(nameof(ShouldBeTo));
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
			ShouldBeFrom = null;
			ShouldBeTo = null;
			DateFrom = DateTime.Now;
			DateTo = DateTime.Now;
		}

		public FundCategoryFilterRequest Clone()
		{
			return new FundCategoryFilterRequest
			{
				Name = Name,
				DateFrom = DateFrom,
				DateTo = DateTo,
				ShouldBeFrom = ShouldBeFrom,
				ShouldBeTo = ShouldBeTo,
				LastDate = LastDate
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
