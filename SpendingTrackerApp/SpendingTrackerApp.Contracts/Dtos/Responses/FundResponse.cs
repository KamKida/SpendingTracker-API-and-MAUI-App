using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class FundResponse
	{
		public Guid Id { get; set; }
		//public FundCategory? FundCategory { get; set; }
		//public Guid? FundCategoryId { get; set; }
		public decimal Amount { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
