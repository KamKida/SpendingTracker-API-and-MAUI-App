using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
	}
}
