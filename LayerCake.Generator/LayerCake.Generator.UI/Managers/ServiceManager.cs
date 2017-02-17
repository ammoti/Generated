// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;
	using System.Xml;

	public static class ServiceManager
	{
		#region [ Constants ]

		private static readonly string SERVICE_URL = "http://www.layercake-generator.net/Service.ashx";

		private static readonly string REQUEST = "Action={0}&AppName={1}&AppVersion={2}&ExecutionCount={3}&ProcessCount={4}&Content={5}";

		#endregion

		#region [ Versioning ]

		public static bool CheckUpdate(out string date, out string version)
		{
			date = null; version = null;

			if (AppHelper.AppName == "XDesProc") // DEV
				return false;

			try
			{
				string request = GetRequest("Versioning");
				string reponse = WebRequestHelper.GetResponseFromPostRequest(SERVICE_URL, request);

				XmlDocument xmlFile = new XmlDocument();
				xmlFile.LoadXml(reponse);

				XmlNode rootNode = xmlFile.DocumentElement;

				date = rootNode.SelectSingleNode("Date").InnerText;
				version = rootNode.SelectSingleNode("Version").InnerText;

				if (!string.IsNullOrEmpty(version))
				{
					int iCurrentVersion = Convert.ToInt32(AppHelper.AppVersion.Replace(".", string.Empty));
					int iLatestVersion = Convert.ToInt32(version.Replace(".", string.Empty));

					return iLatestVersion > iCurrentVersion;
				}
			}
			catch
			{
				// Nothing
			}

			return false;
		}

		#endregion

		#region [ ProcessReporting ]

		public static void SubmitProcessAsync(string content)
		{
			if (!PreferenceManager.WithSendProcessReports)
				return;

			if (content == null)
				return;

			AsyncHelper.FireAndForget(() =>
			{
				try
				{
					string request = GetRequest("ProcessReporting", content);
					WebRequestHelper.PostRequest(SERVICE_URL, request);
				}
				catch
				{
					// Nothing
				}
			});
		}

		#endregion

		#region [ ErrorReporting ]

		public static void SubmitExceptionAsync(Exception exception)
		{
			if (!PreferenceManager.WithSendErrorReports)
				return;

			if (exception == null)
				return;

			AsyncHelper.FireAndForget(() =>
			{
				try
				{
					string request = GetRequest("ErrorReporting", exception.ToString());
					WebRequestHelper.PostRequest(SERVICE_URL, request);
				}
				catch
				{
					// Nothing
				}
			});
		}

		public static void SubmitErrorAsync(string error)
		{
			if (!PreferenceManager.WithSendErrorReports)
				return;

			if (error == null)
				return;

			AsyncHelper.FireAndForget(() =>
			{
				try
				{
					string request = GetRequest("ErrorReporting", error);
					WebRequestHelper.PostRequest(SERVICE_URL, request);
				}
				catch
				{
					// Nothing
				}
			});
		}

		#endregion

		#region [ Methods ]

		private static string GetRequest(string action, string content = null)
		{
			return string.Format(REQUEST, action, AppHelper.AppName, AppHelper.AppVersion, PreferenceManager.ExecutionCount, PreferenceManager.ProcessCount, content);
		}

		#endregion
	}
}
