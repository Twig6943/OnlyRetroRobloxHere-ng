using System.Collections.Generic;
using System.IO;

namespace OnlyRetroRobloxHere.Launcher.Models;

internal abstract class TreePathItem
{
	public TreePathItem? Parent { get; set; }

	public string Name { get; set; }

	public List<string> GetPaths()
	{
		List<string> list = new List<string> { Name };
		for (TreePathItem parent = Parent; parent != null; parent = parent.Parent)
		{
			list.Insert(0, parent.Name);
		}
		return list;
	}

	public string GetPath()
	{
		return Path.Combine(GetPaths().ToArray());
	}
}
