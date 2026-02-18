using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class UserService : IUserService
	{
		private readonly BaseHttpService _http;
		private readonly ILogger<UserService> _logger;

		public UserService(
			BaseHttpService http,
			ILogger<UserService> logger)
		{
			_http = http;
			_logger = logger;
		}

		public async Task<HttpResponseMessage> LoginUser(UserRequest request)
		{
			_logger.LogInformation("Rozpoczynam logowanie użytkownika. Email: {Email}", request.Email);

			try
			{
				var response = await _http._httpClient.PostAsJsonAsync("account/login", request);

				_logger.LogInformation("Wynik logowania użytkownika {Email}: {StatusCode}", request.Email, response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas logowania użytkownika {Email}", request.Email);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas logowania użytkownika {Email}", request.Email);
				throw;
			}
		}

		public async Task<HttpResponseMessage> CreateUser(UserRequest request)
		{
			_logger.LogInformation("Rozpoczynam rejestrację użytkownika. Email: {Email}", request.Email);

			try
			{
				var response = await _http._httpClient.PostAsJsonAsync("account/register", request);

				_logger.LogInformation("Wynik rejestracji użytkownika {Email}: {StatusCode}", request.Email, response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas rejestracji użytkownika {Email}", request.Email);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas rejestracji użytkownika {Email}", request.Email);
				throw;
			}
		}

		public async Task<HttpResponseMessage> ResetPassword(UserRequest request)
		{
			_logger.LogInformation("Rozpoczynam reset hasła użytkownika. Email: {Email}", request.Email);

			try
			{
				var response = await _http._httpClient.PutAsJsonAsync("account/resetPassword", request);

				_logger.LogInformation("Wynik resetu hasła użytkownika {Email}: {StatusCode}", request.Email, response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas resetu hasła użytkownika {Email}", request.Email);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas resetu hasła użytkownika {Email}", request.Email);
				throw;
			}
		}

		public async Task<HttpResponseMessage> GetThisMonthInfo()
		{
			_logger.LogInformation("Rozpoczynam pobieranie informacji użytkownika za bieżący miesiąc.");

			try
			{
				var response = await _http._httpClient.GetAsync("account/getThisMonthInfo");

				_logger.LogInformation("Wynik pobierania informacji użytkownika za bieżący miesiąc: {StatusCode}", response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania informacji użytkownika za bieżący miesiąc");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania informacji użytkownika za bieżący miesiąc");
				throw;
			}
		}

		public async Task<HttpResponseMessage> GetUserBaseData()
		{
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych danych użytkownika.");

			try
			{
				var response = await _http._httpClient.GetAsync("account/getUserBaseData");

				_logger.LogInformation("Wynik pobierania podstawowych danych użytkownika: {StatusCode}", response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas pobierania podstawowych danych użytkownika");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania podstawowych danych użytkownika");
				throw;
			}
		}

		public async Task<HttpResponseMessage> EditUser(UserRequest request)
		{
			_logger.LogInformation("Rozpoczynam edycję użytkownika. Email: {Email}", request.Email);

			try
			{
				var response = await _http._httpClient.PutAsJsonAsync("account/edit", request);

				_logger.LogInformation("Wynik edycji użytkownika {Email}: {StatusCode}", request.Email, response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas edycji użytkownika {Email}", request.Email);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas edycji użytkownika {Email}", request.Email);
				throw;
			}
		}

		public async Task<HttpResponseMessage> DeleteUser()
		{
			_logger.LogInformation("Rozpoczynam usuwanie użytkownika.");

			try
			{
				var response = await _http._httpClient.DeleteAsync("account/delete");

				_logger.LogInformation("Wynik usunięcia użytkownika: {StatusCode}", response.StatusCode);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "Błąd HTTP podczas usuwania użytkownika");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Nieoczekiwany błąd podczas usuwania użytkownika");
				throw;
			}
		}
	}
}
