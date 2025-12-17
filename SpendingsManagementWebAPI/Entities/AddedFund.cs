using System;

namespace SpendingsManagementWebAPI.Entities
{
    public class AddedFund
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Currency { get; set; }
        public decimal AmountAdded { get; set; }
        public string? Source {  get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
