namespace SpendingTracker.Contracts.Dtos.Responses
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal ThisMonthFundSum { get; set; }
		public decimal ThisMonthSpendingsSum { get; set; }
        public List<SpendingReponse> SpendingReponses { get; set; } = new List<SpendingReponse>();
        public List<SpendingCategoryResponse> SpendingCategoryResponses { get; set; } = new List<SpendingCategoryResponse>();

	}
}
