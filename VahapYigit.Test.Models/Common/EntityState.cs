// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.ComponentModel;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	using VahapYigit.Test.Core;

	/// <summary>
	/// Entity state enumeration
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace)]
	[DefaultValue(EntityState.None)]
	public enum EntityState
	{
		/// <summary>
		/// None - Default value.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		None = 0,

		/// <summary>
		/// ToInsert - New entity (will be create in DB on Save() call).
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		ToInsert = 1,

		/// <summary>
		/// ToUpdate - Updated entity (will be updated in DB on Save() call).
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		ToUpdate = 2,

		/// <summary>
		/// Deleted - Deleted entity.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Deleted = 3
	}
}
