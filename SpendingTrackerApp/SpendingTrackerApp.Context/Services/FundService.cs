using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class FundService : IFundService
	{
		private readonly BaseHttpService Http;
		public User User;


		public FundService(User user, BaseHttpService http)
		{
			User = user;
			Http = http;

		}

		public async Task<(int StatusCode, string Content)> AddFund(FundRequest request)
		{
			try
			{
				var response = await Http._httpClient.PostAsJsonAsync("fund/add", request);

				var content = await response.Content.ReadAsStringAsync();
				var statusCode = (int)response.StatusCode;

				return (statusCode, content);
			}
			catch (Exception ex)
			{
				return (0, ex.Message);
			}
		}
	}
}
