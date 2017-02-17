// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;

	public static class DirectoryHelper
	{
		/// <summary>
		/// Deletes the directory.
		/// </summary>
		/// 
		/// <param name="directoryPath">
		/// Path of the directory to delete. All the content will be deleted too.
		/// </param>
		/// 
		/// <param name="withException">
		/// Value indicating whether an exception is thrown on action error.
		/// </param>
		public static void DeleteDirectory(string directoryPath, bool withException = true)
		{
			try
			{
				if (Directory.Exists(directoryPath))
				{
					foreach (string file in Directory.GetFiles(directoryPath))
					{
						File.SetAttributes(file, FileAttributes.Normal);
						File.Delete(file);
					}

					foreach (string dir in Directory.GetDirectories(directoryPath))
					{
						DirectoryHelper.DeleteDirectory(dir);
					}

					Directory.Delete(directoryPath, false);
				}
			}
			catch
			{
				if (withException) throw;
			}
		}
	}
}
