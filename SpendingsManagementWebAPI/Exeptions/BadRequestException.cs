using System;

namespace SpendingsManagementWebAPI.Exeptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
            
        }
    }
}
