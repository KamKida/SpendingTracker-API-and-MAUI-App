namespace SpendingTracker.Domain.Models
{
    public class FundCategory
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal? ShouldBe {  get; set; }
        public DateTime CreationDate { get; set; }
        public string? Description { get; set; }
        public List<Fund> Funds { get; set; } = new List<Fund>();
    }
}
