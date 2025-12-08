using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

public class SnowfallCanvas : Canvas
{
	public enum SnowflakeAnimation
	{
		None,
		Fade
	}

	private static readonly BitmapImage[] _sparkleImages = new BitmapImage[5]
	{
		new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Snowfall/Snowflake1.png")),
		new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Snowfall/Snowflake2.png")),
		new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Snowfall/Snowflake3.png")),
		new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Snowfall/Snowflake4.png")),
		new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Snowfall/Snowflake5.png"))
	};

	private readonly Random _random = new Random();

	private DispatcherTimer? _timer;

	public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register("ScaleFactor", typeof(double), typeof(SnowfallCanvas), new PropertyMetadata(0.5));

	public static readonly DependencyProperty EmissionRateProperty = DependencyProperty.Register("EmissionRate", typeof(int), typeof(SnowfallCanvas), new PropertyMetadata(5));

	public static readonly DependencyProperty OpacityFactorProperty = DependencyProperty.Register("OpacityFactor", typeof(double), typeof(SnowfallCanvas), new PropertyMetadata(3.0));

	public static readonly DependencyProperty ParticleSpeedProperty = DependencyProperty.Register("ParticleSpeed", typeof(double), typeof(SnowfallCanvas), new PropertyMetadata(1.0));

	public static readonly DependencyProperty LeaveAnimationProperty = DependencyProperty.Register("LeaveAnimation", typeof(SnowflakeAnimation), typeof(SnowfallCanvas), new PropertyMetadata(SnowflakeAnimation.None));

	public double ScaleFactor
	{
		get
		{
			return (double)GetValue(ScaleFactorProperty);
		}
		set
		{
			SetValue(ScaleFactorProperty, value);
		}
	}

	public int EmissionRate
	{
		get
		{
			return (int)GetValue(EmissionRateProperty);
		}
		set
		{
			SetValue(EmissionRateProperty, value);
		}
	}

	public double OpacityFactor
	{
		get
		{
			return (double)GetValue(OpacityFactorProperty);
		}
		set
		{
			SetValue(OpacityFactorProperty, value);
		}
	}

	public double ParticleSpeed
	{
		get
		{
			return (double)GetValue(ParticleSpeedProperty);
		}
		set
		{
			SetValue(ParticleSpeedProperty, value);
		}
	}

	public SnowflakeAnimation LeaveAnimation
	{
		get
		{
			return (SnowflakeAnimation)GetValue(LeaveAnimationProperty);
		}
		set
		{
			SetValue(LeaveAnimationProperty, value);
		}
	}

	public SnowfallCanvas()
	{
		base.Loaded += Snowfall_Loaded;
		base.Unloaded += Snowfall_Unloaded;
	}

	private void Snowfall_Unloaded(object sender, RoutedEventArgs e)
	{
		Stop();
	}

	private void Snowfall_Loaded(object sender, RoutedEventArgs e)
	{
		Start();
	}

	public void Stop()
	{
		_timer?.Stop();
		base.Children.Clear();
	}

	public void Start()
	{
		if (base.Visibility == Visibility.Visible && base.IsLoaded)
		{
			_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds((int)(1000.0 / (double)EmissionRate))
			};
			_timer.Tick += delegate
			{
				EmitSnowflake();
			};
			_timer.Start();
		}
	}

	private void EmitSnowflake()
	{
		int num = _random.Next(0, (int)base.ActualWidth);
		double num2 = (_random.NextDouble() * 0.6 + 0.5) * ScaleFactor;
		int num3 = _random.Next(0, 270);
		RotateTransform value = new RotateTransform(num3);
		ScaleTransform value2 = new ScaleTransform(num2, num2);
		TranslateTransform value3 = new TranslateTransform(num, 0.0 - 50.0 * ScaleFactor);
		Duration duration = new Duration(TimeSpan.FromSeconds((double)_random.Next(8, 10) * (1.0 / ParticleSpeed)));
		Duration duration2 = new Duration(TimeSpan.FromSeconds(2.0));
		Image flake = new Image();
		flake.Source = _sparkleImages[_random.Next(_sparkleImages.Length)];
		flake.Height = 32.0;
		flake.Width = 32.0;
		flake.RenderTransformOrigin = new Point(0.5, 0.5);
		flake.Opacity = (_random.NextDouble() * 0.5 + 0.5) * OpacityFactor;
		flake.HorizontalAlignment = HorizontalAlignment.Left;
		flake.VerticalAlignment = VerticalAlignment.Top;
		flake.RenderTransform = new TransformGroup
		{
			Children = new TransformCollection { value, value2, value3 }
		};
		base.Children.Add(flake);
		DoubleAnimation value4 = GenerateAnimation(num + _random.Next(-10, 10), duration, flake, "RenderTransform.Children[2].X");
		DoubleAnimation value5 = GenerateAnimation((int)(base.ActualHeight - 32.0 + 50.0 * ScaleFactor), duration, flake, "RenderTransform.Children[2].Y");
		num3 += _random.Next(90, 360);
		DoubleAnimation value6 = GenerateAnimation(num3, duration, flake, "RenderTransform.Children[0].Angle");
		DoubleAnimation value7 = GenerateAnimation(0.0, duration2, flake, "Opacity", duration.Subtract(duration2).TimeSpan);
		Storyboard story = new Storyboard();
		story.Children.Add(value4);
		story.Children.Add(value5);
		story.Children.Add(value6);
		if (LeaveAnimation == SnowflakeAnimation.Fade)
		{
			story.Children.Add(value7);
		}
		flake.Loaded += delegate
		{
			story.Begin();
		};
		flake.MouseEnter += delegate(object sender, MouseEventArgs args)
		{
			args.Handled = false;
		};
		story.Completed += delegate
		{
			base.Children.Remove(flake);
		};
	}

	private static DoubleAnimation GenerateAnimation(double x, Duration duration, Image flake, string propertyPath, TimeSpan? beginTime = null)
	{
		DoubleAnimation obj = new DoubleAnimation
		{
			BeginTime = (beginTime ?? TimeSpan.Zero),
			To = x,
			Duration = duration
		};
		Storyboard.SetTarget(obj, flake);
		Storyboard.SetTargetProperty(obj, new PropertyPath(propertyPath));
		return obj;
	}
}
