using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class TernaryStringConverter : IMultiValueConverter
{
	public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length != 3)
		{
			throw new ArgumentException("Convert must be given 2 values");
		}
		if (!(values[0] is bool flag))
		{
			throw new ArgumentException("Convert parameter #1 must be a boolean");
		}
		if (!(values[1] is string result))
		{
			throw new ArgumentException("Convert parameter #2 must be a string");
		}
		if (!(values[2] is string result2))
		{
			throw new ArgumentException("Convert parameter #3 must be a string");
		}
		if (!flag)
		{
			return result2;
		}
		return result;
	}

	public object[] ConvertBack(object? value, Type[] targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
