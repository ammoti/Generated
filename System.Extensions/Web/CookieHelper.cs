// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Web
{
	public static class CookieHelper
	{
		/// <summary>
		/// Checks if the cookies are supported by the client's browser.
		/// </summary>
		///
		/// <returns>
		/// True if the client's browser supports cookies; otherwise, false.
		/// </returns>
		public static bool CheckCookieSuport()
		{
			if (HttpContext.Current == null)
			{
				throw new InvalidOperationException("This method is supported in Web context only");
			}

			if (HttpContext.Current.Request.Cookies["CheckCookieSuport"] == null)
			{
				if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.QueryString["cookie"]))
				{
					HttpContext.Current.Response.Cookies["CheckCookieSuport"].Value = "Yes";
					HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString() + "?cookie=created", true);
				}
				else if (HttpContext.Current.Request.QueryString["cookie"].Equals("created"))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Protects the value.
		/// </summary>
		///
		/// <param name="value">
		/// Value to protect.
		/// </param>
		///
		/// <param name="salt">
		/// Additional input to protect the value.
		/// </param>
		///
		/// <returns>
		/// The protected value.
		/// </returns>
		public static string Protect(string cookieValue, string salt = null)
		{
			return MachineKeyHelper.Protect(cookieValue, salt);
		}

		/// <summary>
		/// Unprotects the protected value.
		/// </summary>
		///
		/// <param name="value">
		/// Value to unprotect.
		/// </param>
		///
		/// <param name="salt">
		/// Additional input to unprotect the value.
		/// </param>
		///
		/// <returns>
		/// The clear value.
		/// </returns>
		public static string Unprotect(string protectedValue, string salt = null)
		{
			return MachineKeyHelper.Unprotect(protectedValue, salt);
		}
	}
}
