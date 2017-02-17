// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using System;
	using System.Web;
	using System.Web.Mvc;

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class DisableClientCacheAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.RequestContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			filterContext.RequestContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
			filterContext.RequestContext.HttpContext.Response.Cache.SetNoStore();
			filterContext.RequestContext.HttpContext.Response.AppendHeader("pragma", "no-cache");
		}
	}
}