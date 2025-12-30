using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Contracts.Dtos.Requests
{
	public class FundFilterRequest
	{
	public decimal? AmountFrom { get; set; }
	public decimal? AmountTo { get; set; }
	public DateTime? DateFrom { get; set; }
	public DateTime? DateTo { get; set; }

	public DateTime? LastDate { get; set; }

	}
}
