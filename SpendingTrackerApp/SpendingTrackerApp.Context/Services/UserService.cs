using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Responses;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.BaseServices;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SpendingTrackerApp.Infrastructure.Services
{
	public class UserService : IUserService
	{
		private readonly BaseHttpService Http;
		public User User;


		public UserService(User user, BaseHttpService http)
		{
			User = user;
			Http = http;

		}

		public async Task<(int StatusCode, string Content)> LoginUser(UserRequest request)
		{
			try
			{
				var response = await Http._httpClient.PostAsJsonAsync("account/login", request);

				var content = await response.Content.ReadAsStringAsync();
				var statusCode = (int)response.StatusCode;

				return (statusCode, content);
			}
			catch (Exception ex)
			{
				return (0, ex.Message);
			}
		}

		public async Task<(int StatusCode, string Content)> CreateUser(UserRequest request)
		{
			try
			{
				var response = await Http._httpClient.PostAsJsonAsync("account/register", request);

				var content = await response.Content.ReadAsStringAsync();
				var statusCode = (int)response.StatusCode;

				return (statusCode, content);
			}
			catch (Exception ex)
			{
				return (0, ex.Message);
			}
		}

		public async Task<(int StatusCode, string Content)> ResetPassword(UserRequest request)
		{
			try
			{
				var response = await Http._httpClient.PutAsJsonAsync("account/resetPassword", request);

				var content = await response.Content.ReadAsStringAsync();
				var statusCode = (int)response.StatusCode;


				return (statusCode, content);
			}
			catch (Exception ex)
			{
				return (0, ex.Message);
			}
		}

		public async Task<(int StatusCode, string Content)> GetBaseInfo()
		{
			try
			{
				var response = await Http._httpClient.GetAsync("account/getBaseInfo");

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
