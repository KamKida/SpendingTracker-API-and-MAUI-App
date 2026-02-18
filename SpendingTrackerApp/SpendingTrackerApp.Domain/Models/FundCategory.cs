using System;

namespace SpendingTrackerApp.Domain.Models
{
	public class FundCategory
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? ShouldBe { get; set; }
		public DateTime CreationDate { get; set; }
		public string? Description { get; set; }
	}
}
