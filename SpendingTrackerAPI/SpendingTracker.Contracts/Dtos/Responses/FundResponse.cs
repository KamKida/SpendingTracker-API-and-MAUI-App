using SpendingTracker.Domain.Models;

namespace SpendingTracker.Contracts.Dtos.Responses
{
    public class FundResponse
    {
        public FundCategory? FundCategory { get; set; }
        public Guid? FundCategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
