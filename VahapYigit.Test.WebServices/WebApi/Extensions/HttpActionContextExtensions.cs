// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Web.Http.Controllers
{
	public static class HttpActionContextExtensions
	{
		public static string GetClientIp(this HttpActionContext context)
		{
			if (context != null && context.Request != null)
			{
				if (context.Request.Properties.ContainsKey("MS_HttpContext"))
				{
					return ((HttpContextWrapper)context.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
				}
			}

			return "UNKNOWN-IP";
		}
	}
}
