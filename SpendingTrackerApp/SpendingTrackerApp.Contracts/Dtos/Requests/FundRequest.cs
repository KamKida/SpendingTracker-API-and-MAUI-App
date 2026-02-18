namespace SpendingTrackerApp.Contracts.Dtos.Requests
{
	public class FundRequest
	{
		public Guid Id { get; set; }
		public Guid? FundCategoryId { get; set; }
		public string? Description { get; set; }
		public decimal Amount { get; set; }
	}
}
