using System;

namespace SpendingTrackerApp.Domain.Models
{
	public class Fund
	{
		public Guid Id { get; set; }
		public FundCategory? FundCategory { get; set; }
		public decimal Amount { get; set; }
		public DateTime CreationDate { get; set; }
		public string? Description { get; set; }
	}
}
