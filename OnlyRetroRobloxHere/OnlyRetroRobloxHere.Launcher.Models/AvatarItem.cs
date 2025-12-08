using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Launcher.Extensions;

namespace OnlyRetroRobloxHere.Launcher.Models;

internal class AvatarItem : IComparable<AvatarItem>
{
	[JsonIgnore]
	public ulong Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = "";

	[JsonPropertyName("description")]
	public string Description { get; set; } = "";

	[JsonIgnore]
	public Uri Image => GetImage();

	[JsonPropertyName("type")]
	public AvatarAssetType Type { get; set; }

	public string TypeName => Type.GetDescription();

	public string TypeNamePlural
	{
		get
		{
			if (Type == AvatarAssetType.Pants)
			{
				return TypeName;
			}
			return TypeName + "s";
		}
	}

	[JsonPropertyName("creator")]
	public string? Creator { get; set; }

	[JsonPropertyName("created")]
	public DateTime CreationDate { get; set; } = DateTime.MinValue;

	[JsonPropertyName("items")]
	public List<ulong>? Items { get; set; }

	[JsonPropertyName("assetVersion")]
	public int AssetVersion { get; set; }

	[JsonIgnore]
	public bool Custom { get; set; }

	public Uri GetImage()
	{
		if (Custom)
		{
			return Type switch
			{
				AvatarAssetType.TShirt => new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Thumbnails/CustomItems/tshirt.png"), 
				AvatarAssetType.Shirt => new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Thumbnails/CustomItems/shirt.png"), 
				AvatarAssetType.Pants => new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Thumbnails/CustomItems/pants.png"), 
				_ => throw new Exception($"Unknown custom asset type {Type}"), 
			};
		}
		string text = Path.Combine(PathHelper.ThumbnailsDeprecated, $"{Id}.png");
		if (File.Exists(text))
		{
			return new Uri(text);
		}
		return new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Thumbnails/default.png");
	}

	public static AvatarItem GetCustom(ulong id, AvatarAssetType type)
	{
		return new AvatarItem
		{
			Id = id,
			Name = $"Custom {type.GetDescription()}: {id}",
			Type = type,
			Custom = true
		};
	}

	public int CompareTo(AvatarItem? other)
	{
		if (other == null)
		{
			return 1;
		}
		return Name.CompareTo(other.Name);
	}

	public override string ToString()
	{
		return $"Avatar Item {Id}";
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (!(obj is AvatarItem avatarItem))
		{
			return false;
		}
		return avatarItem.Id == Id;
	}

	public override int GetHashCode()
	{
		return (((((17 * 23 + Id.GetHashCode()) * 23 + Name.GetHashCode()) * 23 + Description.GetHashCode()) * 23 + Image.GetHashCode()) * 23 + Type.GetHashCode()) * 23 + (Items?.GetHashCode() ?? 0);
	}

	public static bool operator ==(AvatarItem? a, AvatarItem? b)
	{
		return a?.Equals(b) ?? ((object)b == null);
	}

	public static bool operator !=(AvatarItem? a, AvatarItem? b)
	{
		return !(a == b);
	}
}
