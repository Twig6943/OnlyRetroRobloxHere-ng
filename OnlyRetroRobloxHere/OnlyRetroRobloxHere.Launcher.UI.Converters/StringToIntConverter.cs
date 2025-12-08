using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class StringToIntConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is int num))
		{
			throw new ArgumentException("Convert parameter must be an integer");
		}
		return num.ToString();
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is string) || !int.TryParse((string)value, out var result))
		{
			if (parameter is string)
			{
				return int.Parse((string)parameter);
			}
			return 0;
		}
		return result;
	}
}
