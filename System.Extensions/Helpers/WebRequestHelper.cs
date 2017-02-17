// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;
	using System.Net;
	using System.Text;

	public static class WebRequestHelper
	{
		/// <summary>
		/// Submits a POST request and gets back the response.
		/// </summary>
		/// 
		/// <param name="url">
		/// Service's Url.
		/// </param>
		/// 
		/// <param name="formData">
		/// The form data (ex: key1=value1&amp;key2=value2).
		/// </param>
		/// 
		/// <returns>
		/// The response.
		/// </returns>
		public static string GetResponseFromPostRequest(string url, string formData)
		{
			byte[] data = Encoding.UTF8.GetBytes(formData);

			WebRequest webRequest = WebRequest.CreateHttp(url);

			webRequest.Method = "POST";

			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = data.Length;

			webRequest.Proxy = WebRequest.DefaultWebProxy;
			webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

			Stream dataStream = webRequest.GetRequestStream();
			dataStream.Write(data, 0, data.Length);
			dataStream.Close();

			WebResponse webResponse = webRequest.GetResponse();
			dataStream = webResponse.GetResponseStream();

			StreamReader streamReader = new StreamReader(dataStream);
			string response = streamReader.ReadToEnd();

			streamReader.Close();
			//dataStream.Close(); -> CA2202: Do not dispose objects multiple times - Object 'dataStream' can be disposed more than once in method 'WebRequestHelper.GetResponseFromPostRequest(string, string)'. To avoid generating a System.ObjectDisposedException you should not call Dispose more than one time on an object.
			webResponse.Close();

			return response;
		}

		/// <summary>
		/// Submits a POST request.
		/// </summary>
		/// 
		/// <param name="url">
		/// Service's Url.
		/// </param>
		/// 
		/// <param name="formData">
		/// The form data (ex: key1=value1&amp;key2=value2).
		/// </param>
		public static void PostRequest(string url, string formData)
		{
			byte[] data = Encoding.UTF8.GetBytes(formData);

			WebRequest webRequest = WebRequest.CreateHttp(url);

			webRequest.Method = "POST";

			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = data.Length;

			webRequest.Proxy = WebRequest.DefaultWebProxy;
			webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

			Stream dataStream = webRequest.GetRequestStream();
			dataStream.Write(data, 0, data.Length);
			dataStream.Close();
		}
	}
}
