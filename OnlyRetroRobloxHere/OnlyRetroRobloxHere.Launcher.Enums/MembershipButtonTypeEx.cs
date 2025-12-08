using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.Launcher.Enums;

internal static class MembershipButtonTypeEx
{
	public static bool IsMonthly(this MembershipButtonType type)
	{
		if (type == MembershipButtonType.BCMonthly || type == MembershipButtonType.TBCMonthly || type == MembershipButtonType.OBCMonthly)
		{
			return true;
		}
		return false;
	}

	public static bool Is6Months(this MembershipButtonType type)
	{
		if (type == MembershipButtonType.BC6Months || type == MembershipButtonType.TBC6Months)
		{
			return true;
		}
		return false;
	}

	public static bool Is12Months(this MembershipButtonType type)
	{
		if (type == MembershipButtonType.BC12Months || type == MembershipButtonType.TBC12Months)
		{
			return true;
		}
		return false;
	}

	public static bool IsLifetime(this MembershipButtonType type)
	{
		if (type == MembershipButtonType.BCLifetime || type == MembershipButtonType.TBCLifetime)
		{
			return true;
		}
		return false;
	}

	public static bool IsBC(this MembershipButtonType type)
	{
		if ((uint)(type - 1) <= 3u)
		{
			return true;
		}
		return false;
	}

	public static bool IsTBC(this MembershipButtonType type)
	{
		if ((uint)(type - 5) <= 3u)
		{
			return true;
		}
		return false;
	}

	public static bool IsOBC(this MembershipButtonType type)
	{
		return type == MembershipButtonType.OBCMonthly;
	}

	public static MembershipType GetMembershipType(this MembershipButtonType type)
	{
		if (type.IsBC())
		{
			return MembershipType.BuildersClub;
		}
		if (type.IsTBC())
		{
			return MembershipType.TurboBuildersClub;
		}
		if (type.IsOBC())
		{
			return MembershipType.OutrageousBuildersClub;
		}
		return MembershipType.None;
	}
}
