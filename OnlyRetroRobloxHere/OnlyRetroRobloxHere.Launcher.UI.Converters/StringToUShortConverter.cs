using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class StringToUShortConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is ushort num))
		{
			throw new ArgumentException("Convert parameter must be a ushort");
		}
		return num.ToString();
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is string) || !ushort.TryParse((string)value, out var result))
		{
			if (parameter is string)
			{
				return ushort.Parse((string)parameter);
			}
			return 0;
		}
		return result;
	}
}
