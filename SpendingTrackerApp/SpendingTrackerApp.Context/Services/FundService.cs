using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class FundService : IFundService
	{
		private readonly BaseHttpService _httpService;
		private readonly ILogger<FundService> _logger;

		public FundService(
				BaseHttpService httpService,
				ILogger<FundService> logger)
		{
			_httpService = httpService;
			_logger = logger;

		}

		public async Task<HttpResponseMessage> Get10(FundFilterRequest request, bool useDatesFromToo = false)
		{
			_logger.LogInformation(
				"Rozpoczynam pobieranie 10 funduszy z filtrami: DateFrom={DateFrom}, DateTo={DateTo}, AmountFrom={AmountFrom}, AmountTo={AmountTo}, LastDate={LastDate}, FundCategoryId={FundCategoryId}",
				request.DateFrom,
				request.DateTo,
				request.AmountFrom,
				request.AmountTo,
				request.LastDate,
				request.FundCategoryId
			);

			try
			{
				var query = new List<string>();

				if (useDatesFromToo)
				{
					if (request.DateFrom.HasValue)
						query.Add($"dateFrom={Uri.EscapeDataString(request.DateFrom.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff"))}");

					if (request.DateTo.HasValue)
						query.Add($"dateTo={Uri.EscapeDataString(request.DateTo.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff"))}");
				}

				if (request.AmountFrom.HasValue)
					query.Add($"amountFrom={request.AmountFrom.Value}");

				if (request.AmountTo.HasValue)
					query.Add($"amountTo={request.AmountTo.Value}");

				if (request.LastDate.HasValue)
					query.Add($"lastDate={Uri.EscapeDataString(request.LastDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff"))}");

				if (request.FundCategoryId.HasValue)
					query.Add($"fundCategoryId={request.FundCategoryId}");

				var url = "fund/get10";

				if (query.Any())
					url += "?" + string.Join("&", query);

				var response = await _httpService._httpClient.GetAsync(url);

				_logger.LogInformation(
					"Pobieranie 10 funduszy zakończone. StatusCode={StatusCode}",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania listy 10 funduszy."
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania listy 10 funduszy."
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> AddFund(FundRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam dodawanie nowego funduszu. Amount={Amount}",
				request.Amount
			);

			try
			{
				var response = await _httpService._httpClient.PostAsJsonAsync("fund/add", request);

				_logger.LogInformation(
					"Dodawanie funduszu zakończone. StatusCode={StatusCode}",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania funduszu. Amount={Amount}",
					request.Amount
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas dodawania funduszu. Amount={Amount}",
					request.Amount
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> DeleteFund(Guid id)
		{
			_logger.LogInformation(
				"Rozpoczynam usuwanie funduszu. FundId={FundId}",
				id
			);

			try
			{
				var response = await _httpService._httpClient.DeleteAsync($"fund/delete/{id}");

				_logger.LogInformation(
					"Usuwanie funduszu zakończone. FundId={FundId}, StatusCode={StatusCode}",
					id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania funduszu. FundId={FundId}",
					id
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania funduszu. FundId={FundId}",
					id
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> EditFund(FundRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam edycję funduszu. FundId={FundId}, Amount={Amount}",
				request.Id,
				request.Amount
			);

			try
			{
				var response = await _httpService._httpClient.PutAsJsonAsync("fund/edit", request);

				_logger.LogInformation(
					"Edycja funduszu zakończona. FundId={FundId}, StatusCode={StatusCode}",
					request.Id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji funduszu. FundId={FundId}",
					request.Id
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji funduszu. FundId={FundId}",
					request.Id
				);
				throw;
			}
		}
	}
}

