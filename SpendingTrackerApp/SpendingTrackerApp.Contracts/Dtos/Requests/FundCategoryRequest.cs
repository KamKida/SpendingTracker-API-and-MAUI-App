using SpendingTrackerApp.Domain.Models;
using System.ComponentModel;

namespace SpendingTrackerApp.Contracts.Dtos.Requests
{
	public class FundCategoryRequest
	{
		public Guid Id { get; set; }
		private string _name;
		private decimal? _shouldBe;

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
		protected void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	}
}
