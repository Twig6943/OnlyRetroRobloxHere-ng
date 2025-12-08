using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

[ValueConversion(typeof(object), typeof(string))]
internal class MultiStringFormatConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		return string.Format((parameter as string) ?? throw new ArgumentException("Convert parameter must be a string"), values);
	}

	public object[] ConvertBack(object? value, Type[] targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
