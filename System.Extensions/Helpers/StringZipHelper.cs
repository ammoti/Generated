// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Text;

	public static class StringZipHelper
	{
		public static byte[] Zip(string source)
		{
			return BitZipHelper.Zip(Encoding.UTF8.GetBytes(source));
		}

		public static string Unzip(byte[] bytes)
		{
			return Encoding.UTF8.GetString(BitZipHelper.Unzip(bytes));
		}
	}
}
