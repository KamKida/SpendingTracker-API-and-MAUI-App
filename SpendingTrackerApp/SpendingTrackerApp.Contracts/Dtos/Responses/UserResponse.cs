namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class UserResponse
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public decimal ThisMonthFund { get; set; }
	}
}
