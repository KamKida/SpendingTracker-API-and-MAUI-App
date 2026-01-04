using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class UserService : IUserService
	{
		private readonly BaseHttpService _http;
		public User _user;
		private readonly ILogger<UserService> _logger;


		public UserService(
		User user, 
		BaseHttpService http,
		ILogger<UserService> logger)
		{
			_user = user;
			_http = http;
			_logger = logger;

		}

		public async Task<HttpResponseMessage> LoginUser(UserRequest request)
		{
			_logger.LogInformation("Rozpoczynam logowanie użytkownika. Email: {Email}", request.Email);


			try
			{
				var response = await _http._httpClient.PostAsJsonAsync("account/login", request);

				_logger.LogInformation("Wynik logowania: {StatusCode}", response.StatusCode);

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
			_logger.LogInformation(
				"Rozpoczynam rejestrację użytkownika. Email: {Email}",
				request.Email
			);

			try
			{
				var response = await _http._httpClient.PostAsJsonAsync(
					"account/register",
					request
				);

				_logger.LogInformation(
					"Wynik rejestracji użytkownika {Email}: {StatusCode}",
					request.Email,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas rejestracji użytkownika {Email}",
					request.Email
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas rejestracji użytkownika {Email}",
					request.Email
				);
				throw;
			}
		}


		public async Task<HttpResponseMessage> ResetPassword(UserRequest request)
		{
			_logger.LogInformation(
				"Rozpoczynam reset hasła użytkownika. Email: {Email}",
				request.Email
			);

			try
			{
				var response = await _http._httpClient.PutAsJsonAsync(
					"account/resetPassword",
					request
				);

				_logger.LogInformation(
					"Wynik resetu hasła użytkownika {Email}: {StatusCode}",
					request.Email,
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas resetu hasła użytkownika {Email}",
					request.Email
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas resetu hasła użytkownika {Email}",
					request.Email
				);
				throw;
			}
		}


		public async Task<HttpResponseMessage> GetBaseInfo()
		{
			_logger.LogInformation("Rozpoczynam pobieranie podstawowych informacji użytkownika.");

			try
			{
				var response = await _http._httpClient.GetAsync(
					"account/getBaseInfo"
				);

				_logger.LogInformation(
					"Wynik pobierania podstawowych informacji użytkownika: {StatusCode}",
					response.StatusCode
				);

				return response;
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas pobierania podstawowych informacji użytkownika"
				);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas pobierania podstawowych informacji użytkownika"
				);
				throw;
			}
		}

	}
}
