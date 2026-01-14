using System.ComponentModel;

namespace SpendingTrackerApp.Domain.Models
{
	public class FundCategory : INotifyPropertyChanged
	{
		public Guid Id { get; set; }
		private string _name;
		private decimal? _shouldBe;
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

		public decimal? ShouldBe
		{
			get => _shouldBe;
			set
			{
				if (_shouldBe != value)
				{
					_shouldBe = value;
					OnPropertyChanged(nameof(ShouldBe));
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string prop) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
	}
}
