using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Contracts.Dtos.Requests
{
	public class SpendingCategoryRequest
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? WeeklyLimit { get; set; }
		public decimal? MonthlyLimit { get; set; }
		public string? Description { get; set; }
	}
}
