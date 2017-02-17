// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Runtime.Serialization;

	using VahapYigit.Test.Core;

	/// <summary>
	/// Transforms an EntityBase class to an IOwnershipEntity one.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Type of the entity.
	/// </typeparam>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	public abstract class OwnershipEntityDecorator<T> : EntityBase, IOwnershipEntity where T : EntityBase
	{
		#region [ IOwnershipEntity Implementation ]

		/// <summary>
		/// Gets the date of creation.
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets the creator identifier.
		/// </summary>
		public long CreatedBy { get; set; }

		/// <summary>
		/// Gets the date of the last modification.
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Gets the modifier identifier.
		/// </summary>
		public long ModifiedBy { get; set; }

		/// <summary>
		/// Gets the owner of the entity (generally the same as creation owner).
		/// </summary>
		public long Owner { get; set; }

		/// <summary>
		/// Gets the value indicating whether the entity is locked.
		/// </summary>
		public bool IsLocked { get; set; }

		#endregion
	}
}
