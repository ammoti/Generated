// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Crud
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;

	using VahapYigit.Test.Core;
	using VahapYigit.Test.Models;

	/// <summary>
	/// CrudBase is the base class for all Crud classes (based on entities).
	/// This class contains data access methods.
	/// </summary>
	public abstract partial class CrudBase<T> : Crud
		where T : EntityBase
	{
		#region [ Constructors ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected CrudBase()
			: base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userContext">User context.</param>
		/// <param name="options">Crud options.</param>
		protected CrudBase(IUserContext userContext)
			: base(userContext)
		{
		}

		#endregion

		#region [ Execute methods ]

		/// <summary>
		/// Executes the query and returns an entity observable collection.
		/// </summary>
		/// <typeparam name="T">Type of the returned entity (derived from EntityBase).</typeparam>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <param name="withDeepMapping">Value indicating whether possible linked entities are mapped.</param>
		/// <returns>A collection.</returns>
		protected TCollection<T> ToEntityCollection(string procedure, IDictionary<string, object> parameters, bool? withDeepMapping = true)
		{
			DbConnection dbConnection;
			TCollection<T> collection = null;

			using (var et = new ExecutionTracerService(procedure))
			using (var dbReader = this.ToDataReader(procedure, parameters, out dbConnection))
			{
				collection = (withDeepMapping == true) ? this.DeepMap(dbReader) : this.Map(dbReader);
			}

			if (dbConnection != null)
				dbConnection.Close();

			return collection;
		}

		#endregion

		#region [ Public methods ]

		/// <summary>
		/// Indicates whether the search returns at least 1 record.
		/// </summary>
		/// 
		/// <param name="options">
		/// Optional search options. If not defined, all records are returned.
		/// </param>
		/// 
		/// <returns>
		/// True if the search returns at least 1 record; otherwise, false.
		/// </returns>
		public bool HasResult(SearchOptions options = null)
		{
			return this.Count(options) != 0;
		}

		#endregion

		#region [ Abstract methods ]

		/// <summary>
		/// Refreshs the entity instance from the database.
		/// </summary>
		/// 
		/// <param name="entity">
		/// Entity to refresh (must be in database).
		/// </param>
		public abstract void Refresh(ref T entity);

		/// <summary>
		/// Gets an entity given its unique ID.
		/// </summary>
		/// 
		/// <param name="id">
		/// Unique ID.
		/// </param>
		/// 
		/// <returns>
		/// The entity.
		/// </returns>
		public abstract T GetById(long id);

		/// <summary>
		/// Searchs entities with search options.
		/// </summary>
		/// 
		/// <param name="options">
		/// Optional options, filters, orderby, paging, etc.
		/// </param>
		/// 
		/// <returns>
		/// A collection of entities.
		/// </returns>
		public abstract TCollection<T> Search(ref SearchOptions options);

		/// <summary>
		/// Gets the number of records that verify the search options.
		/// </summary>
		/// 
		/// <param name="options">
		/// Optional search options. If not defined, all records are returned.
		/// </param>
		/// 
		/// <returns>
		/// The number of records.
		/// </returns>
		public abstract int Count(SearchOptions options = null);

		/// <summary>
		/// Saves (or updates) the entity in the database.
		/// </summary>
		///
		/// <param name="entity">
		/// Entity to save or update.
		/// </param>
		/// 
		/// <param name="options">
		/// Optional options.
		/// </param>
		/// 
		/// <returns>
		/// The number of affected rows.
		/// </returns>
		public abstract int Save(ref T entity, SaveOptions options = null);

		/// <summary>
		/// Deletes the entity from the database.
		/// </summary>
		/// 
		/// <param name="user">
		/// Entity to delete.
		/// </param>
		/// 
		/// <returns>
		/// The number of affected rows.
		/// </returns>
		public abstract int Delete(T appSettings);

		/// <summary>
		/// Deletes the entity given its unique ID.
		/// </summary>
		/// 
		/// <param name="id">
		/// Unique ID.
		/// </param>
		/// 
		/// <returns>
		/// The number of affected rows.
		/// </returns>
		public abstract int Delete(long id);

		/// <summary>
		/// Converts an IDataReader to a TCollection WITHOUT associated children (linked entities).
		/// </summary>
		/// <param name="source">IDataReader source.</param>
		/// <returns>A collection.</returns>
		protected abstract TCollection<T> Map(IDataReader source);

		/// <summary>
		/// Converts an DataTable to a TCollection WITHOUT associated children (linked entities).
		/// </summary>
		/// <param name="source">DataTable source.</param>
		/// <returns>A collection.</returns>
		protected abstract TCollection<T> Map(DataTable source);

		/// <summary>
		/// Converts an IDataReader to a TCollection WITH possible associated children (linked entities).
		/// </summary>
		/// <param name="source">IDataReader source.</param>
		/// <returns>A collection.</returns>
		protected abstract TCollection<T> DeepMap(IDataReader source);

		/// <summary>
		/// Converts an DataTable to a TCollection WITH possible associated children (linked entities).
		/// </summary>
		/// <param name="source">DataTable source.</param>
		/// <returns>A collection.</returns>
		protected abstract TCollection<T> DeepMap(DataTable source);

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the value indicating whether the current language is returned (other language values will have 'null' value).
		/// </summary>
		protected bool WithCurrentLanguageOnly
		{
			get;
			set;
		}

		#endregion
	}
}
