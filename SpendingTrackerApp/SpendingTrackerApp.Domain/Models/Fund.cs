using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTrackerApp.Domain.Models
{
	public class Fund : INotifyPropertyChanged
	{
		public decimal _amount { get; set; }
		public DateTime _creationDate { get; set; }

		public decimal Amount{ 
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

	public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	} }
