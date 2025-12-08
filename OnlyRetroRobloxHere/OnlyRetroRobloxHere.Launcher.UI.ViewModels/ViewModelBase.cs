using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels;

internal class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
	{
		if (!object.Equals(field, newValue))
		{
			field = newValue;
			OnPropertyChanged(propertyName);
			return true;
		}
		return false;
	}

	public virtual void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
