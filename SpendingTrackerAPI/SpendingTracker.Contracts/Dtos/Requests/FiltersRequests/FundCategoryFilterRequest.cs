namespace SpendingTracker.Contracts.Dtos.Requests.FiltersRequests
{
	public class FundCategoryFilterRequest
	{
		public string? Name { get; set; }
		public decimal? ShouldBeFrom { get; set; }
		public decimal? ShouldBeTo { get; set; }
		public DateTimeOffset? DateFrom { get; set; }
		public DateTimeOffset? DateTo { get; set; }
		public DateTimeOffset? LastDate { get; set; }
	}
}
