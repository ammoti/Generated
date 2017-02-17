// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;

	public static class PathHelper
	{
		/// <summary>
		/// Returns a temporary path of the current user.
		/// </summary>
		/// 
		/// <param name="withCreation">
		/// Value indicating whether the temporary path is created.
		/// </param>
		/// 
		/// <returns>
		/// The temporary path folder.
		/// </returns>
		public static string GetTempPath(bool withCreation = false)
		{
			string tempPath = Path.Combine(
				Path.GetTempPath(),
				Path.GetFileNameWithoutExtension(Path.GetTempFileName()));

			if (withCreation)
			{
				if (!Directory.Exists(tempPath))
				{
					Directory.CreateDirectory(tempPath);
				}
			}

			return tempPath;
		}

		/// <summary>
		/// Returns the full file path.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// File path.
		/// </param>
		/// 
		/// <returns>
		/// The full file path.
		/// </returns>
		public static string GetLocalPath(string filePath)
		{
			if (!Path.IsPathRooted(filePath))
			{
				filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
			}

			return Path.GetFullPath(filePath);
		}
	}
}
