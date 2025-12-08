using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace OnlyRetroRobloxHere.Launcher.UI.Behaviors;

public class TreeViewBindableSelectedItemBehavior : Behavior<TreeView>
{
	public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeViewBindableSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

	public object SelectedItem
	{
		get
		{
			return GetValue(SelectedItemProperty);
		}
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is TreeViewItem treeViewItem)
		{
			treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		if (base.AssociatedObject != null)
		{
			base.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
		}
	}

	private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		SelectedItem = e.NewValue;
	}
}
