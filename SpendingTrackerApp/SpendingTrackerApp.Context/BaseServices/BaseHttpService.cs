using System.Net.Http.Headers;

namespace SpendingTrackerApp.Infrastructure.BaseServices
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

        public void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
