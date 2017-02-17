// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using System;
	using System.Web.Http.Controllers;

	/// <summary>
	/// Filter used to set user access restriction to WebApi operations.
	/// The users accounts are defined in the database / User table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class CustomAuthenticationFilterAttribute : BasicAuthenticationFilterAttribute
	{
		public CustomAuthenticationFilterAttribute(bool isActivated = true)
			: base(isActivated)
		{
		}

		protected override bool OnAuthorizeUser(string identifier, string password, HttpActionContext context)
		{
			if (!base.OnAuthorizeUser(identifier, password, context))
			{
				return false;
			}

			return new CustomUserValidator().IsValidated(identifier, password);
		}
	}
}