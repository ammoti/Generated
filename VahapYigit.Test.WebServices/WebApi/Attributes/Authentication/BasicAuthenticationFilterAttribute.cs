// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Security.Principal;
	using System.Text;
	using System.Threading;
	using System.Web;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;

	/// <summary>
	/// Provides Basic authentifaction (identifier/password).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class BasicAuthenticationFilterAttribute : AuthorizationFilterAttribute
	{
		private bool _isActivated = true;

		public BasicAuthenticationFilterAttribute(bool isActivated = true)
		{
			_isActivated = isActivated;
		}

		public override void OnAuthorization(HttpActionContext context)
		{
			if (_isActivated)
			{
				var identity = this.ParseAuthorizationHeader(context);
				if (identity == null)
				{
					this.Challenge(context);
					return;
				}

				if (!this.OnAuthorizeUser(identity.Name, identity.Password, context))
				{
					this.Challenge(context);
					return;
				}

				var principal = new GenericPrincipal(identity, null);

				Thread.CurrentPrincipal = principal;

				if (HttpContext.Current != null)
				{
					HttpContext.Current.User = principal;
				}

				base.OnAuthorization(context);
			}
		}

		protected virtual bool OnAuthorizeUser(string identifier, string password, HttpActionContext context)
		{
			if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
			{
				return false;
			}

			return true;
		}

		protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext context)
		{
			string authHeader = null;

			var auth = context.Request.Headers.Authorization;

			if (auth != null && auth.Scheme.Equals("Basic"))
			{
				authHeader = auth.Parameter;
			}

			if (string.IsNullOrEmpty(authHeader))
			{
				return null;
			}

			authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

			var tokens = authHeader.Split(':');
			if (tokens.Length < 2)
			{
				return null;
			}

			return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
		}

		private void Challenge(HttpActionContext context)
		{
			context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized);
			context.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", context.Request.RequestUri.DnsSafeHost));
		}
	}

	public class BasicAuthenticationIdentity : GenericIdentity
	{
		public BasicAuthenticationIdentity(string identifier, string password)
			: base(identifier, "Basic")
		{
			this.Password = password;
		}

		public string Password { get; set; }
	}
}