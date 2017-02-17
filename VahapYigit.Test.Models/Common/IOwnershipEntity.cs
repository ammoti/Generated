// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;

	/// <summary>
	/// IOwnershipEntity interface (defines ownership entities).
	/// </summary>
	public interface IOwnershipEntity
	{
		/// <summary>
		/// Gets the date of creation.
		/// </summary>
		DateTime CreatedOn { get; }

		/// <summary>
		/// Gets the creator identifier.
		/// </summary>
		long CreatedBy { get; }

		/// <summary>
		/// Gets the date of the last modification.
		/// </summary>
		DateTime ModifiedOn { get; }

		/// <summary>
		/// Gets the modifier identifier.
		/// </summary>
		long ModifiedBy { get; }

		/// <summary>
		/// Gets the owner of the entity (generally the same as creation owner).
		/// </summary>
		long Owner { get; }

		/// <summary>
		/// Gets the value indicating whether the entity is locked.
		/// </summary>
		bool IsLocked { get; }
	}
}
