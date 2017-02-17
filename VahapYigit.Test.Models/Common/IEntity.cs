// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	/// <summary>
	/// Entity interface.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// Gets or sets the unique ID.
		/// </summary>
		long Id { get; set; }

		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		EntityState State { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the entity has been loaded from the DB (if Id > 0).
		/// </summary>
		bool IsInDb { get; }
	}
}
