namespace SpendingTracker.Domain.Models
{
    public class Fund
    {
        public Guid Id { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
        public FundCategory? FundCategory { get; set; }
        public Guid? FundCategoryId {  get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
