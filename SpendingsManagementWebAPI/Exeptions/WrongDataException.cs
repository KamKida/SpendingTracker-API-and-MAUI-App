namespace SpendingsManagementWebAPI.Exeptions
{
    public class WrongDataException : Exception
    {
        public WrongDataException(string message) : base(message) 
        {
            
        }
    }
}
