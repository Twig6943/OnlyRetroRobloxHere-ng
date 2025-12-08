using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.XP.Controls;

internal class ImageButton : Button
{
	private bool _hover;

	private bool _pressed;

	public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ImagePropertyChanged));

	public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register("HoverImage", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ImagePropertyChanged));

	public static readonly DependencyProperty PressImageProperty = DependencyProperty.Register("PressImage", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ImagePropertyChanged));

	public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ImagePropertyChanged));

	public ImageSource? Image
	{
		get
		{
			return GetValue(ImageProperty) as ImageSource;
		}
		set
		{
			SetValue(ImageProperty, value);
		}
	}

	public ImageSource? HoverImage
	{
		get
		{
			return GetValue(HoverImageProperty) as ImageSource;
		}
		set
		{
			SetValue(HoverImageProperty, value);
		}
	}

	public ImageSource? PressImage
	{
		get
		{
			return GetValue(PressImageProperty) as ImageSource;
		}
		set
		{
			SetValue(PressImageProperty, value);
		}
	}

	public ImageSource? DisabledImage
	{
		get
		{
			return GetValue(DisabledImageProperty) as ImageSource;
		}
		set
		{
			SetValue(DisabledImageProperty, value);
		}
	}

	private static void ImagePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
	{
		if (source is ImageButton imageButton)
		{
			imageButton.UpdateImage();
		}
	}

	public ImageButton()
	{
		base.MouseEnter += ImageButton_MouseEnter;
		base.MouseLeave += ImageButton_MouseLeave;
		base.PreviewMouseDown += ImageButton_PreviewMouseDown;
		base.PreviewMouseUp += ImageButton_PreviewMouseUp;
		base.IsEnabledChanged += ImageButton_IsEnabledChanged;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		UpdateImage();
	}

	private void UpdateImage()
	{
		if (base.Content is Image image)
		{
			if (!base.IsEnabled)
			{
				image.Source = DisabledImage ?? Image;
			}
			else if (_pressed && PressImage != null)
			{
				image.Source = PressImage;
			}
			else if (_hover && HoverImage != null)
			{
				image.Source = HoverImage;
			}
			else
			{
				image.Source = Image;
			}
		}
	}

	protected override void OnContentChanged(object oldValue, object newValue)
	{
		base.OnContentChanged(oldValue, newValue);
		UpdateImage();
	}

	private void ImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		UpdateImage();
	}

	private void ImageButton_MouseEnter(object sender, MouseEventArgs e)
	{
		_hover = true;
		UpdateImage();
	}

	private void ImageButton_MouseLeave(object sender, MouseEventArgs e)
	{
		_hover = false;
		UpdateImage();
	}

	private void ImageButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		_pressed = true;
		UpdateImage();
	}

	private void ImageButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		_pressed = false;
		UpdateImage();
	}
}
