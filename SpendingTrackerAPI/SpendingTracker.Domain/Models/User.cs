using SpendingTracker.Contracts.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreationDate { get; set; }

        public void EditUser(UserEditRequest request)
        {
            Email = request.Email;
            FirstName = request.FirstName;
            LastName = request.LastName;
        }
    }
}
