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
	/// EntityNotFoundException class.
	/// </summary>
	[Serializable]
	public class EntityNotFoundException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="entityName">
		/// Entity name.
		/// </param>
		/// 
		/// <param name="entityId">
		/// Entity ID.
		/// </param>
		public EntityNotFoundException(string entityName, long entityId)
			: base(string.Format("Cannot find the {0} entity with Id = {1}", entityName, entityId))
		{
		}
	}
}
