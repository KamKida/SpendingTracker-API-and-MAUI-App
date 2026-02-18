using SpendingTrackerApp.Domain.Models;
using System.ComponentModel;

namespace SpendingTrackerApp.Contracts.Dtos.Requests
{
	public class FundCategoryRequest
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal? ShouldBe {  get; set; }
		public string? Description { get; set; }
		
	}
}
