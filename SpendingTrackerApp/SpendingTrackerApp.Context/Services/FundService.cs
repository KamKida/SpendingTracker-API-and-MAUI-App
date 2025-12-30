using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class FundService : IFundService
	{
		private readonly BaseHttpService Http;

		public FundService(User user,
				BaseHttpService http)
		{
			Http = http;

		}

		public async Task<(int StatusCode, string Content)> GetTop10()
		{
			try
			{
				var response = await Http._httpClient.GetAsync("fund/top10");

				var content = await response.Content.ReadAsStringAsync();
				var statusCode = (int)response.StatusCode;

				return (statusCode, content);
			}
			catch (Exception ex)
			{
				return (0, ex.Message);
			}
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

		public async Task<(int StatusCode, string Content)> DeleteFund(Guid id)
		{
			try
			{
				var response = await Http._httpClient.DeleteAsync($"fund/delete/{id}");

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
