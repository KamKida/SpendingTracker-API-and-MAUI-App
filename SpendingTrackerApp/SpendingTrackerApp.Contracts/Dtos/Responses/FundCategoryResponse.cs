namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class FundCategoryResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? ShouldBe { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
