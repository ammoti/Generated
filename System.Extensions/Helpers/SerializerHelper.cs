// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel;
	using System.IO;
	using System.IO.Compression;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Web.Script.Serialization;
	using System.Xml.Serialization;

	[DefaultValue(SerializerType.Xml)]
	public enum SerializerType
	{
		/// <summary>
		/// Does not support circular references.
		/// </summary>
		Xml,

		/// <summary>
		/// Supports circular references.
		/// </summary>
		DataContract,

		Json
	}

	public static class SerializerHelper
	{
		#region [ Members ]

		private readonly static JavaScriptSerializer _jsonSerializer = new JavaScriptSerializer();

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Serialize an object to a Xml representation using SerializerType.Xml (DOES NOT SUPPORT CIRCULAR REFERENCES) or SerializerType.DataContract (supports circular references).
		/// </summary>
		/// 
		/// <param name="serializerType">
		/// Serializer to use.
		/// </param>
		/// 
		/// <param name="source">
		/// Object to serialize.
		/// </param>
		/// 
		/// <returns>
		/// The Xml representation.
		/// </returns>
		public static string ToXml(SerializerType serializerType, object source)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (serializerType != SerializerType.Xml &&
				serializerType != SerializerType.DataContract)
			{
				ThrowException.ThrowArgumentException(string.Format("The '{0}' serializer is not supported in this context", serializerType));
			}

			if (serializerType == SerializerType.DataContract)
			{
				var mStream = new MemoryStream(); // no using -> CA2202 Do not dispose objects multiple times Object 'mStream' can be disposed more than once in method 'SerializerHelper.ToXml(XmlSerializerType, object)'.
				
				using (var reader = new StreamReader(mStream))
				{
					var dcSerializer = new DataContractSerializer(source.GetType());
					dcSerializer.WriteObject(mStream, source);
					mStream.Position = 0;

					return reader.ReadToEnd();
				}
			}
			else
			{
				var xmlSerializer = new XmlSerializer(source.GetType());
				using (var writer = new StringWriter())
				{
					// NOTE: if you get "A circular reference was detected while serializing an object of type 'Entity'"
					//       then use XmlSerializerType.DataContract instead.

					xmlSerializer.Serialize(writer, source);
					return writer.ToString();
				}
			}
		}

		/// <summary>
		/// Serialize an object to a Json representation.
		/// </summary>
		/// 
		/// <param name="source">
		/// Object to serialize.
		/// </param>
		/// 
		/// <returns>
		/// The Json representation.
		/// </returns>
		public static string ToJson(object source)
		{
			return _jsonSerializer.Serialize(source);
		}

		/// <summary>
		/// Deserialize a xml representation to an object using XmlSerializerType.Xml or XmlSerializerType.DataContract.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the object returned.
		/// </typeparam>
		/// 
		/// <param name="serializerType">
		/// Serializer to use.
		/// </param>
		/// 
		/// <param name="content">
		/// String representation to deserialize.
		/// </param>
		/// 
		/// <returns>
		/// The object.
		/// </returns>
		public static T ToObject<T>(SerializerType serializerType, string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				ThrowException.ThrowArgumentNullException("content");
			}

			if (serializerType == SerializerType.DataContract)
			{
				using (var stream = new MemoryStream())
				{
					byte[] data = Encoding.UTF8.GetBytes(content);
					stream.Write(data, 0, data.Length);
					stream.Position = 0;

					var dcSerializer = new DataContractSerializer(typeof(T));
					return (T)dcSerializer.ReadObject(stream);
				}
			}
			else if (serializerType == SerializerType.Json)
			{
				return _jsonSerializer.Deserialize<T>(content.Trim());
			}
			else
			{
				var xmlSerializer = new XmlSerializer(typeof(T));
				using (var reader = new StringReader(content))
				{
					return (T)xmlSerializer.Deserialize(reader);
				}
			}
		}

		/// <summary>
		/// Serialize an object to a file with specific SerializerType.
		/// </summary>
		/// 
		/// <param name="source">
		/// Object to serialize.
		/// </param>
		/// 
		/// <param name="filePath">
		/// Output file.
		/// </param>
		/// 
		/// <param name="serializerType">
		/// The type of the Serializer to use.
		/// </param>
		/// 
		/// <returns>
		/// The FileInfo instance.
		/// </returns>
		public static FileInfo ToFile(object source, string filePath, SerializerType serializerType)
		{
			string content = (serializerType == SerializerType.Json) ?
				ToJson(source) :
				ToXml(serializerType, source);

			File.WriteAllText(filePath, content);

			return new FileInfo(filePath);
		}

		/// <summary>
		/// Deserialize a file to an object.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the output object.
		/// </typeparam>
		/// 
		/// <param name="filePath">
		/// File to deserialize.
		/// </param>
		/// 
		/// <param name="serializerType">
		/// The type of the Serializer to use.
		/// </param>
		/// 
		/// <returns>
		/// The object.
		/// </returns>
		public static T ToObject<T>(string filePath, SerializerType serializerType) where T : class, new()
		{
			string content = File.ReadAllText(filePath);

			return ToObject<T>(serializerType, content);
		}

		/// <summary>
		/// Serialize an object to a file with optional GZIP compression.
		/// </summary>
		/// 
		/// <param name="source">
		/// Object to serialize.
		/// </param>
		/// 
		/// <param name="filePath">
		/// Output file.
		/// </param>
		/// 
		/// <param name="withCompression">
		/// Value indicating whether the GZIP compression is used.
		/// </param>
		/// 
		/// <returns>
		/// The FileInfo instance.
		/// </returns>
		public static FileInfo ToFile(object source, string filePath, bool withCompression = false)
		{
			Stream stream = null;
			Stream gzipStream = null;

			IFormatter formatter = new BinaryFormatter();

			try
			{
				stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

				if (withCompression)
				{
					gzipStream = new GZipStream(stream, CompressionLevel.Optimal);
					formatter.Serialize(gzipStream, source);
				}
				else
				{
					formatter.Serialize(stream, source);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				// CA2202 Do not dispose objects multiple times
				// Object 'stream' can be disposed more than once in method 'BinSerializerHelper.Deserialize<T>(string, bool)'.
				// To avoid generating a System.ObjectDisposedException you should not call Dispose more than one time on an object.

				if (gzipStream != null) gzipStream.Close();
				else if (stream != null) stream.Close();
			}

			return new FileInfo(filePath);
		}

		/// <summary>
		/// Deserialize a file to an object.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the output object.
		/// </typeparam>
		/// 
		/// <param name="filePath">
		/// File to deserialize.
		/// </param>
		/// 
		/// <param name="isCompressed">
		/// Value indicating whether the file is compressed with GZIP.
		/// </param>
		/// 
		/// <returns>
		/// The object.
		/// </returns>
		public static T ToObject<T>(string filePath, bool isCompressed = false) where T : class, new()
		{
			Stream stream = null;
			Stream gzipStream = null;

			IFormatter formatter = new BinaryFormatter();

			try
			{
				stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

				if (isCompressed)
				{
					gzipStream = new GZipStream(stream, CompressionMode.Decompress);
					return formatter.Deserialize(gzipStream) as T;
				}
				else
				{
					return formatter.Deserialize(stream) as T;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				// CA2202 Do not dispose objects multiple times
				// Object 'stream' can be disposed more than once in method 'BinSerializerHelper.Deserialize<T>(string, bool)'.
				// To avoid generating a System.ObjectDisposedException you should not call Dispose more than one time on an object.

				if (gzipStream != null) gzipStream.Close();
				else if (stream != null) stream.Close();
			}
		}

		#endregion
	}
}