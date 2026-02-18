using System.Globalization;

namespace SpendingTrackerApp.Converters
{
	public class DecimalNullableConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return string.Empty;
			return ((decimal)value).ToString("0.##", CultureInfo.InvariantCulture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value as string;

			if (string.IsNullOrWhiteSpace(str))
				return null;

			if (decimal.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
				return result;

			return null;
		}
	}
}
