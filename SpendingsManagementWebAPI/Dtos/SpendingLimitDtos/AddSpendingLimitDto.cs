namespace SpendingsManagementWebAPI.Dtos.SpendingLimitDtos
{
    public class AddSpendingLimitDto
    {
        public decimal Limit { get; set; }
        public string Currency { get; set; }
        public int NumberOfDays { get; set; }
    }
}

