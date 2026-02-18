using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SpendingTrackerApp.Infrastructure.Extensions;

namespace SpendingTrackerApp
{
    public static class MauiProgram
    {
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();


			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});
			builder.Services.AddServices();


			builder.Logging.ClearProviders();
			builder.Logging.SetMinimumLevel(LogLevel.Trace);

			builder.Logging.AddNLog();
#if DEBUG
			builder.Logging.AddDebug();
#endif
			return builder.Build();
		}
	}
}
