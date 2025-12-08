using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class StringToULongConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is ulong num))
		{
			throw new ArgumentException("Convert parameter must be an unsigned long");
		}
		return num.ToString();
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is string) || !ulong.TryParse((string)value, out var result))
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
