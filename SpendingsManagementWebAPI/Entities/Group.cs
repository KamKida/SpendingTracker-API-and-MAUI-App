using System.Collections.Generic;

namespace SpendingsManagementWebAPI.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public GroupLimit GroupLimit { get; set; }
        public virtual List<Spending> Spendings { get; set; }
        public virtual PlannedSpending PlannedSpending { get; set; }
    }
}
