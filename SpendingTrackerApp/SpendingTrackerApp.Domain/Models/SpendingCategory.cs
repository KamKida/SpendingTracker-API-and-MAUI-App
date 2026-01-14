using System.ComponentModel;

namespace SpendingTrackerApp.Domain.Models
{
	public class SpendingCategory : INotifyPropertyChanged
	{
		public Guid Id { get; set; }
		private string _name;
		private decimal? _weeklyLimit;
		private decimal? _monthlyLimit;
		public decimal? MonthlyLimitDiffrence { get; set; }
		public DateTime CreationDate { get; set; }
		private string? _description;

		public string? Description
		{
			get => _description;
			set
			{
				if (_description != value)
				{
					_description = value;
					OnPropertyChanged(nameof(Description));
				}
			}
		}

		public string Name
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

		public decimal? WeeklyLimit
		{
			get => _weeklyLimit;
			set
			{
				if (_weeklyLimit != value)
				{
					_weeklyLimit = value;
					OnPropertyChanged(nameof(WeeklyLimit));
				}
			}
		}

		public decimal? MonthlyLimit
		{
			get => _monthlyLimit;
			set
			{
				if (_monthlyLimit != value)
				{
					_monthlyLimit = value;
					OnPropertyChanged(nameof(MonthlyLimit));
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	}
}
