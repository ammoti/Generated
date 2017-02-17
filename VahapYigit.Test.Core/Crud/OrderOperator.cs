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
	/// OrberBy operator enumeration.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.OrderOperator")]
	[DefaultValue(OrderOperator.Asc)]
	public enum OrderOperator
	{
		/// <summary>
		/// Ascendant.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Asc = 0,

		/// <summary>
		/// Descendant.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Desc = 1
	}
}
