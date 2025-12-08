using System.Windows.Controls;
using System.Windows.Media;

namespace OnlyRetroRobloxHere.Launcher.UI.Helpers;

internal static class TreeViewHelper
{
	public static TreeViewItem? GetTreeViewItem(ItemsControl container, object item)
	{
		if (container != null)
		{
			if (container.DataContext == item)
			{
				return container as TreeViewItem;
			}
			if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
			{
				container.SetValue(TreeViewItem.IsExpandedProperty, true);
			}
			container.ApplyTemplate();
			ItemsPresenter itemsPresenter = (ItemsPresenter)container.Template.FindName("ItemsHost", container);
			if (itemsPresenter != null)
			{
				itemsPresenter.ApplyTemplate();
			}
			else
			{
				itemsPresenter = FindVisualChild<ItemsPresenter>(container);
				if (itemsPresenter == null)
				{
					container.UpdateLayout();
					itemsPresenter = FindVisualChild<ItemsPresenter>(container);
				}
			}
			Panel obj = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);
			_ = obj.Children;
			VirtualizingStackPanel virtualizingStackPanel = obj as VirtualizingStackPanel;
			int i = 0;
			for (int count = container.Items.Count; i < count; i++)
			{
				TreeViewItem treeViewItem;
				if (virtualizingStackPanel != null)
				{
					treeViewItem = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
				}
				else
				{
					treeViewItem = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
					treeViewItem.BringIntoView();
				}
				if (treeViewItem != null)
				{
					TreeViewItem treeViewItem2 = GetTreeViewItem(treeViewItem, item);
					if (treeViewItem2 != null)
					{
						return treeViewItem2;
					}
					treeViewItem.IsExpanded = false;
				}
			}
		}
		return null;
	}

	private static T? FindVisualChild<T>(Visual visual) where T : Visual
	{
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
		{
			Visual visual2 = (Visual)VisualTreeHelper.GetChild(visual, i);
			if (visual2 != null)
			{
				if (visual2 is T result)
				{
					return result;
				}
				T val = FindVisualChild<T>(visual2);
				if (val != null)
				{
					return val;
				}
			}
		}
		return null;
	}
}
