namespace SpendingsManagementWebAPI.Dtos.FundsDtos
{
    public class AddFundDto
    {
        public decimal AmountAdded {  get; set; }
        public string Currency {  get; set; }
        public string? Source { get; set; }
    }
}
