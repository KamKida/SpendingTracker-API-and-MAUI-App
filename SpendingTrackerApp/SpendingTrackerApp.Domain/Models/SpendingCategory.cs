using System;

namespace SpendingTrackerApp.Domain.Models
{
	public class SpendingCategory
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? WeeklyLimit { get; set; }
		public decimal? MonthlyLimit { get; set; }
		public decimal? MonthlyLimitDiffrence { get; set; }
		public DateTime CreationDate { get; set; }
		public string? Description { get; set; }
	}
}
