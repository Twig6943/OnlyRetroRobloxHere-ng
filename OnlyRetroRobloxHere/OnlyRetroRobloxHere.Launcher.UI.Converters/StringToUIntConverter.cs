using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class StringToUIntConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is uint num))
		{
			throw new ArgumentException("Convert parameter must be an unsigned integer");
		}
		return num.ToString();
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is string) || !uint.TryParse((string)value, out var result))
		{
			if (parameter is string)
			{
				return uint.Parse((string)parameter);
			}
			return 0;
		}
		return result;
	}
}
