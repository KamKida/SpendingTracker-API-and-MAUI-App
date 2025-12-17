using System;

namespace SpendingsManagementWebAPI.Exeptions
{
    public class RegisterExeption : Exception
    {
        public RegisterExeption(string message) : base(message)
        {
            
        }
    }
}
