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
	/// AccentSensitivity enumeration for textual search.
	/// Based on the database collation (French_CI_AI)
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.AccentSensitivity")]
	[DefaultValue(AccentSensitivity.Without)]
	public enum AccentSensitivity
	{
		/// <summary>
		/// Without case sensitivity.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		Without = 0,

		/// <summary>
		/// With case sensitivity.
		/// </summary>
		[XmlEnum()]
		[EnumMember()]
		With = 1
	}
}
