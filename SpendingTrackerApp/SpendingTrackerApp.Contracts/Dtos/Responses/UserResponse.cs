namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class UserResponse
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public decimal ThisMonthFundSum { get; set; }
		public decimal ThisMonthSpendingsSum { get; set; }
		public List<SpendingResponse> SpendingReponses { get; set; } = new List<SpendingResponse>();
		public List<SpendingCategoryResponse> SpendingCategoryResponses { get; set; } = new List<SpendingCategoryResponse>();
	}
}
