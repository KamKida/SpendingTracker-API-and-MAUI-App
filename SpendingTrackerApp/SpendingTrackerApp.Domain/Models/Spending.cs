using System;

namespace SpendingTrackerApp.Domain.Models
{
	public class Spending
	{
		public Guid Id { get; set; }
		public SpendingCategory? SpendingCategory { get; set; }
		public decimal Amount { get; set; }
		public DateTime CreationDate { get; set; }
		public string? Description { get; set; }
	}
}
