using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class StringToPosLongConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is long num))
		{
			throw new ArgumentException("Convert parameter must be a long");
		}
		return num.ToString();
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is string) || !long.TryParse((string)value, out var result) || result <= 0)
		{
			if (parameter is string)
			{
				return long.Parse((string)parameter);
			}
			return 0;
		}
		return result;
	}
}
