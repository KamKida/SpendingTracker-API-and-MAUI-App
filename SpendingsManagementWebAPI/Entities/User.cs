using System;
using System.Collections.Generic;

namespace SpendingsManagementWebAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? UserName { get; set; }
        public DateTime DateOfCreation { get; set; }
        public virtual List<AddedFund> Funds { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual List<Spending> Spendings { get; set; }
        public virtual List<PlannedSpending> PlanedSpendings { get; set; }
        public virtual List<SpendingLimit> SpendingLimits { get; set; }
    }
}
