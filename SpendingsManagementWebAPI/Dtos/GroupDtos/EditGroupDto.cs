using SpendingsManagementWebAPI.Dtos.GroupLimitDtos;

namespace SpendingsManagementWebAPI.Dtos.GroupDtos
{
    public class EditGroupDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public GroupLimitDto? AddEditGroupLimitDto { get; set; }
    }
}
