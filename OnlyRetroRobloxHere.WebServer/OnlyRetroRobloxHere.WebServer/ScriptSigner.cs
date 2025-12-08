using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.WebServer.Enums;

namespace OnlyRetroRobloxHere.WebServer;

internal static class ScriptSigner
{
	private static RSA _RSA;

	private static string GetSignatureFormat()
	{
		return Config.Instance.Client.Signature switch
		{
			SignatureType.None => string.Empty, 
			SignatureType.Legacy => "%{0}%", 
			SignatureType.RbxSig => "--rbxsig%{0}%", 
			_ => throw new Exception($"Unhandled signature type {Config.Instance.Client.Signature}"), 
		};
	}

	private static string GetPreSignScriptFormat(bool includeAssetId)
	{
		if (!includeAssetId)
		{
			return "\r\n{0}";
		}
		return Config.Instance.Client.Signature switch
		{
			SignatureType.None => string.Empty, 
			SignatureType.Legacy => "%{1}%\r\n{0}", 
			SignatureType.RbxSig => "\r\n--rbxassetid%{1}%\r\n{0}", 
			_ => throw new Exception($"Unhandled signature type {Config.Instance.Client.Signature}"), 
		};
	}

	public static string Sign(string script, ulong assetId = 0uL)
	{
		if (Config.Instance.Client.Signature == SignatureType.None)
		{
			return script;
		}
		string signatureFormat = GetSignatureFormat();
		string preSignScriptFormat = GetPreSignScriptFormat(assetId != 0);
		script = string.Format(preSignScriptFormat, script, assetId);
		byte[] inArray = _RSA.SignData(Encoding.Default.GetBytes(script), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
		return string.Format(signatureFormat, Convert.ToBase64String(inArray)) + script;
	}

	static ScriptSigner()
	{
		string path = Path.Combine(PathHelper.Data, "PrivateKey.pem");
		string text = File.ReadAllText(path);
		_RSA = RSA.Create();
		_RSA.ImportFromPem(text);
	}
}
