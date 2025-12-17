namespace SpendingsManagementWebAPI.Dtos.FundsDtos
{
    public class EditFundDto
    {
        public int Id { get; set; }
        public decimal AmountAdded { get; set; }
        public string Currency { get; set; }
        public string? Source { get; set; }
    }
}
