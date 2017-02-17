// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using System;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Runtime.Caching;
	using System.Threading;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class WebApiOutputCacheAttribute : ActionFilterAttribute
	{
		#region [ Members ]

		private static readonly ObjectCache WebApiCache = MemoryCache.Default;

		private readonly int _serverTimesSpan;
		private readonly int _clientTimeSpan;

		private readonly bool _anonymousOnly; // Caching for anonymous users only?

		private string _bodyCacheKey = null;
		private string _contentTypeCacheKey = null;

		#endregion

		#region [ Constructors ]

		public WebApiOutputCacheAttribute(int serverCacheDelayInSecond, int clientCacheDelayInSecond, bool anonymousOnly = false)
		{
			_serverTimesSpan = serverCacheDelayInSecond;
			_clientTimeSpan = clientCacheDelayInSecond;

			_anonymousOnly = anonymousOnly;
		}

		#endregion

		#region [ ActionFilterAttribute Members ]

		public override void OnActionExecuting(HttpActionContext context)
		{
			bool bFromCache = false;

			if (context != null)
			{
				if (this.IsCacheable(context))
				{
					_bodyCacheKey = string.Join(":", context.Request.RequestUri.PathAndQuery, context.Request.Headers.Accept.FirstOrDefault().ToString());
					_contentTypeCacheKey = string.Concat(_bodyCacheKey, ":response-ct");

					if (WebApiCache.Contains(_bodyCacheKey))
					{
						var data = (string)WebApiCache.Get(_bodyCacheKey);
						if (data != null)
						{
							bFromCache = true;

							context.Response = context.Request.CreateResponse();
							context.Response.Content = new StringContent(data);

							var contentType = (MediaTypeHeaderValue)WebApiCache.Get(_contentTypeCacheKey);
							if (contentType == null)
							{
								contentType = new MediaTypeHeaderValue(_bodyCacheKey.Split(':')[1]);
							}

							context.Response.Content.Headers.ContentType = contentType;
							context.Response.Headers.CacheControl = this.SetClientCache();
						}
					}
				}
			}

			LoggerServiceHelper.Current.WriteLine(this,LogStatusEnum.Debug, "[{0}] {1} (-> {2})",
				context.GetClientIp(), context.Request.RequestUri.PathAndQuery, bFromCache ? "cache" : "database");
		}

		public override void OnActionExecuted(HttpActionExecutedContext context)
		{
			if (!WebApiCache.Contains(_bodyCacheKey))
			{
				var body = context.Response.Content.ReadAsStringAsync().Result;

				WebApiCache.Add(_bodyCacheKey, body, DateTime.Now.AddSeconds(_serverTimesSpan));
				WebApiCache.Add(_contentTypeCacheKey, context.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_serverTimesSpan));
			}

			if (this.IsCacheable(context.ActionContext))
			{
				context.ActionContext.Response.Headers.CacheControl = this.SetClientCache();
			}
		}

		#endregion

		#region [ Private Methods ]

		private bool IsCacheable(HttpActionContext context)
		{
			if (_serverTimesSpan > 0 && _clientTimeSpan > 0)
			{
				if (_anonymousOnly)
				{
					if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
					{
						return false;
					}
				}

				if (context.Request.Method == HttpMethod.Get)
				{
					return true;
				}
			}

			return false;
		}

		private CacheControlHeaderValue SetClientCache()
		{
			return new CacheControlHeaderValue
			{
				MaxAge = TimeSpan.FromSeconds(_clientTimeSpan),
				MustRevalidate = true
			};
		}

		#endregion
	}
}
