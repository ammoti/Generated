// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Interface for UserContext.
	/// </summary>
	public interface IUserContext : ICloneable
	{
		/// <summary>
		/// Gets or sets the Id of the user.
		/// </summary>
		long Id { get; set; }

		/// <summary>
		/// Gets or sets the Identifier of the user (Username, Email, etc).
		/// </summary>
		string Identifier { get; set; }

		/// <summary>
		/// Gets or sets the Password of the user.
		/// </summary>
		string Password { get; set; }

		/// <summary>
		/// Gets or sets the Culture of the user.
		/// </summary>
		string Culture { get; set; }

		/// <summary>
		/// Gets or sets the context options.
		/// </summary>
		UserContextOptions Options { get; set; }
	}
}
