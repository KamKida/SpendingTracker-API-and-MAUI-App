using SpendingsManagementWebAPI.Dtos.GroupLimitDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Dtos.GroupDtos
{
    public class GetGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public GroupLimitDto? GroupLimit { get; set; }
        public PlannedSpending? PlannedSpending { get; set; }
    }
}
