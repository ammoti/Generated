// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Data;

	public interface IDataRowToEntityMappingBehavior
	{
		/// <summary>
		/// Performs mapping from a DataRow to an Entity instance.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the entity.
		/// </typeparam>
		/// 
		/// <param name="row">
		/// Input DataRow object.
		/// </param>
		/// 
		/// <param name="entity">
		/// Entity instance.
		/// </param>
		void Map<T>(DataRow row, T entity);
	}
}
