using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OnlyRetroRobloxHere.Launcher.UI.Converters;

internal class BorderGapMaskConverter : IMultiValueConverter
{
	public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter == null || values == null || values.Length != 3 || !(values[0] is double) || !(values[1] is double) || !(values[2] is double))
		{
			return DependencyProperty.UnsetValue;
		}
		if (!(parameter is double) && !(parameter is string))
		{
			return DependencyProperty.UnsetValue;
		}
		double pixels = (double)values[0];
		double num = (double)values[1];
		double num2 = (double)values[2];
		if (num == 0.0 || num2 == 0.0)
		{
			return null;
		}
		double pixels2 = ((!(parameter is string)) ? ((double)parameter) : double.Parse((string)parameter, NumberFormatInfo.InvariantInfo));
		Grid obj = new Grid
		{
			Width = num,
			Height = num2
		};
		ColumnDefinition columnDefinition = new ColumnDefinition();
		ColumnDefinition columnDefinition2 = new ColumnDefinition();
		ColumnDefinition columnDefinition3 = new ColumnDefinition();
		columnDefinition.Width = new GridLength(pixels2);
		columnDefinition2.Width = new GridLength(pixels);
		columnDefinition3.Width = new GridLength(1.0, GridUnitType.Star);
		obj.ColumnDefinitions.Add(columnDefinition);
		obj.ColumnDefinitions.Add(columnDefinition2);
		obj.ColumnDefinitions.Add(columnDefinition3);
		RowDefinition rowDefinition = new RowDefinition();
		RowDefinition rowDefinition2 = new RowDefinition();
		rowDefinition.Height = new GridLength(num2 / 2.0);
		rowDefinition2.Height = new GridLength(1.0, GridUnitType.Star);
		obj.RowDefinitions.Add(rowDefinition);
		obj.RowDefinitions.Add(rowDefinition2);
		Rectangle rectangle = new Rectangle();
		Rectangle rectangle2 = new Rectangle();
		Rectangle rectangle3 = new Rectangle();
		rectangle.SnapsToDevicePixels = true;
		rectangle2.SnapsToDevicePixels = true;
		rectangle3.SnapsToDevicePixels = true;
		rectangle.UseLayoutRounding = true;
		rectangle2.UseLayoutRounding = true;
		rectangle3.UseLayoutRounding = true;
		rectangle.Fill = Brushes.Black;
		rectangle2.Fill = Brushes.Black;
		rectangle3.Fill = Brushes.Black;
		Grid.SetRowSpan(rectangle, 2);
		Grid.SetRow(rectangle, 0);
		Grid.SetColumn(rectangle, 0);
		Grid.SetRow(rectangle2, 1);
		Grid.SetColumn(rectangle2, 1);
		Grid.SetRowSpan(rectangle3, 2);
		Grid.SetRow(rectangle3, 0);
		Grid.SetColumn(rectangle3, 2);
		obj.Children.Add(rectangle);
		obj.Children.Add(rectangle2);
		obj.Children.Add(rectangle3);
		return new VisualBrush(obj);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return new object[1] { Binding.DoNothing };
	}
}
