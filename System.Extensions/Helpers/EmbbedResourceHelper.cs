// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;
	using System.Reflection;

	public static class EmbbedResourceHelper
	{
		public static Stream GetEmbeddedStream(string assemblyFullName, string assemblyName, string file)
		{
			Assembly assembly = Assembly.Load(assemblyFullName);

			Stream stream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", assemblyName, file));
			if (stream == null)
			{
				ThrowException.Throw(
					@"Could not locate embedded resource '{0}' in assembly '{1}'",
					file, assemblyName);
			}

			return stream;
		}

		public static string ExtractEmbeddedFile(string assemblyFullName, string assemblyName, string resourceFile, string fileFullPath)
		{
			string filePath =
				Path.GetFullPath(Path.Combine(Path.GetTempPath(), fileFullPath));

			if (!File.Exists(filePath))
			{
				Stream stream = EmbbedResourceHelper.GetEmbeddedStream(
					assemblyFullName, assemblyName, resourceFile);

				using (var fstream = new FileStream(filePath, FileMode.Create))
				{
					int chunkLength = 0;
					byte[] buffer = new byte[32768];

					while ((chunkLength = stream.Read(buffer, 0, buffer.Length)) > 0)
					{
						fstream.Write(buffer, 0, chunkLength);
					}
				}
			}

			return filePath;
		}
	}
}
