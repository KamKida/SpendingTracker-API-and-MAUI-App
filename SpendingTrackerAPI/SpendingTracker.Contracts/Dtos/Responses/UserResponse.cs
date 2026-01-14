namespace SpendingTracker.Contracts.Dtos.Responses
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal ThisMonthFund { get; set; }
		public decimal ThisMonthSpendings { get; set; }
        public List<SpendingReponse> SpendingReponses { get; set; } = new List<SpendingReponse>();

	}
}
