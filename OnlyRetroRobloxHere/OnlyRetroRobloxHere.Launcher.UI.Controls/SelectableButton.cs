using System.Windows;
using System.Windows.Controls;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

internal class SelectableButton : Button
{
	public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(SelectableButton), new PropertyMetadata(false));

	public bool IsSelected
	{
		get
		{
			return (bool)GetValue(IsSelectedProperty);
		}
		set
		{
			SetValue(IsSelectedProperty, value);
		}
	}
}
