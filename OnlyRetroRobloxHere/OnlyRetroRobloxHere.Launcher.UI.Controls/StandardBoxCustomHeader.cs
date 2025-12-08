using System.Windows;
using System.Windows.Controls;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

public class StandardBoxCustomHeader : ContentControl
{
	public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(object), typeof(StandardBoxCustomHeader), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public object? HeaderContent
	{
		get
		{
			return GetValue(HeaderContentProperty);
		}
		set
		{
			SetValue(HeaderContentProperty, value);
		}
	}
}
