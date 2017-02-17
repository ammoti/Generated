// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using VahapYigit.Test.Models;

	public static class ServiceKnownTypeHelper
	{
		/// <summary>
		/// Retrieves all the public EntityBase types from Models.
		/// </summary>
		/// <returns>EntityBase collection.</returns>
		public static IEnumerable<Type> GetEntityTypes()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EntityBase));
			return assembly.GetTypes().Where(t => t.BaseType == typeof(EntityBase) && t.IsPublic).OrderBy(t => t.Name).ToList();
		}
	}
}
