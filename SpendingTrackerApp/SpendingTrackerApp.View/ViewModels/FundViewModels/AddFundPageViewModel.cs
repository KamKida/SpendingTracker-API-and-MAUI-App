using System.ComponentModel;

namespace SpendingTrackerApp.ViewModels.FundViewModels
{
	public class AddFundPageViewModel : INotifyPropertyChanged
	{
		



		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
