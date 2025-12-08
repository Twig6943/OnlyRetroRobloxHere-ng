using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
[TemplatePart(Name = "PART_Toggle", Type = typeof(CheckBox))]
public class DropdownMenu : ContentControl
{
	private const string PART_POPUP_NAME = "PART_Popup";

	private const string PART_TOGGLE_NAME = "PART_Toggle";

	private Popup? _popup;

	private CheckBox? _toggle;

	public static readonly DependencyProperty IsOpenProperty;

	public bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenProperty);
		}
		set
		{
			SetValue(IsOpenProperty, value);
		}
	}

	static DropdownMenu()
	{
		IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(DropdownMenu), new PropertyMetadata(false));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownMenu), new FrameworkPropertyMetadata(typeof(DropdownMenu)));
	}

	public override void OnApplyTemplate()
	{
		_popup = base.Template.FindName("PART_Popup", this) as Popup;
		if (_popup != null)
		{
			_popup.Closed += Popup_Closed;
		}
		_toggle = base.Template.FindName("PART_Toggle", this) as CheckBox;
		base.OnApplyTemplate();
	}

	private void Popup_Closed(object? sender, EventArgs e)
	{
		if (_toggle != null && !_toggle.IsMouseOver)
		{
			IsOpen = false;
		}
	}
}
