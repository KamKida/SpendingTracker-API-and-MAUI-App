namespace SpendingTrackerApp.Contracts.Dtos.Requests
{
	public class FundFilterRequest
	{
		public decimal? AmountFrom { get; set; }
		public decimal? AmountTo { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public DateTime? LastDate { get; set; }

		public void Reset()
		{
			AmountFrom = null;
			AmountTo = null;
			DateFrom = null;
			DateTo = null;
		}

	}
}
