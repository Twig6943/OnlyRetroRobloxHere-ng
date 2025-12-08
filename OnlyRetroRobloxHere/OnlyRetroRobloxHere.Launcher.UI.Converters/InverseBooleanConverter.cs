using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

[ValueConversion(typeof(bool), typeof(bool))]
internal class InverseBooleanConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is bool flag))
		{
			throw new ArgumentException("Convert value must be a boolean");
		}
		return !flag;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
