namespace SpendingsManagementWebAPI.Dtos.SpendingDtos
{
    public class EditSpendingDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int? GroupId { get; set; }
        public string Currency { get; set; }
        public string? Description { get; set; }
    }
}
