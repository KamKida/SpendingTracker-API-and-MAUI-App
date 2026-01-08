using System.ComponentModel;

namespace SpendingTrackerApp.Domain.Models
{
	public class Fund : INotifyPropertyChanged
	{
		public Guid Id { get; set; }
		private FundCategory? _fundCategory;
		private decimal _amount;
		private DateTime _creationDate;
		private string? _description;

		public decimal Amount
		{
			get => _amount;
			set

			{
				if (_amount != value)
				{
					_amount = value;
					OnPropertyChanged(nameof(Amount));
				}
			}
		}

		public DateTime CreationDate
		{
			get => _creationDate;
			set

			{
				if (_creationDate != value)
				{
					_creationDate = value;
					OnPropertyChanged(nameof(CreationDate));
				}
			}
		}

		public FundCategory? FundCategory
		{
			get => _fundCategory;
			set
			{
				if (_fundCategory != value)
				{
					_fundCategory = value;
					OnPropertyChanged(nameof(FundCategory));
				}
			}
		}

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

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	}
}
