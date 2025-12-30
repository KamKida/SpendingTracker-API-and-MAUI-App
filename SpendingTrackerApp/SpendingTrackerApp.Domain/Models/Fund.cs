using System.ComponentModel;

namespace SpendingTrackerApp.Domain.Models
{
	public class Fund : INotifyPropertyChanged
	{
	    public Guid Id {  get; set; }
		public decimal _amount { get; set; }
		public string _creationDate { get; set; }


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

		public string CreationDate
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

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	} }
