// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System;
	using System.IdentityModel.Selectors;
	using System.IdentityModel.Tokens;
	using System.Linq;

	using VahapYigit.Test.Contracts;
	using VahapYigit.Test.Core;
	using VahapYigit.Test.Models;
	using VahapYigit.Test.Services;

	/// <summary>
	/// UserNamePasswordValidator class.
	/// </summary>
	public class CustomUserValidator : UserNamePasswordValidator
	{
		public override void Validate(string identifier, string password)
		{
			if (!this.IsValidated(identifier, password))
			{
				throw new SecurityTokenException("Wrong username and/or password");
			}
		}

		/// <summary>
		/// Determines whether the user is registered.
		/// </summary>
		/// 
		/// <param name="identifier">
		/// The user's identifier.
		/// </param>
		/// 
		/// <param name="password">
		/// The user's password.
		/// </param>
		/// 
		/// <returns>
		/// True if the user is registered, otherwise, false.
		/// </returns>
		public bool IsValidated(string identifier, string password)
		{
			if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
			{
				return false;
			}

			bool isValidated = false;

			// Custom code above...
			// ----------------------

			isValidated = this.GetUserWithRoles(identifier, password) != null;

			// ----------------------

			return isValidated;
		}

		/// <summary>
		/// Returns the User instance with Roles if the user is registered; otherwise, null.
		/// </summary>
		/// 
		/// <param name="identifier">
		/// The user's identifier.
		/// </param>
		/// 
		/// <param name="password">
		/// The user's password.
		/// </param>
		/// 
		/// <returns>
		/// The User instance with Roles if the user is registered; otherwise, null.
		/// </returns>
		public User GetUserWithRoles(string identifier, string password)
		{
			if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
			{
				return null;
			}

			User user;

			bool isValidated = ServiceContext.AuthenticationService.IsRegistered(
				new ClientContext(identifier, password),
				out user);

			return isValidated ? user : null;
		}
	}
}