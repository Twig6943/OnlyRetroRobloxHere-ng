using System.Windows;
using System.Windows.Controls;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

public class StandardBox : ContentControl
{
	public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(StandardBox), new FrameworkPropertyMetadata("Standard Box", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(StandardBox), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public string HeaderText
	{
		get
		{
			return (string)GetValue(HeaderTextProperty);
		}
		set
		{
			SetValue(HeaderTextProperty, value);
		}
	}

	public Visibility HeaderVisibility
	{
		get
		{
			return (Visibility)GetValue(HeaderVisibilityProperty);
		}
		set
		{
			SetValue(HeaderVisibilityProperty, value);
		}
	}
}
