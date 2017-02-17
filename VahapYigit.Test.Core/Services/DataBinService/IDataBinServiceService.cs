// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.IO;

	/// <summary>
	/// DataBinService interface.
	/// </summary>
	public interface IDataBinService
	{
		/// <summary>
		/// Generate a binary file.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// Path of the binary file to create.
		/// </param>
		/// 
		/// <param name="data">
		/// Data to store.
		/// </param>
		/// 
		/// <param name="withCompression">
		/// Value indicating whether the GZIP compression is used.
		/// </param>
		/// 
		/// <returns>
		/// The FileInfo instance.
		/// </returns>
		FileInfo GenerateBinFile(string filePath, object data, bool withCompression = false);

		/// <summary>
		/// Gets data from a binary file.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// Path of the binary file to read.
		/// </param>
		/// 
		/// <param name="isCompressed">
		/// Value indicating whether the binary file is compressed.
		/// </param>
		/// 
		/// <returns>
		/// The data.
		/// </returns>
		object GetDataFromBinFile(string filePath, bool isCompressed = false);

		/// <summary>
		/// Gets data from a binary file.
		/// </summary>
		/// 
		/// <param name="filePath">
		/// Path of the binary file to read.
		/// </param>
		/// 
		/// <param name="isCompressed">
		/// Value indicating whether the binary file is compressed.
		/// </param>
		/// 
		/// <returns>
		/// A collection of entities.
		/// </returns>
		TCollection<T> GetDataFromBinFile<T>(string filePath, bool isCompressed = false) where T : IDataRowMapping, new();
	}
}
