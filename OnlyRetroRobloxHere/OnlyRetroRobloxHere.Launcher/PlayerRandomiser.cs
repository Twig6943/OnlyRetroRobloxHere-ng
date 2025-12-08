using System;

namespace OnlyRetroRobloxHere.Launcher;

internal class PlayerRandomiser
{
	private static string[] _UsernameParts = new string[135]
	{
		"elephant", "roblox", "xxx", "gamer", "xbox", "fan", "boy", "old", "ps2", "ps3",
		"playstation", "bling", "hacker", "haxxer", "hax", "z0mg", "mine", "craft", "broke", "minecraft",
		"blockland", "diamond", "mlg", "guy", "girl", "dude", "the", "guest", "awesome", "knight",
		"fighter", "rapper", "rich", "beautiful", "bacon", "waffle", "house", "walls", "baller", "ye",
		"html", "ancient", "team", "fortress", "counter", "strike", "ricochet", "half", "life", "halflife3",
		"aim", "aol", "skype", "official", "real", "twitter", "block", "land", "lego", "cyan",
		"blue", "red", "voilet", "purple", "orange", "orangebox", "box", "pink", "green", "violent",
		"bully", "xtreme", "extreme", "epic", "sword", "skater", "soccer", "football", "skate", "hockey",
		"america", "europe", "hotline", "911", "call", "meh", "plz", "poor", "donate", "please",
		"thx", "buy", "me", "coffee", "and", "cookie", "yay", "fancy", "super", "male",
		"female", "robot", "bot", "binary", "executable", "dot", "exe", "injector", "love", "channel",
		"beep", "boop", "cole", "fabulous", "late", "incredible", "unexpected", "expected", "hax0r", "friend",
		"random", "generator", "factory", "worker", "lol", "lmao", "cute", "dog", "cat", "bunny",
		"puppy", "famous", "wealthy", "unimaginable", "popular"
	};

	public static int GenerateId()
	{
		return new Random().Next(1, 10000000);
	}

	public static string GenerateUsername()
	{
		Random random = new Random();
		string obj = _UsernameParts[random.Next(_UsernameParts.Length)];
		string text = _UsernameParts[random.Next(_UsernameParts.Length)];
		string text2 = random.Next(0, 100).ToString();
		return obj + text + text2;
	}
}
