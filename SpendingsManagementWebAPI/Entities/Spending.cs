namespace SpendingsManagementWebAPI.Entities
{
    public class Spending
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }

        //Miejsce na pole ze zdjęciem
    }
}
