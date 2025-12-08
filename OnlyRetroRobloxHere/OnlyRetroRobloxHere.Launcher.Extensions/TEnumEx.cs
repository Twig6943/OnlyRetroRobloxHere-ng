using System;

namespace OnlyRetroRobloxHere.Launcher.Extensions;

internal static class TEnumEx
{
	public static string GetDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
	{
		return ((Enum)(object)EnumValue).GetDescription();
	}
}
