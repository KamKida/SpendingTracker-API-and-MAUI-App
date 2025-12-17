using SpendingsManagementWebAPI.Dtos.GroupLimitDtos;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI.Dtos.GroupDtos
{
    public class AddGroupDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public GroupLimitDto? AddEditGroupLimitDto { get; set; }
    }
}
