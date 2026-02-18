using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpendingTrackerApp.Domain.Models
{
	public class User
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Token { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public decimal ThisMonthFundSum { get; set; }
		public decimal ThisMonthSpendingsSum { get; set; }
	}
}
