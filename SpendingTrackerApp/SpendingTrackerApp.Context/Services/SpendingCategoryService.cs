using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class SpendingCategoryService : ISpendingCategoryService
	{
		private readonly BaseHttpService _httpService;
		private readonly ILogger<SpendingCategoryService> _logger;

		public SpendingCategoryService(
				BaseHttpService httpService,
				ILogger<SpendingCategoryService> logger)
		{
			_httpService = httpService;
			_logger = logger;
		}

		public async Task<HttpResponseMessage> Get10(SpendingCategoryFilterRequest request, bool useDatesFromToo = false)
		{
			_logger.LogInformation(
				"Rozpoczynam pobieranie 10 kategorii wydatków z filtrem: Name={Name}, DateFrom={DateFrom}, DateTo={DateTo}, WeeklyLimitFrom={WeeklyLimitFrom}, WeeklyLimitTo={WeeklyLimitTo}, MonthlyLimitFrom={MonthlyLimitFrom}, MonthlyLimitTo={MonthlyLimitTo}, LastDate={LastDate}",
				request.Name,
				request.DateFrom,
				request.DateTo,
				request.WeeklyLimitFrom,
				request.WeeklyLimitTo,
				request.MonthlyLimitFrom,
				request.MonthlyLimitTo,
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

				if (!string.IsNullOrEmpty(request.Name))
				{
					query.Add($"name={request.Name}");
				}

				if (request.MonthlyLimitFrom.HasValue)
					query.Add($"monthlyLimitFrom={request.MonthlyLimitFrom.Value}");

				if (request.MonthlyLimitTo.HasValue)
					query.Add($"monthlyLimitTo={request.MonthlyLimitTo.Value}");

				if (request.WeeklyLimitFrom.HasValue)
					query.Add($"weeklyLimitFrom={request.WeeklyLimitFrom.Value}");

				if (request.WeeklyLimitTo.HasValue)
					query.Add($"weeklyLimitTo={request.WeeklyLimitTo.Value}");

				if (request.LastDate.HasValue)
					query.Add($"lastDate={Uri.EscapeDataString(request.LastDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff"))}");

				var url = "spendingCategory/get10";

				if (query.Any())
					url += "?" + string.Join("&", query);

				var response = await _httpService._httpClient.GetAsync(url);

				_logger.LogInformation(
					"Pobrano 10 kategorii wydatków. Status odpowiedzi: {StatusCode}",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Wystąpił błąd HTTP podczas pobierania 10 kategorii wydatków. Name={Name}",
					request.Name
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania 10 kategorii wydatków. Name={Name}",
					request.Name
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> AddSpendingCategory(SpendingCategoryRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam dodawanie kategorii wydatków: Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
				request.Name,
				request.WeeklyLimit,
				request.MonthlyLimit
			);

			try
			{
				var response = await _httpService._httpClient.PostAsJsonAsync("spendingCategory/add", request);

				_logger.LogInformation(
					"Kategoria wydatków dodana. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}, Status odpowiedzi={StatusCode}",
					request.Name,
					request.WeeklyLimit,
					request.MonthlyLimit,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					request.Name,
					request.WeeklyLimit,
					request.MonthlyLimit
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas dodawania kategorii wydatków. Name={Name}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					request.Name,
					request.WeeklyLimit,
					request.MonthlyLimit
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> DeleteSpendingCategory(Guid id)
		{
			_logger.LogInformation(
				"Rozpoczynam usuwanie kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
				id
			);

			try
			{
				var response = await _httpService._httpClient.DeleteAsync($"spendingCategory/delete/{id}");

				_logger.LogInformation(
					"Kategoria wydatków usunięta. SpendingCategoryId={SpendingCategoryId}, Status odpowiedzi={StatusCode}",
					id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
					id
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania kategorii wydatków. SpendingCategoryId={SpendingCategoryId}",
					id
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> EditSpendingCategory(SpendingCategoryRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam edycję kategorii wydatków. SpendingCategoryId={SpendingCategoryId}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
				request.Id,
				request.WeeklyLimit,
				request.MonthlyLimit
			);

			try
			{
				var response = await _httpService._httpClient.PutAsJsonAsync("spendingCategory/edit", request);

				_logger.LogInformation(
					"Kategoria wydatków zaktualizowana. SpendingCategoryId={SpendingCategoryId}, Status odpowiedzi={StatusCode}",
					request.Id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					request.Id,
					request.WeeklyLimit,
					request.MonthlyLimit
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji kategorii wydatków. SpendingCategoryId={SpendingCategoryId}, WeeklyLimit={WeeklyLimit}, MonthlyLimit={MonthlyLimit}",
					request.Id,
					request.WeeklyLimit,
					request.MonthlyLimit
				);
				throw;
			}
		}
	}
}
