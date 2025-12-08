using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

[ValueConversion(typeof(object), typeof(Visibility))]
internal class NullVisibilityConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		bool flag = value == null;
		if (!(parameter?.ToString() == "opposite"))
		{
			return flag ? Visibility.Collapsed : Visibility.Visible;
		}
		return (!flag) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
