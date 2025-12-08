using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Markup;

namespace OnlyRetroRobloxHere.Launcher.UI.Dialogs;

public partial class BootstrapperRetroWindow : BootstrapperWindowShared, IComponentConnector
{
	public BootstrapperRetroWindow(Process process)
		: base(process)
	{
		InitializeComponent();
	}

}
