using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Contracts.Dtos.Requests.FiltersRequests
{
	public class SpendingCategoryFilterRequest
	{
		public string? Name { get; set; }
		public decimal? WeeklyLimitFrom { get; set; }
		public decimal? WeeklyLimitTo { get; set; }
		public decimal? MonthlyLimitFrom { get; set; }
		public decimal? MonthlyLimitTo { get; set; }
		public DateTimeOffset? DateFrom { get; set; }
		public DateTimeOffset? DateTo { get; set; }
		public DateTimeOffset? LastDate { get; set; }
	}
}
