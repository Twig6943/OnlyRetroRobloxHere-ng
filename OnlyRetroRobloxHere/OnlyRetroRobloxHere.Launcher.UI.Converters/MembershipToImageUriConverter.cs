using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

[ValueConversion(typeof(int), typeof(SolidColorBrush))]
internal class MembershipToImageUriConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (!(value is MembershipType value2))
		{
			throw new ArgumentException("Convert value must be Membership enum");
		}
		return new Uri($"pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Membership/{value2}.png");
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack");
	}
}
