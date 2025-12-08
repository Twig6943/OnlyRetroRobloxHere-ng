using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

[ValueConversion(typeof(int), typeof(SolidColorBrush))]
internal class BrickIdToBrushConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is int id))
		{
			throw new ArgumentException("Convert value must be an integer");
		}
		return BrickColors.GetColourFromId(id);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
