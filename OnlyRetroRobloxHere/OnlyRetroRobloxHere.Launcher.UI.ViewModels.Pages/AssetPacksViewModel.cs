using System.Collections.Generic;
using System.Windows;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class AssetPacksViewModel : ViewModelBase
{
	private Visibility _nothingSelectedVisibility;

	private Visibility _infoVisibility;

	private string _name = "A cool asset pack";

	private string _version = "1.0.0";

	private Visibility _versionVisibility;

	private string _authors = "Matt";

	private Visibility _authorsVisibility;

	private string _description = "A cool description";

	private Visibility _descriptionVisibility;

	private string _clients = "Not 2010L";

	private Visibility _disabledVisibility;

	private string _apiText = "Using V1 API";

	private string _folderText = "Directory name: assetpack";

	public List<AssetPack> AssetPacks => AssetPackManager.Instance.AssetPacks;

	public Visibility NothingSelectedVisibility
	{
		get
		{
			return _nothingSelectedVisibility;
		}
		set
		{
			SetProperty(ref _nothingSelectedVisibility, value, "NothingSelectedVisibility");
		}
	}

	public Visibility InfoVisibility
	{
		get
		{
			return _infoVisibility;
		}
		set
		{
			SetProperty(ref _infoVisibility, value, "InfoVisibility");
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			SetProperty(ref _name, value, "Name");
		}
	}

	public string Version
	{
		get
		{
			return _version;
		}
		set
		{
			SetProperty(ref _version, value, "Version");
		}
	}

	public Visibility VersionVisibility
	{
		get
		{
			return _versionVisibility;
		}
		set
		{
			SetProperty(ref _versionVisibility, value, "VersionVisibility");
		}
	}

	public string Authors
	{
		get
		{
			return _authors;
		}
		set
		{
			SetProperty(ref _authors, value, "Authors");
		}
	}

	public Visibility AuthorsVisibility
	{
		get
		{
			return _authorsVisibility;
		}
		set
		{
			SetProperty(ref _authorsVisibility, value, "AuthorsVisibility");
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			SetProperty(ref _description, value, "Description");
		}
	}

	public Visibility DescriptionVisibility
	{
		get
		{
			return _descriptionVisibility;
		}
		set
		{
			SetProperty(ref _descriptionVisibility, value, "DescriptionVisibility");
		}
	}

	public string Clients
	{
		get
		{
			return _clients;
		}
		set
		{
			SetProperty(ref _clients, value, "Clients");
		}
	}

	public Visibility DisabledVisibility
	{
		get
		{
			return _disabledVisibility;
		}
		set
		{
			SetProperty(ref _disabledVisibility, value, "DisabledVisibility");
		}
	}

	public string ApiText
	{
		get
		{
			return _apiText;
		}
		set
		{
			SetProperty(ref _apiText, value, "ApiText");
		}
	}

	public string FolderText
	{
		get
		{
			return _folderText;
		}
		set
		{
			SetProperty(ref _folderText, value, "FolderText");
		}
	}
}
