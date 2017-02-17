// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;
	using System.Text;
	using System.Threading;

	using Microsoft.Win32;

	public static class FileHelper
	{
		/// <summary>
		/// Returns the mime type of the specified file.
		/// </summary>
		/// 
		/// <param name="file">
		/// File path.
		/// </param>
		/// 
		/// <returns>
		/// The mime type of the file path (the return value can be null).
		/// </returns>
		public static string GetMimeType(string file)
		{
			if (string.IsNullOrWhiteSpace(file))
			{
				ThrowException.ThrowArgumentNullException("file");
			}

			string filePath = Path.GetFullPath(file);
			string extension = Path.GetExtension(filePath);

			if (extension == null || extension.Length == 0)
			{
				ThrowException.ThrowArgumentException(
					"file",
					string.Format("The '{0}' file must have an extension", filePath));
			}

			string registryPath = string.Format(@"HKEY_CLASSES_ROOT\{0}", extension);

			string mimeType = null;

			try
			{
				object value = Registry.GetValue(registryPath, "Content Type", null);

				if (value != null)
				{
					mimeType = value.ToString();
				}
			}
			catch
			{
			}

			return mimeType;
		}

		/// <summary>
		/// Tries to delete the file without throwing exception.
		/// </summary>
		/// 
		/// <param name="path">
		/// Path of the file.
		/// </param>
		/// 
		/// <param name="retryCount">
		/// Retry count.
		/// </param>
		/// 
		/// <returns>
		/// True if the file has been deleted; otherwise, false.
		/// </returns>
		public static bool TryDelete(string path, int retryCount = 3)
		{
			while (retryCount > 0)
			{
				try
				{
					if (File.Exists(path))
					{
						File.Delete(path);
					}

					return true;
				}
				catch
				{
					retryCount--;
					Thread.Sleep(200);
				}
			}

			return retryCount > 0;
		}

		/// <summary>
		/// Tries to move the file without throwing exception.
		/// </summary>
		/// 
		/// <param name="sourceFileName">
		/// The source path.
		/// </param>
		/// 
		/// <param name="destFileName">
		/// The destination path.
		/// </param>
		/// 
		/// <param name="retryCount">
		/// Retry count.
		/// </param>
		/// 
		/// <returns>
		/// True if the file has been moved; otherwise, false.
		/// </returns>
		public static bool TryMove(string sourceFileName, string destFileName, int retryCount = 3)
		{
			while (retryCount > 0)
			{
				try
				{
					if (File.Exists(sourceFileName))
					{
						if (File.Exists(destFileName))
						{
							FileHelper.TryDelete(destFileName);
						}

						File.Move(sourceFileName, destFileName);
					}

					return true;
				}
				catch
				{
					retryCount--;
					Thread.Sleep(200);
				}
			}

			return retryCount > 0;
		}

		/// <summary>
		/// Tries to copy the file without throwing exception.
		/// </summary>
		/// 
		/// <param name="sourceFileName">
		/// The source path.
		/// </param>
		/// 
		/// <param name="destFileName">
		/// The destination path.
		/// </param>
		/// 
		/// <param name="withOverwrite">
		/// Value indicating whether the destFileName can be overwrited. Default value is false.
		/// </param>
		/// 
		/// <param name="retryCount">
		/// Retry count.
		/// </param>
		/// 
		/// <returns>
		/// True if the file has been copied; otherwise, false.
		/// </returns>
		public static bool TryCopy(string sourceFileName, string destFileName, bool withOverwrite = false, int retryCount = 3)
		{
			while (retryCount > 0)
			{
				try
				{
					if (File.Exists(sourceFileName))
					{
						if (File.Exists(destFileName))
						{
							if (withOverwrite)
							{
								File.Copy(sourceFileName, destFileName, withOverwrite);
								return true;
							}
							else
							{
								return false;
							}
						}
						else
						{
							File.Copy(sourceFileName, destFileName, withOverwrite);
							return true;
						}
					}
					else
					{
						return false;
					}
				}
				catch
				{
					retryCount--;
					Thread.Sleep(200);
				}
			}

			return retryCount > 0;
		}

		/// <summary>
		/// Opens a file, reads all lines of the file, and then closes the file.
		/// If the file does not exist or if an error occurred, string.Empty is returned.
		/// </summary>
		/// 
		/// <param name="path">
		/// The file to open for reading.
		/// </param>
		/// 
		/// <returns>
		/// A string containing all lines of the file.
		/// </returns>
		public static string TryReadAllText(string path)
		{
			if (!File.Exists(path))
			{
				return string.Empty;
			}

			try
			{
				return File.ReadAllText(path);
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
		/// If the file does not exist or if an error occurred, string.Empty is returned.
		/// </summary>
		/// 
		/// <param name="path">
		/// The file to open for reading.
		/// </param>
		/// 
		/// <param name="encoding">
		/// The encoding applied to the contents of the file.
		/// </param>
		/// 
		/// <returns>
		/// A string containing all lines of the file.
		/// </returns>
		public static string TryReadAllText(string path, Encoding encoding)
		{
			if (!File.Exists(path))
			{
				return string.Empty;
			}

			try
			{
				return File.ReadAllText(path, encoding);
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
