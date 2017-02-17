// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System;
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;

	// Parser Error: Server Error in '/' Application
	// Could not load type '[Namespace].Global
	// Close ALL Visual Studio instances and reopen the solution

	public partial class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
		}

		protected void Session_Start(object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			this.TraceException();
		}

		protected void Session_End(object sender, EventArgs e)
		{
		}

		protected void Application_End(object sender, EventArgs e)
		{
		}

		private void TraceException()
		{
			if (HttpContext.Current != null)
			{
				string page = null;
				string file = null;

				if (HttpContext.Current.Request != null)
				{
					if (HttpContext.Current.Request.UrlReferrer != null)
					{
						page = HttpContext.Current.Request.UrlReferrer.ToString();
					}

					if (HttpContext.Current.Request.Url != null)
					{
						file = HttpContext.Current.Request.Url.ToString();
					}
				}

				LoggerServiceHelper.Current.WriteLine("Global::Application_Error()",
					LogStatusEnum.Error,
					"Exception in Page = {0}, File = {1} -> {2}",
					page,
					file,
					HttpContext.Current.Error);
			}
		}
	}
}