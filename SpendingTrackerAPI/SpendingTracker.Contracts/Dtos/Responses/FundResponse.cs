using SpendingTracker.Domain.Models;

namespace SpendingTracker.Contracts.Dtos.Responses
{
    public class FundResponse
    {
		public Guid Id { get; set; }
		public FundCategory? FundCategory { get; set; }
        public Guid? FundCategoryId { get; set; }
        public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
    }
}
