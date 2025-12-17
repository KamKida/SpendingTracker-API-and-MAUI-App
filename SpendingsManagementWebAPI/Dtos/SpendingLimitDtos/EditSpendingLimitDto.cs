namespace SpendingsManagementWebAPI.Dtos.SpendingLimitDtos
{
    public class EditSpendingLimitDto
    {
        public int Id { get; set; }
        public decimal Limit { get; set; }
        public string Currency { get; set; }
        public int NumberOfDays { get; set; }
    }
}
