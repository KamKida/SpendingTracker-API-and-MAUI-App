using SpendingTracker.Domain.Models;

namespace SpendingTracker.Contracts.Dtos.Responses
{
	public class SpendingReponse
	{
		public Guid Id { get; set; }
		public SpendingCategory? SpendingCategory { get; set; }
		public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
