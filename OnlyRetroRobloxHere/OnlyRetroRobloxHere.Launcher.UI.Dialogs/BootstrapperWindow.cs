using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Markup;

namespace OnlyRetroRobloxHere.Launcher.UI.Dialogs;

public partial class BootstrapperWindow : BootstrapperWindowShared, IComponentConnector
{
	public BootstrapperWindow(Process process)
		: base(process)
	{
		InitializeComponent();
	}

}
