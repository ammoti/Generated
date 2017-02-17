// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Data;

	using VahapYigit.Test.Core;

	public class DataRowToEntityMapping : IDataRowToEntityMappingBehavior
	{
		#region [ IDataRowToEntityMappingBehavior ]

		/// <summary>
		/// Performs mapping from a DataRow to an entity instance.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the entity.
		/// </typeparam>
		/// 
		/// <param name="source">
		/// Input DataRow object.
		/// </param>
		/// 
		/// <param name="entity">
		/// Entity instance.
		/// </param>
		public void Map<T>(DataRow source, T entity)
		{
			var e = entity as EntityBase;
			if (e == null)
			{
				ThrowException.ThrowArgumentException(
					"The 'entity' parameter must inherit the VahapYigit.Test.Models.EntityBase abstract class and must be instanciated");
			}

			e.Map(source);
		}

		#endregion
	}
}
