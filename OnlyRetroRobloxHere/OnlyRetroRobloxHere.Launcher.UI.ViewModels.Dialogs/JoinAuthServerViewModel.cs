namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Dialogs;

internal class JoinAuthServerViewModel : ViewModelBase
{
	private string _status = "Status";

	private bool _keyTextBoxEnabled;

	private bool _submitButtonEnabled;

	private string _keyText = "";

	public string Status
	{
		get
		{
			return _status;
		}
		set
		{
			SetProperty(ref _status, value, "Status");
		}
	}

	public bool KeyTextBoxEnabled
	{
		get
		{
			return _keyTextBoxEnabled;
		}
		set
		{
			SetProperty(ref _keyTextBoxEnabled, value, "KeyTextBoxEnabled");
		}
	}

	public bool SubmitButtonEnabled
	{
		get
		{
			return _submitButtonEnabled;
		}
		set
		{
			SetProperty(ref _submitButtonEnabled, value, "SubmitButtonEnabled");
		}
	}

	public string KeyText
	{
		get
		{
			return _keyText;
		}
		set
		{
			SetProperty(ref _keyText, value, "KeyText");
		}
	}
}
