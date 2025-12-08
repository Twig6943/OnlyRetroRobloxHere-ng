using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace Wpf.XP.Controls;

public class WindowFrame : ContentControl
{
	private const int TitleBarHeight = 30;

	private const int ResizeBorderSize = 4;

	private const int MinimumHeight = 34;

	private const int MinimumWidth = 115;

	private bool _loaded;

	private Image _icon;

	private Window _window;

	private TextBlock _title;

	private Image _titleBarLeft;

	private Image _titleBarMiddle;

	private Image _titleBarRight;

	private Image _sideLeft;

	private Image _sideRight;

	private Image _bottomLeft;

	private Image _bottomMiddle;

	private Image _bottomRight;

	private ImageButton _minimizeButton;

	private ImageButton _maximizeButton;

	private ImageButton _restoreButton;

	private ImageButton _closeButton;

	private WindowChrome _windowChrome;

	public static readonly DependencyProperty TitleProperty;

	public static readonly DependencyProperty IconProperty;

	public static readonly DependencyProperty IconVisibleProperty;

	public static readonly DependencyProperty ResizeModeProperty;

	public string? Title
	{
		get
		{
			return GetValue(TitleProperty) as string;
		}
		set
		{
			SetValue(TitleProperty, value);
		}
	}

	public ImageSource? Icon
	{
		get
		{
			return GetValue(IconProperty) as ImageSource;
		}
		set
		{
			SetValue(IconProperty, value);
		}
	}

	public bool IconVisible
	{
		get
		{
			return (bool)GetValue(IconVisibleProperty);
		}
		set
		{
			SetValue(IconVisibleProperty, value);
		}
	}

	public ResizeMode ResizeMode
	{
		get
		{
			return (ResizeMode)GetValue(ResizeModeProperty);
		}
		set
		{
			SetValue(ResizeModeProperty, value);
		}
	}

	private static void IconVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
	{
		if (source is WindowFrame { _loaded: not false } windowFrame)
		{
			bool iconVisibility = (bool)e.NewValue;
			windowFrame.SetIconVisibility(iconVisibility);
		}
	}

	private static void ResizeModePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
	{
		if (source is WindowFrame { _loaded: not false } windowFrame)
		{
			ResizeMode resizeMode = (ResizeMode)e.NewValue;
			windowFrame.UpdateTitlebarButtons(resizeMode);
		}
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		bool isInDesignMode = DesignerProperties.GetIsInDesignMode(this);
		_icon = FindChild<Image>(this, "Icon");
		_title = FindChild<TextBlock>(this, "Title");
		_titleBarLeft = FindChild<Image>(this, "TitleBarLeft");
		_titleBarMiddle = FindChild<Image>(this, "TitleBarMiddle");
		_titleBarRight = FindChild<Image>(this, "TitleBarRight");
		_sideLeft = FindChild<Image>(this, "SideLeft");
		_sideRight = FindChild<Image>(this, "SideRight");
		_bottomLeft = FindChild<Image>(this, "BottomLeft");
		_bottomMiddle = FindChild<Image>(this, "BottomMiddle");
		_bottomRight = FindChild<Image>(this, "BottomRight");
		_minimizeButton = FindChild<ImageButton>(this, "MinimizeButton");
		_maximizeButton = FindChild<ImageButton>(this, "MaximizeButton");
		_restoreButton = FindChild<ImageButton>(this, "RestoreButton");
		_closeButton = FindChild<ImageButton>(this, "CloseButton");
		_windowChrome = new WindowChrome
		{
			ResizeBorderThickness = new Thickness(4.0),
			CaptionHeight = 26.0,
			GlassFrameThickness = new Thickness(4.0)
		};
		UpdateTitlebarButtons(ResizeMode);
		SetIconVisibility(IconVisible);
		if (!isInDesignMode)
		{
			_window = Window.GetWindow(this);
			_window.SizeChanged += Window_SizeChanged;
			CheckWindowSize();
			HookWindowProc();
			_window.StateChanged += Window_StateChanged;
			_window.Activated += Window_Activated;
			_window.Deactivated += Window_Deactivated;
			_minimizeButton.Click += MinimizeButton_Click;
			_maximizeButton.Click += MaximizeButton_Click;
			_restoreButton.Click += RestoreButton_Click;
			_closeButton.Click += CloseButton_Click;
			WindowChrome.SetWindowChrome(_window, _windowChrome);
		}
		_loaded = true;
	}

	private void HookWindowProc()
	{
		HwndSource.FromHwnd(new WindowInteropHelper(_window).Handle)?.AddHook(WindowProc);
	}

	private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == 36)
		{
			Native.WmGetMinMaxInfo(hwnd, lParam, (int)base.MinWidth, (int)base.MinHeight);
			handled = true;
		}
		return (IntPtr)0;
	}

	private void CheckWindowSize()
	{
		if (_window.Height < 34.0)
		{
			_window.Height = 34.0;
		}
		if (_window.Width < 115.0)
		{
			_window.Width = 115.0;
		}
	}

	private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		CheckWindowSize();
	}

	private void UpdateTitlebarButtons(ResizeMode resizeMode)
	{
		if (resizeMode == ResizeMode.NoResize || resizeMode == ResizeMode.CanMinimize)
		{
			_windowChrome.CaptionHeight = 30.0;
			_windowChrome.ResizeBorderThickness = new Thickness(0.0);
		}
		else
		{
			_windowChrome.CaptionHeight = 26.0;
			_windowChrome.ResizeBorderThickness = new Thickness(4.0);
		}
		if (resizeMode == ResizeMode.NoResize)
		{
			_minimizeButton.Visibility = Visibility.Collapsed;
			_maximizeButton.Visibility = Visibility.Collapsed;
			_restoreButton.Visibility = Visibility.Collapsed;
			return;
		}
		_maximizeButton.IsEnabled = resizeMode != ResizeMode.CanMinimize;
		_restoreButton.IsEnabled = resizeMode != ResizeMode.CanMinimize;
		WindowState windowState = ((_window != null) ? _window.WindowState : WindowState.Normal);
		_minimizeButton.Visibility = Visibility.Visible;
		_maximizeButton.Visibility = ((windowState == WindowState.Maximized) ? Visibility.Collapsed : Visibility.Visible);
		_restoreButton.Visibility = ((windowState != WindowState.Maximized) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void SetIconVisibility(bool visible)
	{
		Visibility visibility = ((!visible) ? Visibility.Collapsed : Visibility.Visible);
		_icon.Visibility = visibility;
	}

	private ImageSource CreateImageSource(string uri)
	{
		return new BitmapImage(new Uri("pack://application:,,,/Wpf.XP;component/Resources/Frame/" + uri + ".png"));
	}

	private DropShadowEffect CreateNewDropShadowEffect(DropShadowEffect effect, double newOpacity)
	{
		return new DropShadowEffect
		{
			BlurRadius = effect.BlurRadius,
			Direction = effect.Direction,
			Opacity = newOpacity,
			ShadowDepth = effect.ShadowDepth,
			Color = effect.Color
		};
	}

	private void ChangeActive(bool isActive)
	{
		string text = (isActive ? "" : "Inactive");
		_titleBarLeft.Source = CreateImageSource("TitleLeft" + text);
		_titleBarMiddle.Source = CreateImageSource("TitleMiddle" + text);
		_titleBarRight.Source = CreateImageSource("TitleRight" + text);
		_sideLeft.Source = CreateImageSource("SideLeft" + text);
		_sideRight.Source = CreateImageSource("SideRight" + text);
		_bottomLeft.Source = CreateImageSource("BottomLeft" + text);
		_bottomMiddle.Source = CreateImageSource("BottomMiddle" + text);
		_bottomRight.Source = CreateImageSource("BottomRight" + text);
		_minimizeButton.Image = CreateImageSource("MinimizeButton" + text);
		_maximizeButton.Image = CreateImageSource("MaximizeButton" + text);
		_maximizeButton.DisabledImage = CreateImageSource("MaximizeButtonDisabled" + text);
		_restoreButton.Image = CreateImageSource("RestoreButton" + text);
		_restoreButton.DisabledImage = CreateImageSource("RestoreButtonDisabled" + text);
		_closeButton.Image = CreateImageSource("CloseButton" + text);
		Color color = new Color
		{
			R = (byte)(isActive ? 255u : 216u),
			G = (byte)(isActive ? 255u : 228u),
			B = (byte)(isActive ? 255u : 248u),
			A = byte.MaxValue
		};
		_title.Foreground = new SolidColorBrush(color);
		DropShadowEffect effect = CreateNewDropShadowEffect((DropShadowEffect)_title.Effect, isActive ? 1.0 : 0.0);
		_title.Effect = effect;
	}

	private void Window_Activated(object? sender, EventArgs e)
	{
		ChangeActive(isActive: true);
	}

	private void Window_Deactivated(object? sender, EventArgs e)
	{
		ChangeActive(isActive: false);
	}

	private void MinimizeButton_Click(object sender, RoutedEventArgs e)
	{
		_window.WindowState = WindowState.Minimized;
	}

	private void MaximizeButton_Click(object sender, RoutedEventArgs e)
	{
		_window.WindowState = WindowState.Maximized;
	}

	private void RestoreButton_Click(object sender, RoutedEventArgs e)
	{
		_window.WindowState = WindowState.Normal;
	}

	private void CloseButton_Click(object sender, RoutedEventArgs e)
	{
		_window.Close();
	}

	private void Window_StateChanged(object? sender, EventArgs e)
	{
		if (_window.WindowState == WindowState.Maximized)
		{
			_maximizeButton.Visibility = Visibility.Collapsed;
			_restoreButton.Visibility = Visibility.Visible;
		}
		else
		{
			_maximizeButton.Visibility = Visibility.Visible;
			_restoreButton.Visibility = Visibility.Collapsed;
		}
	}

	public static T? FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
	{
		if (parent == null)
		{
			return null;
		}
		T val = null;
		int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
		for (int i = 0; i < childrenCount; i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(parent, i);
			if (child as T == null)
			{
				val = FindChild<T>(child, childName);
				if (val != null)
				{
					break;
				}
				continue;
			}
			if (!string.IsNullOrEmpty(childName))
			{
				if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
				{
					val = (T)child;
					break;
				}
				continue;
			}
			val = (T)child;
			break;
		}
		return val;
	}

	static WindowFrame()
	{
		TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WindowFrame), new FrameworkPropertyMetadata("Binbows xp", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(WindowFrame), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		IconVisibleProperty = DependencyProperty.Register("IconVisible", typeof(bool), typeof(WindowFrame), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IconVisiblePropertyChanged));
		ResizeModeProperty = DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(WindowFrame), new FrameworkPropertyMetadata(ResizeMode.CanResize, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ResizeModePropertyChanged));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowFrame), new FrameworkPropertyMetadata(typeof(WindowFrame)));
	}
}
