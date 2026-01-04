namespace SpendingTrackerApp.Contracts.Dtos.Requests
{
	public class FundCategoryRequest
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? ShouldBe { get; set; }


	}
}
