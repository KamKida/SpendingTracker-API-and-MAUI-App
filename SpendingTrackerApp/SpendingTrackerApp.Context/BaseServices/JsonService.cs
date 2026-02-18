using System.Text.Json;

namespace SpendingTrackerApp.Infrastructure.BaseServices
{
	public class JsonService
	{
		public T Deserialize<T>(string json)
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			return JsonSerializer.Deserialize<T>(json, options);
		}

		public string Serialize(object obj)
		{
			return JsonSerializer.Serialize(obj);
		}
	}
}
