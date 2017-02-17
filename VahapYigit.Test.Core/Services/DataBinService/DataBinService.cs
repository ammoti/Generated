// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Data;
	using System.IO;

	public class DataBinService : IDataBinService
	{
		#region [ IDataBinService Implementation ]

		/// <summary>
		/// Generate a binary file.
		/// </summary>
		/// 
		/// <param name="data">
		/// Data to store.
		/// </param>
		/// 
		/// <param name="filePath">
		/// Path of the binary file to create.
		/// </param>
		/// 
		/// <param name="withCompression">
		/// Value indicating whether the GZIP compression is used.
		/// </param>
		/// 
		/// <returns>
		/// The FileInfo instance.
		/// </returns>
		public FileInfo GenerateBinFile(string filePath, object data, bool withCompression = false)
		{
			return SerializerHelper.ToFile(data, filePath, withCompression);
		}

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
		/// A DataTable object.
		/// </returns>
		public object GetDataFromBinFile(string filePath, bool isCompressed = false)
		{
			if (!File.Exists(filePath))
			{
				ThrowException.ThrowFileNotFoundException(
					string.Format("Cannot access to the file {0}", filePath),
					filePath);
			}

			object obj = null;

			try
			{
				obj = SerializerHelper.ToObject<object>(filePath, isCompressed);
			}
			catch (Exception x)
			{
				string message = string.Format("Cannot deserialize the file {0}.", Path.GetFileName(filePath));

				message = string.Concat(message, (isCompressed) ? " Perhaps it is not compressed?" : " Perhaps it is compressed?");
				x.GetMessages().ForEach((error) => { message = string.Concat(message, Environment.NewLine, Environment.NewLine, error); });

				ThrowException.Throw(message, x);
			}

			return obj;
		}

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
		public TCollection<T> GetDataFromBinFile<T>(string filePath, bool isCompressed = false)
			where T : IDataRowMapping, new()
		{
			TCollection<T> collection = new TCollection<T>();

			object data = this.GetDataFromBinFile(filePath, isCompressed);

			return data as TCollection<T>;






			//DataTable data =null;//= this.GetDataFromBinFile(filePath, isCompressed);
			//TCollection<T> collection = new TCollection<T>();

			//foreach (DataRow row in data.Rows)
			//{
			//	T entity = new T();
			//	entity.DeepMap(row);

			//	collection.Add(entity);
			//}

			//return collection;
		}

		#endregion
	}
}
