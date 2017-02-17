// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;
	using System.IO.Compression;

	public static class BitZipHelper
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Microsoft.Usage",
			"CA2202:Do not dispose objects multiple times",
			Justification = "We suppose the Dispose method is correctly implemented and can be called multiple times without throwing an exception - http://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k(CA2202);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5)&rd=true")]
		public static byte[] Zip(byte[] plainData)
		{
			if (plainData == null)
			{
				return null;
			}

			using (var msi = new MemoryStream(plainData))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(mso, CompressionMode.Compress))
				{
					CopyTo(msi, gs);
				}

				return mso.ToArray();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Microsoft.Usage",
			"CA2202:Do not dispose objects multiple times",
			Justification = "We suppose the Dispose method is correctly implemented and can be called multiple times without throwing an exception - http://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k(CA2202);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5)&rd=true")]
		public static byte[] Unzip(byte[] zippedData)
		{
			if (zippedData == null)
			{
				return null;
			}

			using (var msi = new MemoryStream(zippedData))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(msi, CompressionMode.Decompress))
				{
					CopyTo(gs, mso);
				}

				return mso.ToArray();
			}
		}

		private static void CopyTo(Stream source, Stream destination)
		{
			int cnt;
			byte[] bytes = new byte[4096];

			while ((cnt = source.Read(bytes, 0, bytes.Length)) != 0)
			{
				destination.Write(bytes, 0, cnt);
			}
		}
	}
}
