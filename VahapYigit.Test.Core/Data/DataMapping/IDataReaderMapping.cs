// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Data;

	public interface IDataReaderMapping
	{
		/// <summary>
		/// Fill the entity properties using a IDataReader object.
		/// </summary>
		/// 
		/// <param name="source">
		/// IDataReader object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		/// 
		/// <param name="columnPrefix">
		/// Column prefix (optional).
		/// </param>
		void Map(IDataReader source, IUserContext userContext = null, string columnPrefix = null);

		/// <summary>
		/// Fills the entity properties and all its dependencies using a IDataReader object.
		/// </summary>
		/// 
		/// <param name="source">
		/// IDataReader object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		void DeepMap(IDataReader source, IUserContext userContext = null);
	}
}
