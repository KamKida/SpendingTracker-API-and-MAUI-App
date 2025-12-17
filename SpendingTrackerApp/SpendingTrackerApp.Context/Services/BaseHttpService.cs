using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SpendingTrackerApp.Infrastructure.Services
{
    public class BaseHttpService
    {
        public HttpClient _httpClient;

        public BaseHttpService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
               HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.0.2.2:5250/spending/")
            };
        }

        public void SetToken(User user)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", user.Token);
        }
    }
}
