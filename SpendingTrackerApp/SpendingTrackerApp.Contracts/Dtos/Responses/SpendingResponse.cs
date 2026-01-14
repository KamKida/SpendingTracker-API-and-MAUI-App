using SpendingTrackerApp.Domain.Models;

namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class SpendingResponse
	{
		public Guid Id { get; set; }
		public SpendingCategory? SpendingCategory { get; set; }
		public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
