// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Security.Cryptography;
	using System.Text;

	public static class HashHelper
	{
		public static string EncodeWithMD5(string value)
		{
			using (var provider = MD5.Create())
			{
				return Encode(provider, value);
			}
		}

		public static string EncodeWithSha1(string value)
		{
			using (var provider = SHA1.Create())
			{
				return Encode(provider, value);
			}
		}

		public static string EncodeWithSHA256(string value)
		{
			using (var provider = SHA256.Create())
			{
				return Encode(provider, value);
			}
		}

		public static string Encode(HashAlgorithm hashAlgorithm, string value)
		{
			var encoder = new ASCIIEncoding();
			var buffer = encoder.GetBytes(value ?? string.Empty);

			return BitConverter.ToString(hashAlgorithm.ComputeHash(buffer)).Replace("-", "");
		}
	}
}
