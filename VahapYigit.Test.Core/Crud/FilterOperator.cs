// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.ComponentModel;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	/// <summary>
	/// Filter operator enumeration.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.FilterOperator")]
	[DefaultValue(FilterOperator.Equals)]
	public enum FilterOperator
	{
		/// <summary>
		/// Equals.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Equals = 0,

		/// <summary>
		/// Different.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Different = 1,

		/// <summary>
		/// Greater (> value).
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Greater = 2,

		/// <summary>
		/// GreaterOrEquals.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		GreaterOrEquals = 3,

		/// <summary>
		/// Less.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Less = 4,

		/// <summary>
		/// LessOrEquals.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		LessOrEquals = 5,

		/// <summary>
		/// Between.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Between = 6,

		/// <summary>
		/// Like.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Like = 7,

		/// <summary>
		/// NotLike.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		NotLike = 8,

		/// <summary>
		/// In (IN (value1, value2)).
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		In = 9,

		/// <summary>
		/// NotIn (NOT IN (value1, value2)).
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		NotIn = 10,

		/// <summary>
		/// StartsWith.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		StartsWith = 11,

		/// <summary>
		/// EndsWith.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		EndsWith = 12,

		/// <summary>
		/// IsNull.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		IsNull = 13,

		/// <summary>
		/// IsNotNull.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		IsNotNull = 14,

		/// <summary>
		/// IsNullOrEmpty.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		IsNullOrEmpty = 15,

		/// <summary>
		/// Contains. Fast Search Text - report to the MSDN to know how to use SQL Server CONTAINS instruction.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Contains = 16
	}
}
