using SpendingTrackerApp.Domain.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SpendingTrackerApp.Domain.HelpModels
{
	public class FundsList : INotifyPropertyChanged
	{
		private ObservableCollection<Fund> _funds { get; set; } = new ObservableCollection<Fund>();

		public ObservableCollection<Fund> Funds
		{
			get => _funds; set
			{
				if (_funds != value)
				{
					_funds = value;
					OnPropertyChanged(nameof(Funds));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	}
}
