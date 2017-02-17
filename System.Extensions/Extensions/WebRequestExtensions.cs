// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Net
{
	using System.Threading.Tasks;

	public static class WebRequestExtensions
	{
		public static Task<WebResponse> GetReponseAsync(this WebRequest webRequest)
		{
			#region [ Usage ]

			// WebRequest webRequest = WebRequest.Create("http://www.google.com");
			// webRequest.GetReponseAsync().ContinueWith(t =>
			// {
			//    using (var streamReader = new StreamReader(t.Result.GetResponseStream()))
			//    {
			//        string line = streamReader.ReadLine();
			//        // ...
			//    }
			// }, TaskScheduler.FromCurrentSynchronizationContext());

			#endregion

			return Task.Factory.FromAsync<WebResponse>(
				webRequest.BeginGetResponse, webRequest.EndGetResponse, null);
		}
	}
}
