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
	using System.Web.Http.Controllers;

	using VahapYigit.Test.Models;

	/// <summary>
	/// Filter used to set admin access restriction on WebApi operations.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class AdminAuthenticationFilterAttribute : BasicAuthenticationFilterAttribute
	{
		public AdminAuthenticationFilterAttribute(bool isActivated = true)
			: base(isActivated)
		{
		}

		protected override bool OnAuthorizeUser(string identifier, string password, HttpActionContext context)
		{
			if (!base.OnAuthorizeUser(identifier, password, context))
			{
				return false;
			}

			bool withSimpleAdminAuth = ConfigurationManagerHelper.GetValue<bool>("WebApi.WithSimpleAdminAuth");
			if (withSimpleAdminAuth)
			{
				string adminIdentifier = ConfigurationManagerHelper.GetValue<string>("WebApi.AdminIdentifier");
				string adminPassword = ConfigurationManagerHelper.GetValue<string>("WebApi.AdminPassword");

				if (identifier == adminIdentifier && password == adminPassword)
				{
					return true;
				}
			}

			// Authentication from database (the user must be registered in the database and have 'Admin' role).

			var user = new CustomUserValidator().GetUserWithRoles(identifier, password);

            return user != null && user.UserRoleCollection.Select(ur => ur.Role).Any(r => r.CodeRef == Role.CodeRefs.Admin);
		}
	}
}