namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class FundResponse
	{
		public Guid Id { get; set; }
		public FundCategoryResponse? FundCategory { get; set; }
		public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
