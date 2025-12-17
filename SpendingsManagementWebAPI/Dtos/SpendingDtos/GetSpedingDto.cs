using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Dtos.SpendingDtos
{
    public class GetSpedingDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string GroupName { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
