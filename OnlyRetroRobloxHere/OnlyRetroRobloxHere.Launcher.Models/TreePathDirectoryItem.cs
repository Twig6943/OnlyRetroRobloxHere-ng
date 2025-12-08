using System.Collections.ObjectModel;

namespace OnlyRetroRobloxHere.Launcher.Models;

internal class TreePathDirectoryItem : TreePathItem
{
	public ObservableCollection<TreePathItem> Items { get; set; } = new ObservableCollection<TreePathItem>();
}
