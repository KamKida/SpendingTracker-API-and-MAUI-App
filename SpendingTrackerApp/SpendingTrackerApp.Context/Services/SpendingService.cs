using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class SpendingService : ISpendingService
	{
		private readonly BaseHttpService _httpService;
		private readonly ILogger<SpendingService> _logger;

		public SpendingService(
			BaseHttpService httpService,
			ILogger<SpendingService> logger)
		{
			_httpService = httpService;
			_logger = logger;
		}

		public async Task<HttpResponseMessage> Get10(SpendingFilterRequest request, bool useDatesFromToo = false)
		{
			_logger.LogInformation(
				"Rozpoczynam pobieranie 10 wydatków z filtrem: DateFrom={DateFrom}, DateTo={DateTo}, AmountFrom={AmountFrom}, AmountTo={AmountTo}, LastDate={LastDate}",
				request.DateFrom,
				request.DateTo,
				request.AmountFrom,
				request.AmountTo,
				request.LastDate
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

				if (request.SpendingCategoryId.HasValue)
					query.Add($"spendingCategoryId={request.SpendingCategoryId}");

				var url = "spending/get10";

				if (query.Any())
					url += "?" + string.Join("&", query);

				var response = await _httpService._httpClient.GetAsync(url);

				_logger.LogInformation(
					"Pobieranie 10 wydatków zakończone. StatusCode={StatusCode}",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania 10 wydatków."
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania 10 wydatków."
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> AddSpending(SpendingRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam dodawanie wydatku. Kwota={Amount}",
				request.Amount
			);

			try
			{
				var response = await _httpService._httpClient.PostAsJsonAsync("spending/add", request);

				_logger.LogInformation(
					"Dodawanie wydatku zakończone. StatusCode={StatusCode}",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania wydatku. Kwota={Amount}",
					request.Amount
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas dodawania wydatku. Kwota={Amount}",
					request.Amount
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> DeleteSpending(Guid id)
		{
			_logger.LogInformation(
				"Rozpoczynam usuwanie wydatku. SpendingId={SpendingId}",
				id
			);

			try
			{
				var response = await _httpService._httpClient.DeleteAsync($"spending/delete/{id}");

				_logger.LogInformation(
					"Usuwanie wydatku zakończone. SpendingId={SpendingId}, StatusCode={StatusCode}",
					id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania wydatku. SpendingId={SpendingId}",
					id
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania wydatku. SpendingId={SpendingId}",
					id
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> EditSpending(SpendingRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam edycję wydatku. SpendingId={SpendingId}, Kwota={Amount}",
				request.Id,
				request.Amount
			);

			try
			{
				var response = await _httpService._httpClient.PutAsJsonAsync("spending/edit", request);

				_logger.LogInformation(
					"Edycja wydatku zakończona. SpendingId={SpendingId}, StatusCode={StatusCode}",
					request.Id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji wydatku. SpendingId={SpendingId}",
					request.Id
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji wydatku. SpendingId={SpendingId}",
					request.Id
				);
				throw;
			}
		}
	}
}
