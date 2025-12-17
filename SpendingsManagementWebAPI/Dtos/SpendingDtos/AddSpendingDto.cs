namespace SpendingsManagementWebAPI.Dtos.SpendingDtos
{
    public class AddSpendingDto
    {
        public int? GroupId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string Currency { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
