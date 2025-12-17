namespace SpendingsManagementWebAPI.Dtos.GroupLimitDtos
{
    public class GroupLimitDto
    {
        public decimal LimitPerSpending { get; set; }
        public decimal TotalSpendingLimit { get; set; }
        public string Currency { get; set; }
    }
}
