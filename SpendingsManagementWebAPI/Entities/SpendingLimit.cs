namespace SpendingsManagementWebAPI.Entities
{
    public class SpendingLimit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public decimal Limit { get; set; }
        public string Currency {  get; set; }
        public int NumberOfDays { get; set; }

    }
}
