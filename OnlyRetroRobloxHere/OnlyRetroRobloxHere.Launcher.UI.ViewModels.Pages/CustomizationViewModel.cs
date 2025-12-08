using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Collection;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class CustomizationViewModel : ViewModelBase
{
	private string _colourSelectionModalTitle = "Select a EPICAL!!! Color";

	private Visibility _colourSelectionModalVisibility = Visibility.Collapsed;

	private IReadOnlyDictionary<int, SolidColorBrush> _characterColours = BrickColors.Character;

	private Visibility _figureCharacterTypeModalVisibility = Visibility.Collapsed;

	private List<AvatarItem> _unequippedCategorisedItems = new List<AvatarItem>();

	private bool _showCustomIdField = true;

	private ulong _customId;

	private bool _tabWardrobeSelected = true;

	private bool _tabOutfitsSelected;

	private Visibility _outfitCreateModalVisibility = Visibility.Collapsed;

	private string _outfitCreateName = "";

	private string _outfitCreateClient = "None";

	private string _outfitCreateError = "Error!";

	private Outfit? _outfitSelected;

	private Visibility _outfitDetailsModalVisibility = Visibility.Collapsed;

	private string _outfitDetailsHeaderText = "Outfit \"ROFLCOPTER\"";

	private string _outfitDetailsName = "";

	private string _outfitDetailsClient = "None";

	private string? _outfitDetailsError = "Error!";

	private string? _outfitDetailsSuccess = "Success!";

	private string _outfitDetailsDeleteText = "Delete";

	private string _outfitDetailsWearingText = "1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13";

	private BitmapImage _characterImage = ReadPackImage("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Unavailable.jpg");

	private Visibility _characterProgressIndicatorVisible = Visibility.Collapsed;

	private Visibility _characterRenderModalVisibility = Visibility.Collapsed;

	public int HeadColour
	{
		get
		{
			return Settings.Default.Character.Head;
		}
		set
		{
			if (Settings.Default.Character.Head != value)
			{
				Settings.Default.Character.Head = value;
				OnPropertyChanged("HeadColour");
			}
		}
	}

	public int TorsoColour
	{
		get
		{
			return Settings.Default.Character.Torso;
		}
		set
		{
			if (Settings.Default.Character.Torso != value)
			{
				Settings.Default.Character.Torso = value;
				OnPropertyChanged("TorsoColour");
			}
		}
	}

	public int RightArmColour
	{
		get
		{
			return Settings.Default.Character.RightArm;
		}
		set
		{
			if (Settings.Default.Character.RightArm != value)
			{
				Settings.Default.Character.RightArm = value;
				OnPropertyChanged("RightArmColour");
			}
		}
	}

	public int LeftArmColour
	{
		get
		{
			return Settings.Default.Character.LeftArm;
		}
		set
		{
			if (Settings.Default.Character.LeftArm != value)
			{
				Settings.Default.Character.LeftArm = value;
				OnPropertyChanged("LeftArmColour");
			}
		}
	}

	public int RightLegColour
	{
		get
		{
			return Settings.Default.Character.RightLeg;
		}
		set
		{
			if (Settings.Default.Character.RightLeg != value)
			{
				Settings.Default.Character.RightLeg = value;
				OnPropertyChanged("RightLegColour");
			}
		}
	}

	public int LeftLegColour
	{
		get
		{
			return Settings.Default.Character.LeftLeg;
		}
		set
		{
			if (Settings.Default.Character.LeftLeg != value)
			{
				Settings.Default.Character.LeftLeg = value;
				OnPropertyChanged("LeftLegColour");
			}
		}
	}

	public FigureCharacterType FigureCharacterType
	{
		get
		{
			return Settings.Default.Character.FigureCharacterType;
		}
		set
		{
			if (Settings.Default.Character.FigureCharacterType != value)
			{
				Settings.Default.Character.FigureCharacterType = value;
				OnPropertyChanged("FigureCharacterType");
			}
		}
	}

	public string ColourSelectionModalTitle
	{
		get
		{
			return _colourSelectionModalTitle;
		}
		set
		{
			if (_colourSelectionModalTitle != value)
			{
				_colourSelectionModalTitle = value;
				OnPropertyChanged("ColourSelectionModalTitle");
			}
		}
	}

	public Visibility ColourSelectionModalVisibility
	{
		get
		{
			return _colourSelectionModalVisibility;
		}
		set
		{
			if (_colourSelectionModalVisibility != value)
			{
				_colourSelectionModalVisibility = value;
				OnPropertyChanged("ColourSelectionModalVisibility");
			}
		}
	}

	public IReadOnlyDictionary<int, SolidColorBrush> CharacterColours
	{
		get
		{
			return _characterColours;
		}
		set
		{
			SetProperty(ref _characterColours, value, "CharacterColours");
		}
	}

	public Visibility FigureCharacterTypeModalVisibility
	{
		get
		{
			return _figureCharacterTypeModalVisibility;
		}
		set
		{
			if (_figureCharacterTypeModalVisibility != value)
			{
				_figureCharacterTypeModalVisibility = value;
				OnPropertyChanged("FigureCharacterTypeModalVisibility");
			}
		}
	}

	public static FigureCharacterType[] FigureCharacterTypes => new FigureCharacterType[8]
	{
		FigureCharacterType.Figure1,
		FigureCharacterType.Figure2,
		FigureCharacterType.Figure3,
		FigureCharacterType.Figure4,
		FigureCharacterType.Figure5,
		FigureCharacterType.Figure6,
		FigureCharacterType.Figure7,
		FigureCharacterType.Figure8
	};

	public List<AvatarItem> UnequippedCategorisedItems
	{
		get
		{
			return _unequippedCategorisedItems;
		}
		set
		{
			if (_unequippedCategorisedItems != value)
			{
				_unequippedCategorisedItems = value;
				OnPropertyChanged("UnequippedCategorisedItems");
			}
		}
	}

	public static ObservableCollection<AvatarItem> UnequippedItems => Appearance.Unequipped;

	public static ObservableDictionary<AvatarSlot, AvatarItem> EquippedItems => Appearance.Equipped;

	public int UserId
	{
		get
		{
			return Settings.Default.Player.Id;
		}
		set
		{
			if (value <= 0)
			{
				OnPropertyChanged("UserId");
				return;
			}
			Settings.Default.Player.Id = value;
			OnPropertyChanged("UserId");
		}
	}

	public string Username
	{
		get
		{
			return Settings.Default.Player.Name;
		}
		set
		{
			if (Settings.Default.Player.Name != value)
			{
				Settings.Default.Player.Name = value;
				OnPropertyChanged("Username");
			}
		}
	}

	public bool ShowCustomIdField
	{
		get
		{
			return _showCustomIdField;
		}
		set
		{
			SetProperty(ref _showCustomIdField, value, "ShowCustomIdField");
		}
	}

	public ulong CustomId
	{
		get
		{
			return _customId;
		}
		set
		{
			SetProperty(ref _customId, value, "CustomId");
		}
	}

	public bool TabWardrobeSelected
	{
		get
		{
			return _tabWardrobeSelected;
		}
		set
		{
			SetProperty(ref _tabWardrobeSelected, value, "TabWardrobeSelected");
		}
	}

	public bool TabOutfitsSelected
	{
		get
		{
			return _tabOutfitsSelected;
		}
		set
		{
			SetProperty(ref _tabOutfitsSelected, value, "TabOutfitsSelected");
		}
	}

	public OutfitManager OutfitManager { get; } = new OutfitManager(Settings.Default.OutfitPreferences);

	public ObservableDictionary<string, Outfit> Outfits => OutfitManager.Outfits;

	public ObservableCollection<string> OutfitClients { get; } = new ObservableCollection<string> { "None" };

	public Visibility OutfitCreateModalVisibility
	{
		get
		{
			return _outfitCreateModalVisibility;
		}
		set
		{
			SetProperty(ref _outfitCreateModalVisibility, value, "OutfitCreateModalVisibility");
		}
	}

	public string OutfitCreateName
	{
		get
		{
			return _outfitCreateName;
		}
		set
		{
			SetProperty(ref _outfitCreateName, value, "OutfitCreateName");
		}
	}

	public string OutfitCreateClient
	{
		get
		{
			return _outfitCreateClient;
		}
		set
		{
			SetProperty(ref _outfitCreateClient, value, "OutfitCreateClient");
		}
	}

	public string OutfitCreateError
	{
		get
		{
			return _outfitCreateError;
		}
		set
		{
			SetProperty(ref _outfitCreateError, value, "OutfitCreateError");
		}
	}

	public Outfit? OutfitSelected
	{
		get
		{
			return _outfitSelected;
		}
		set
		{
			SetProperty(ref _outfitSelected, value, "OutfitSelected");
		}
	}

	public Visibility OutfitDetailsModalVisibility
	{
		get
		{
			return _outfitDetailsModalVisibility;
		}
		set
		{
			SetProperty(ref _outfitDetailsModalVisibility, value, "OutfitDetailsModalVisibility");
		}
	}

	public string OutfitDetailsHeaderText
	{
		get
		{
			return _outfitDetailsHeaderText;
		}
		set
		{
			SetProperty(ref _outfitDetailsHeaderText, value, "OutfitDetailsHeaderText");
		}
	}

	public string OutfitDetailsName
	{
		get
		{
			return _outfitDetailsName;
		}
		set
		{
			SetProperty(ref _outfitDetailsName, value, "OutfitDetailsName");
		}
	}

	public string OutfitDetailsClient
	{
		get
		{
			return _outfitDetailsClient;
		}
		set
		{
			SetProperty(ref _outfitDetailsClient, value, "OutfitDetailsClient");
		}
	}

	public string? OutfitDetailsError
	{
		get
		{
			return _outfitDetailsError;
		}
		set
		{
			SetProperty(ref _outfitDetailsError, value, "OutfitDetailsError");
		}
	}

	public string? OutfitDetailsSuccess
	{
		get
		{
			return _outfitDetailsSuccess;
		}
		set
		{
			SetProperty(ref _outfitDetailsSuccess, value, "OutfitDetailsSuccess");
		}
	}

	public string OutfitDetailsDeleteText
	{
		get
		{
			return _outfitDetailsDeleteText;
		}
		set
		{
			SetProperty(ref _outfitDetailsDeleteText, value, "OutfitDetailsDeleteText");
		}
	}

	public string OutfitDetailsWearingText
	{
		get
		{
			return _outfitDetailsWearingText;
		}
		set
		{
			SetProperty(ref _outfitDetailsWearingText, value, "OutfitDetailsWearingText");
		}
	}

	public ObservableCollection<string> OutfitDetailsClients { get; } = new ObservableCollection<string> { "None" };

	public BitmapImage CharacterImage
	{
		get
		{
			return _characterImage;
		}
		set
		{
			SetProperty(ref _characterImage, value, "CharacterImage");
		}
	}

	public Visibility CharacterProgressIndicatorVisible
	{
		get
		{
			return _characterProgressIndicatorVisible;
		}
		set
		{
			SetProperty(ref _characterProgressIndicatorVisible, value, "CharacterProgressIndicatorVisible");
		}
	}

	public Visibility CharacterRenderModalVisibility
	{
		get
		{
			return _characterRenderModalVisibility;
		}
		set
		{
			SetProperty(ref _characterRenderModalVisibility, value, "CharacterRenderModalVisibility");
		}
	}

	private static BitmapImage ReadPackImage(string path)
	{
		return Utils.ToBitmapImage(new Bitmap(Utils.GetStreamFromUri(new Uri(path))));
	}
}
