using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OnlyRetroRobloxHere.Launcher.UI.Controls;

public class LazyImage : Image
{
	public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register("ImagePath", typeof(Uri), typeof(LazyImage), new PropertyMetadata(OnImagePathChanged));

	public Uri ImagePath
	{
		get
		{
			return (Uri)GetValue(ImagePathProperty);
		}
		set
		{
			SetValue(ImagePathProperty, value);
		}
	}

	private static async void OnImagePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		await ((LazyImage)d).LoadImageAsync((Uri)e.NewValue);
	}

	private Stream GetStreamFromUri(Uri uri)
	{
		string absoluteUri = uri.AbsoluteUri;
		if (absoluteUri.StartsWith("file://"))
		{
			string text = absoluteUri;
			int num = (absoluteUri.StartsWith("file:///") ? 8 : 7);
			return File.OpenRead(HttpUtility.UrlDecode(text.Substring(num, text.Length - num)));
		}
		if (absoluteUri.StartsWith("pack://"))
		{
			return Application.GetResourceStream(uri).Stream;
		}
		throw new NotImplementedException("Uri not handled: " + absoluteUri);
	}

	private bool DoesImageExist(Uri uri)
	{
		if (uri == null)
		{
			return false;
		}
		string absoluteUri = uri.AbsoluteUri;
		if (absoluteUri.StartsWith("file://"))
		{
			string text = absoluteUri;
			int num = (absoluteUri.StartsWith("file:///") ? 8 : 7);
			return File.Exists(HttpUtility.UrlDecode(text.Substring(num, text.Length - num)));
		}
		return true;
	}

    private async Task LoadImageAsync(Uri imagePath)
    {
        if (!DoesImageExist(imagePath))
        {
            return;
        }
        base.Source = await Task.Run(delegate
        {
            using Stream streamSource = GetStreamFromUri(imagePath);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = streamSource;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        });
    }
}
