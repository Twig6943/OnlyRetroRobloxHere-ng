using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.Helpers;

internal static class TreePathItemProvider
{
	public static void AddItems(ObservableCollection<TreePathItem> items, string path, TreePathItem? parent = null)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		foreach (DirectoryInfo directoryInfo2 in directories)
		{
			TreePathDirectoryItem treePathDirectoryItem = new TreePathDirectoryItem
			{
				Name = directoryInfo2.Name,
				Parent = parent
			};
			AddItems(treePathDirectoryItem.Items, directoryInfo2.FullName, treePathDirectoryItem);
			items.Add(treePathDirectoryItem);
		}
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo file in files)
		{
			if (Utils.ValidMapExtensions.Any((string x) => file.Name.EndsWith(x)))
			{
				TreePathFileItem item = new TreePathFileItem
				{
					Name = file.Name,
					Parent = parent
				};
				items.Add(item);
			}
		}
	}
}
