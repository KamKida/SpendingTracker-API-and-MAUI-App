namespace SpendingTrackerApp.Contracts.Dtos.Requests
{
	public class SpendingRequest
	{
		public Guid Id { get; set; }
		public Guid? SpendingCategoryId { get; set; }
		public decimal Amount { get; set; }
		public string? Description { get; set; }
	}
}
