namespace SpendingTracker.Domain.Models
{
	public class Spending
	{
		public Guid Id { get; set; }
		public User User { get; set; }
		public Guid UserId { get; set; }
		public SpendingCategory? SpendingCategory { get; set; }
		public Guid? SpendingCategoryId { get; set; }
		public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
