using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class FundCategoryService : IFundCategoryService
	{
		private readonly BaseHttpService _httpService;
		private readonly ILogger<FundService> _logger;

		public FundCategoryService(
				BaseHttpService httpService,
				ILogger<FundService> logger)
		{
			_httpService = httpService;
			_logger = logger;

		}

		public async Task<HttpResponseMessage> Get10(FundCategoryFilterRequest request, bool useDatesFromToo = false)
		{
			_logger.LogInformation(
				"Rozpoczynam pobieranie 10 kategorii funduszy z filtrem: Name={Name}, DateFrom={DateFrom}, DateTo={DateTo}, ShouldBeFrom={ShouldBeFrom}, ShouldBeTo={ShouldBeTo}, LastDate={LastDate}.",
				request.Name,
				request.DateFrom,
				request.DateTo,
				request.ShouldBeFrom,
				request.ShouldBeTo,
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

				if (request.ShouldBeFrom.HasValue)
					query.Add($"shouldBeFrom={request.ShouldBeFrom.Value}");

				if (request.ShouldBeTo.HasValue)
					query.Add($"shouldBeTo={request.ShouldBeTo.Value}");

				if (request.LastDate.HasValue)
					query.Add($"lastDate={Uri.EscapeDataString(request.LastDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff"))}");

				var url = "fundCategory/get10";

				if (query.Any())
					url += "?" + string.Join("&", query);

				var response = await _httpService._httpClient.GetAsync(url);

				_logger.LogInformation(
					"Zakończono pobieranie 10 kategorii funduszy. StatusCode={StatusCode}.",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania 10 kategorii funduszy. Name={Name}.",
					request.Name
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania 10 kategorii funduszy. Name={Name}.",
					request.Name
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> AddFundCategory(FundCategoryRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam dodawanie nowej kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}.",
				request.Name,
				request.ShouldBe
			);

			try
			{
				var response = await _httpService._httpClient.PostAsJsonAsync("fundCategory/add", request);

				_logger.LogInformation(
					"Zakończono dodawanie kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}, StatusCode={StatusCode}.",
					request.Name,
					request.ShouldBe,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas dodawania kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}.",
					request.Name,
					request.ShouldBe
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas dodawania kategorii funduszy. Name={Name}, ShouldBe={ShouldBe}.",
					request.Name,
					request.ShouldBe
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> DeleteFundCategory(Guid id)
		{
			_logger.LogInformation(
				"Rozpoczynam usuwanie kategorii funduszu. FundCategoryId={FundCategoryId}.",
				id
			);

			try
			{
				var response = await _httpService._httpClient.DeleteAsync($"fundCategory/delete/{id}");

				_logger.LogInformation(
					"Zakończono usuwanie kategorii funduszu. FundCategoryId={FundCategoryId}, StatusCode={StatusCode}.",
					id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas usuwania kategorii funduszu. FundCategoryId={FundCategoryId}.",
					id
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas usuwania kategorii funduszu. FundCategoryId={FundCategoryId}.",
					id
				);
				throw;
			}
		}

		public async Task<HttpResponseMessage> EditFundCategory(FundCategoryRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam edycję kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}.",
				request.Id,
				request.ShouldBe
			);

			try
			{
				var response = await _httpService._httpClient.PutAsJsonAsync("fundCategory/edit", request);

				_logger.LogInformation(
					"Zakończono edycję kategorii funduszu. FundCategoryId={FundCategoryId}, StatusCode={StatusCode}.",
					request.Id,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}.",
					request.Id,
					request.ShouldBe
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}.",
					request.Id,
					request.ShouldBe
				);
				throw;
			}
		}
	}
}