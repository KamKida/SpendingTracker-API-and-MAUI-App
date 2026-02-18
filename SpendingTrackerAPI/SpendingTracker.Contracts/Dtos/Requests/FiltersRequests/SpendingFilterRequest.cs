namespace SpendingTracker.Contracts.Dtos.Requests.FiltersRequests
{
	public class SpendingFilterRequest
	{
		public decimal? AmountFrom {  get; set; }
		public decimal? AmountTo { get; set; }
		public DateTimeOffset? DateFrom { get; set; }
		public DateTimeOffset? DateTo { get; set; }
		public DateTimeOffset? LastDate { get; set; }
		public Guid? SpendingCategoryId { get; set; }
	}
}
