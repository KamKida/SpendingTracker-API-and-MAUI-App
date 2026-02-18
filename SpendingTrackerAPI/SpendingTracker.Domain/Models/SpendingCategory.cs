namespace SpendingTracker.Domain.Models
{
	public class SpendingCategory
	{
		public Guid Id { get; set; }
		public User User { get; set; }
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public decimal? WeeklyLimit { get; set; }
		public decimal? MonthlyLimit { get; set; }
		public DateTime CreationDate { get; set; }
		public string? Description { get; set; }
		public List<Spending> Spendings { get; set; } = new List<Spending>();
	}
}
