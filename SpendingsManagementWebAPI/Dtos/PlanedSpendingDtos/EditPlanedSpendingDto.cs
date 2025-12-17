namespace SpendingsManagementWebAPI.Dtos.PlanedSpendingDtos
{
    public class EditPlanedSpendingDto
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string? Description { get; set; }
        public int? AtWhichPointOfMonth { get; set; }
        public int? WhichDayOfMonth { get; set; }
    }
}
