namespace SpendingsManagementWebAPI.Dtos.UserDtos
{
    public class EditUserDto
    {
        public string NewEmail { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
