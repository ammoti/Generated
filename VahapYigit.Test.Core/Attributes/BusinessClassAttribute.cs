// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;

	/// <summary>
	/// Attribute used to tag business class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	sealed public class BusinessClassAttribute : Attribute
	{
	}
}
