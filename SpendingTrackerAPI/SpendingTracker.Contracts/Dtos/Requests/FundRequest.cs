using SpendingTracker.Domain.Models;

namespace SpendingTracker.Contracts.Dtos.Requests
{
	public class FundRequest
	{
		public Guid Id { get; set; }
		public FundCategory? FundCategory { get; set; }
		public decimal Amount { get; set; }
	}
}
