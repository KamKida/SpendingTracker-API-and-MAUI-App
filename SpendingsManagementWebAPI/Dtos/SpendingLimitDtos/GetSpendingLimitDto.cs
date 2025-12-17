namespace SpendingsManagementWebAPI.Dtos.SpendingLimitDtos
{
    public class GetSpendingLimitDto
    {
        public string Id { get; set; }
        public string Limit { get; set; }
        public string Currency { get; set; }
        public int NumberOfDays { get; set; }
    }
}
