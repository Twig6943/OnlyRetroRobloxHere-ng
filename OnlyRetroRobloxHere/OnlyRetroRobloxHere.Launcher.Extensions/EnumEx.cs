using System;
using System.ComponentModel;
using System.Reflection;

namespace OnlyRetroRobloxHere.Launcher.Extensions;

internal static class EnumEx
{
	public static string GetDescription(this Enum @enum)
	{
		string text = @enum.ToString();
		FieldInfo field = @enum.GetType().GetField(text);
		if (field == null)
		{
			return text;
		}
		DescriptionAttribute descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>(inherit: false);
		if (descriptionAttribute != null)
		{
			return descriptionAttribute.Description;
		}
		return text;
	}
}
