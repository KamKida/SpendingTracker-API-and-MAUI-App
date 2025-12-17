namespace SpendingsManagementWebAPI.Entities
{
    public class GroupLimit
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public decimal LimitPerSpending { get; set; }
        public decimal TotalSpendingLimit { get; set; }
        public string Currency { get; set; }
    }
}
