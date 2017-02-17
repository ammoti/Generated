// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Data;

	public interface IDataRowMapping
	{
		/// <summary>
		/// Fill the entity properties using a DataRow object.
		/// </summary>
		/// 
		/// <param name="source">
		/// DataRow object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		/// 
		/// <param name="columnPrefix">
		/// Column prefix (optional).
		/// </param>
		void Map(DataRow source, IUserContext userContext = null, string columnPrefix = null);

		/// <summary>
		/// Fill the entity properties and all its dependencies using a DataRow object.
		/// </summary>
		/// 
		/// <param name="source">
		/// DataRow object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		void DeepMap(DataRow source, IUserContext userContext = null);
	}
}
