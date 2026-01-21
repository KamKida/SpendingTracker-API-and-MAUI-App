using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SpendingTrackerApp.Domain.Models
{
	public class User : INotifyPropertyChanged
	{
		private string _email;
		private string _password;
		private string _token;
		private string _firstName;
		private string _lastName;
		private decimal _thisMonthFundSum;
		private decimal _thisMonthSpendingsSum;
		private List<SpendingCategory> _spendingCategories = new List<SpendingCategory>();
		private List<Spending> _spendings = new List<Spending>();

		public string Email
		{
			get => _email;
			set
			{
				if (_email != value)
				{
					_email = value;
					OnPropertyChanged(nameof(Email));
				}
			}
		}
		public string Password
		{
			get => _password;
			set
			{
				if (_password != value)
				{
					_password = value;
					OnPropertyChanged(nameof(Password));
				}
			}
		}
		public string Token
		{
			get => _token;
			set
			{
				if (_token != value)
				{
					_token = value;
					OnPropertyChanged(nameof(Token));
				}
			}
		}
		public string? FirstName
		{
			get => _firstName;
			set
			{
				if (_firstName != value)
				{
					_firstName = value;
					OnPropertyChanged(nameof(FirstName));
				}
			}
		}
		public string? LastName
		{
			get => _lastName;
			set
			{
				if (_lastName != value)
				{
					_lastName = value;
					OnPropertyChanged(nameof(LastName));
				}
			}
		}

		public decimal ThisMonthFundSum
		{
			get => _thisMonthFundSum;
			set
			{
				if (_thisMonthFundSum != value)
				{
					_thisMonthFundSum = value;
					OnPropertyChanged(nameof(ThisMonthFundSum));
				}
			}
		}

		public decimal ThisMonthSpendingsSum
		{
			get => _thisMonthSpendingsSum;
			set
			{
				if (_thisMonthSpendingsSum != value)
				{
					_thisMonthSpendingsSum = value;
					OnPropertyChanged(nameof(ThisMonthSpendingsSum));
				}
			}
		}

		public List<SpendingCategory> SpendingCategories
		{
			get => _spendingCategories;
			set
			{
				if (_spendingCategories != value)
				{
					_spendingCategories = value;
					OnPropertyChanged(nameof(SpendingCategories));
				}
			}
		}

		public List<Spending> Spendings
		{
			get => _spendings;
			set
			{
				if (_spendings != value)
				{
					_spendings = value;
					OnPropertyChanged(nameof(Spendings));
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	}
}
