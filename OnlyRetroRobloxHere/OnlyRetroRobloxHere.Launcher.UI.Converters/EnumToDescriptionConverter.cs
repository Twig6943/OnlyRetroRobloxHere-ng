using System;
using System.Globalization;
using System.Windows.Data;
using OnlyRetroRobloxHere.Launcher.Extensions;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class EnumToDescriptionConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ((Enum)value).GetDescription();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
