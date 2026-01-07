namespace SpendingTracker.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Fund> Funds { get; set; } = new List<Fund>();
        public List<FundCategory> FundCategories { get; set; } = new List<FundCategory>();
        public List<Spending> Spendings { get; set; } = new List<Spending>();
        public List<SpendingCategory> SpendingCategories { get; set; } = new List<SpendingCategory>();

    }
}
