using System.Collections.Generic;
using System.Windows.Media;

namespace OnlyRetroRobloxHere.Launcher;

internal class BrickColors
{
	public static IReadOnlyDictionary<int, SolidColorBrush> Colours { get; } = new Dictionary<int, SolidColorBrush>
	{
		[45] = new SolidColorBrush(Color.FromRgb(180, 210, 228)),
		[1024] = new SolidColorBrush(Color.FromRgb(175, 221, byte.MaxValue)),
		[11] = new SolidColorBrush(Color.FromRgb(128, 187, 220)),
		[102] = new SolidColorBrush(Color.FromRgb(110, 153, 202)),
		[23] = new SolidColorBrush(Color.FromRgb(13, 105, 172)),
		[1010] = new SolidColorBrush(Color.FromRgb(0, 0, byte.MaxValue)),
		[1012] = new SolidColorBrush(Color.FromRgb(33, 84, 185)),
		[1011] = new SolidColorBrush(Color.FromRgb(0, 32, 96)),
		[1027] = new SolidColorBrush(Color.FromRgb(159, 243, 233)),
		[1018] = new SolidColorBrush(Color.FromRgb(18, 238, 212)),
		[151] = new SolidColorBrush(Color.FromRgb(120, 144, 130)),
		[1022] = new SolidColorBrush(Color.FromRgb(127, 142, 100)),
		[135] = new SolidColorBrush(Color.FromRgb(116, 134, 157)),
		[1019] = new SolidColorBrush(Color.FromRgb(0, byte.MaxValue, byte.MaxValue)),
		[1013] = new SolidColorBrush(Color.FromRgb(4, 175, 236)),
		[107] = new SolidColorBrush(Color.FromRgb(0, 143, 156)),
		[1028] = new SolidColorBrush(Color.FromRgb(204, byte.MaxValue, 204)),
		[29] = new SolidColorBrush(Color.FromRgb(161, 196, 140)),
		[119] = new SolidColorBrush(Color.FromRgb(164, 189, 71)),
		[37] = new SolidColorBrush(Color.FromRgb(75, 151, 75)),
		[1021] = new SolidColorBrush(Color.FromRgb(58, 125, 21)),
		[1020] = new SolidColorBrush(Color.FromRgb(0, byte.MaxValue, 0)),
		[28] = new SolidColorBrush(Color.FromRgb(40, 127, 71)),
		[141] = new SolidColorBrush(Color.FromRgb(39, 70, 45)),
		[1029] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, 204)),
		[226] = new SolidColorBrush(Color.FromRgb(253, 234, 141)),
		[1008] = new SolidColorBrush(Color.FromRgb(193, 190, 66)),
		[24] = new SolidColorBrush(Color.FromRgb(245, 205, 48)),
		[1017] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 175, 0)),
		[1009] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, 0)),
		[1005] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 175, 0)),
		[105] = new SolidColorBrush(Color.FromRgb(226, 155, 64)),
		[1025] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 201, 201)),
		[125] = new SolidColorBrush(Color.FromRgb(234, 184, 146)),
		[101] = new SolidColorBrush(Color.FromRgb(218, 134, 122)),
		[1007] = new SolidColorBrush(Color.FromRgb(163, 75, 75)),
		[1016] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 102, 204)),
		[1032] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 0, 191)),
		[1004] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 0, 0)),
		[21] = new SolidColorBrush(Color.FromRgb(196, 40, 28)),
		[9] = new SolidColorBrush(Color.FromRgb(232, 186, 200)),
		[1026] = new SolidColorBrush(Color.FromRgb(177, 167, byte.MaxValue)),
		[1006] = new SolidColorBrush(Color.FromRgb(180, 128, byte.MaxValue)),
		[153] = new SolidColorBrush(Color.FromRgb(149, 121, 119)),
		[1023] = new SolidColorBrush(Color.FromRgb(140, 91, 159)),
		[1015] = new SolidColorBrush(Color.FromRgb(170, 0, 170)),
		[1031] = new SolidColorBrush(Color.FromRgb(98, 37, 209)),
		[104] = new SolidColorBrush(Color.FromRgb(107, 50, 124)),
		[5] = new SolidColorBrush(Color.FromRgb(215, 197, 154)),
		[1030] = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 204, 153)),
		[18] = new SolidColorBrush(Color.FromRgb(204, 142, 105)),
		[106] = new SolidColorBrush(Color.FromRgb(218, 133, 65)),
		[38] = new SolidColorBrush(Color.FromRgb(160, 95, 53)),
		[1014] = new SolidColorBrush(Color.FromRgb(170, 85, 0)),
		[217] = new SolidColorBrush(Color.FromRgb(124, 92, 70)),
		[192] = new SolidColorBrush(Color.FromRgb(105, 64, 40)),
		[1001] = new SolidColorBrush(Color.FromRgb(248, 248, 248)),
		[1] = new SolidColorBrush(Color.FromRgb(242, 243, 243)),
		[208] = new SolidColorBrush(Color.FromRgb(229, 228, 223)),
		[1002] = new SolidColorBrush(Color.FromRgb(205, 205, 205)),
		[194] = new SolidColorBrush(Color.FromRgb(163, 162, 165)),
		[199] = new SolidColorBrush(Color.FromRgb(99, 95, 98)),
		[26] = new SolidColorBrush(Color.FromRgb(27, 42, 53)),
		[1003] = new SolidColorBrush(Color.FromRgb(17, 17, 17))
	};

	public static IReadOnlyDictionary<int, SolidColorBrush> Character => Colours;

	public static IReadOnlyDictionary<int, SolidColorBrush> CharacterOlder { get; }

	public static SolidColorBrush GetColourFromId(int id)
	{
		return Character[id];
	}

	static BrickColors()
	{
		Dictionary<int, SolidColorBrush> dictionary = new Dictionary<int, SolidColorBrush>();
		dictionary[1] = Colours[1];
		dictionary[208] = Colours[208];
		dictionary[194] = Colours[194];
		dictionary[199] = Colours[199];
		dictionary[26] = Colours[26];
		dictionary[21] = Colours[21];
		dictionary[24] = Colours[24];
		dictionary[226] = Colours[226];
		dictionary[23] = Colours[23];
		dictionary[107] = Colours[107];
		dictionary[102] = Colours[102];
		dictionary[11] = Colours[11];
		dictionary[45] = Colours[45];
		dictionary[135] = Colours[135];
		dictionary[106] = Colours[106];
		dictionary[105] = Colours[105];
		dictionary[141] = Colours[141];
		dictionary[28] = Colours[28];
		dictionary[37] = Colours[37];
		dictionary[119] = Colours[119];
		dictionary[29] = Colours[29];
		dictionary[151] = Colours[151];
		dictionary[38] = Colours[38];
		dictionary[192] = Colours[192];
		dictionary[104] = Colours[104];
		dictionary[9] = Colours[9];
		dictionary[101] = Colours[101];
		dictionary[21] = Colours[21];
		dictionary[5] = Colours[5];
		dictionary[153] = Colours[153];
		dictionary[217] = Colours[217];
		dictionary[18] = Colours[18];
		dictionary[125] = Colours[125];
		CharacterOlder = dictionary;
	}
}
