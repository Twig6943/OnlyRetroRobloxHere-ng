using System.ComponentModel;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

public class FancyLazyImage : LazyImage
{
	public FancyLazyImage()
	{
		if (!DesignerProperties.GetIsInDesignMode(this))
		{
			base.Opacity = 0.0;
		}
	}
}
