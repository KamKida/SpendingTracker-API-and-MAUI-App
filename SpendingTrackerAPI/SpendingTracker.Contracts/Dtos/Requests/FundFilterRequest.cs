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

		public DateTimeOffset? DateFrom { get; set; }
		public DateTimeOffset? DateTo { get; set; }
		public DateTimeOffset? LastDate { get; set; }

	}
}
