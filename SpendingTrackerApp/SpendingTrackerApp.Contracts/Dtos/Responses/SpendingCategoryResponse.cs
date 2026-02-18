using SpendingTrackerApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTrackerApp.Contracts.Dtos.Responses
{
	public class SpendingCategoryResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? WeeklyLimit { get; set; }
		public decimal? MonthlyLimit { get; set; }
		public decimal? MonthlyLimitDiffrence { get; set; }
		public string? Description { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
