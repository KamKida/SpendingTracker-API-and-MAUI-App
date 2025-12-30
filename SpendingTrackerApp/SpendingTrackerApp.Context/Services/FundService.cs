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

		public async Task<HttpResponseMessage> Get10(FundFilterRequest request)
		{
			try
			{
				var url =
				$"fund/get10" +
				$"?dateFrom={request.DateFrom:O}" +
				$"&dateTo={request.DateTo:O}" +
				$"&amountFrom={request.AmountFrom}" +
				$"&amountTo={request.AmountTo}" +
				$"&lastDate={request.LastDate:O}";


				var response = await Http._httpClient.GetAsync(url);

				return (response);
			}
			catch (Exception ex)
			{
				return (null);
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

		public async Task<HttpResponseMessage> EditFund(FundRequest request)
		{
			try
			{
				var response = await Http._httpClient.PutAsJsonAsync($"fund/edit", request);


				return (response);
			}
			catch (Exception ex)
			{
				return (null);
			}
		}
	}
}
