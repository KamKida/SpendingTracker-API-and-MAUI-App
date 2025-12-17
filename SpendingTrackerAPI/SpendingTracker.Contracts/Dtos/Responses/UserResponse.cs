using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Contracts.Dtos.Responses
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
