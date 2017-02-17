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
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	using System.Data.SqlTypes;
	using System.Diagnostics;

	using VahapYigit.Test.Core;

	#region [ Delegates ]

	/// <summary>
	/// Handler on Crud operations on error.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">CrudOperationErrorEventArgs.</param>
	public delegate void CrudOperationErrorHandler(object sender, CrudOperationErrorEventArgs e);

	#endregion

	/// <summary>
	/// Crud is the base class for all Crud classes.
	/// This class contains data access methods.
	/// </summary>
	public abstract class Crud : IDisposable
	{
		#region [ Members ]

		private string _connectionString = null;
		protected readonly IUserContext _userContext = null;

		/// <summary>
		/// Event raised on Crud operation errors.
		/// </summary>
		public event CrudOperationErrorHandler OperationError = null;

		#endregion

		#region [ Constants ]

		protected static readonly string DefaultConnectionStringKey = "Default";

		#endregion

		#region [ Constructors & Initialization ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected Crud()
		{
			_userContext = ClientContext.Anonymous;

			this.OperationError += new CrudOperationErrorHandler(OnOperationError);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userContext">User context.</param>
		/// <param name="options">Crud options.</param>
		protected Crud(IUserContext userContext)
			: this()
		{
			if (userContext != null)
			{
				_userContext = userContext;
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~Crud()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Initializes the instance.
		/// </summary>
		public virtual void Initialize()
		{
			string connectionStringKey = this.GetConnectionStringKey();

			ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings[connectionStringKey];
			if (connection != null)
			{
				_connectionString = connection.ConnectionString;
			}

			if (string.IsNullOrEmpty(_connectionString))
			{
				ThrowException.ThrowConfigurationErrorsException(string.Format(
					"Cannot find the '{0}' connectionString using ConfigurationManager.ConnectionStrings[\"{0}\"]",
					connectionStringKey));
			}
		}

		#endregion

		#region [ Events ]

		/// <summary>
		/// Handler on Crud operations on error.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">CrudOperationErrorEventArgs.</param>
		private void OnOperationError(object sender, CrudOperationErrorEventArgs e)
		{
			var paramString = e.Parameters.InString();

			if (paramString.IsNullOrEmpty())
			{
				paramString = "none";
			}

			string line = string.Format("{0}({1}), {2} procedure has raised an exception: {3} / parameters : {4}",
				sender.GetType().Name,
				this.GetExceptionMethodName(),
				e.Procedure,
				e.Exception.GetFullMessage(),
				paramString);

			LoggerServiceHelper.Current.WriteLine(this, LogStatusEnum.Error, line);
		}

		#endregion

		#region [ Execute methods ]

		/// <summary>
		/// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
		/// </summary>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <returns>The number of rows affected.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		protected int ExecuteNonQuery(string procedure, IDictionary<string, object> parameters)
		{
			int nbRows = 0;

			SqlConnection dbConnection = null;
			SqlCommand dbCommand = null;

			try
			{
				dbConnection = this.GetSqlConnection(true);
				dbCommand = new SqlCommand();

				dbCommand.CommandType = CommandType.StoredProcedure;
				dbCommand.CommandText = procedure;

				dbCommand.Connection = dbConnection;
				this.AddParameters(dbCommand, parameters);

				using (var et = new ExecutionTracerService(procedure))
				{
					nbRows = dbCommand.ExecuteNonQuery();
				}
			}
			catch (Exception x)
			{
				if (this.OperationError != null)
				{
					this.OperationError(this, new CrudOperationErrorEventArgs(x, procedure, parameters));
				}

				throw;
			}
			finally
			{
				dbCommand.SafeDispose();
				dbConnection.SafeDispose();
			}

			return nbRows;
		}

		/// <summary>
		/// Executes the query and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <typeparam name="T">Type of the returned object (simple types as such int, bool, string...).</typeparam>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <returns>The first column of the first row in the result set, or a null reference if the result set is empty. Returns a maximum of 2033 characters.</returns>
		protected T ExecuteScalar<T>(string procedure, IDictionary<string, object> parameters)
		{
			object scalar = null;

			using (var et = new ExecutionTracerService(procedure))
			using (var dtResult = this.ToDataTable(procedure, "Result", parameters))
			{
				if (dtResult.Rows.Count != 0)
				{
					scalar = dtResult.Rows[0][0];
				}
			}

			return (T)scalar;
		}

		/// <summary>
		/// Executes the query and returns a DataSet.
		/// </summary>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <param name="returnValue">Returned value by the stored procedure.</param>
		/// <param name="tableNames">Names of the inner DataTables</param>
		/// <returns>The DataSet containing the different DataTables.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		protected DataSet ToDataSet(string procedure, IDictionary<string, object> parameters, out int returnValue, params string[] tableNames)
		{
			ConditionChecker.Required(
				!tableNames.IsNullOrEmpty(),
				new ArgumentNullException("tableNames"));

			DataSet dsResults = new DataSet() { RemotingFormat = SerializationFormat.Binary };

			SqlConnection dbConnection = null;
			SqlCommand dbCommand = null;

			try
			{
				dbConnection = this.GetSqlConnection(true);
				dbCommand = new SqlCommand();

				dbCommand.CommandType = CommandType.StoredProcedure;
				dbCommand.CommandText = procedure;

				dbCommand.Connection = dbConnection;

				this.AddParameters(dbCommand, parameters);
				dbCommand.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

				using (var et = new ExecutionTracerService(procedure))
				using (var dbAdapter = new SqlDataAdapter(dbCommand))
				{
					const string tName = "Table";
					for (int i = 0; i < tableNames.Length; i++)
					{
						dbAdapter.TableMappings.Add((i == 0) ? tName : tName + i, tableNames[i]);
					}

					dbAdapter.Fill(dsResults);
					returnValue = TypeHelper.To<int>(dbCommand.Parameters["@ReturnValue"].Value);
				}
			}
			catch (Exception x)
			{
				if (this.OperationError != null)
				{
					this.OperationError(this, new CrudOperationErrorEventArgs(x, procedure, parameters));
				}

				throw;
			}
			finally
			{
				dbCommand.SafeDispose();
				dbConnection.SafeDispose();
			}

			return dsResults;
		}

		/// <summary>
		/// Executes the query and returns a DataSet.
		/// </summary>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <param name="tableNames">Names of the inner DataTables</param>
		/// <returns>The DataSet containing the different DataTables.</returns>
		protected DataSet ToDataSet(string procedure, IDictionary<string, object> parameters, params string[] tableNames)
		{
			int returnValue;
			return this.ToDataSet(procedure, parameters, out returnValue, tableNames);
		}

		/// <summary>
		/// Executes the query and returns a DataTable.
		/// </summary>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="tableName">Name of the DataTable</param>
		/// <param name="returnValue">Returned value by the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <returns>The DataTable.</returns>
		protected DataTable ToDataTable(string procedure, string tableName, out int returnValue, IDictionary<string, object> parameters)
		{
			return this.ToDataSet(procedure, parameters, out returnValue, tableName).Tables[tableName];
		}

		/// <summary>
		/// Executes the query and returns a DataTable.
		/// </summary>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="tableName">Name of the DataTable</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <returns>The DataTable.</returns>
		protected DataTable ToDataTable(string procedure, string tableName, IDictionary<string, object> parameters)
		{
			return this.ToDataSet(procedure, parameters, tableName).Tables[tableName];
		}

		/// <summary>
		/// Executes the query and returns a IDataReader.
		/// </summary>
		/// <param name="procedure">Name of the stored procedure.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		/// <param name="connection">The created connection. The connection must be closed once the IDataReader has been read.</param>
		/// <returns>The IDataReader object containing records.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		protected IDataReader ToDataReader(string procedure, IDictionary<string, object> parameters, out DbConnection connection)
		{
			IDataReader dbReader = null;
			SqlCommand dbCommand = null;

			try
			{
				connection = this.GetSqlConnection(true);
				dbCommand = new SqlCommand();

				dbCommand.CommandType = CommandType.StoredProcedure;
				dbCommand.CommandText = procedure;

				dbCommand.Connection = (SqlConnection)connection;

				this.AddParameters(dbCommand, parameters);

				using (var et = new ExecutionTracerService(procedure))
				{
					dbReader = dbCommand.ExecuteReader();
				}
			}
			catch (Exception x)
			{
				if (this.OperationError != null)
				{
					this.OperationError(this, new CrudOperationErrorEventArgs(x, procedure, parameters));
				}

				throw;
			}
			finally
			{
				dbCommand.SafeDispose();
			}

			return dbReader;
		}

		#endregion

		#region [ Protected methods ]

		/// <summary>
		/// Overrides this method to define another ConnectionString key.
		/// </summary>
		/// <returns>The ConnectionString key.</returns>
		protected virtual string GetConnectionStringKey()
		{
			return DefaultConnectionStringKey;
		}

		/// <summary>
		/// Gets the method name that raises the exception using the StackTrace.
		/// </summary>
		/// <returns>The method name that raised the exception.</returns>
		private string GetExceptionMethodName()
		{
			StackFrame stack = new StackFrame(4, false);
			return stack.GetMethod().Name;
		}

		#endregion

		#region [ Private methods ]

		/// <summary>
		/// Returns a SqlConnection object.
		/// </summary>
		/// <param name="opening">Indicates whether the connection is opened.</param>
		/// <returns>A SqlConnection object.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		private SqlConnection GetSqlConnection(bool opening = false)
		{
			SqlConnection dbConnection = new SqlConnection(_connectionString);

			if (opening)
			{
				dbConnection.Open();
			}

			return dbConnection;
		}

		/// <summary>
		/// Converts an input object for DB usage ({null, Guid.Empty} -> DBNull.Value).
		/// </summary>
		/// <param name="obj">Object to convert.</param>
		/// <returns>The converted object.</returns>
		private static object ToDbParameter(object parameter)
		{
			if (parameter != null)
			{
				if (parameter is Guid && (Guid)parameter == Guid.Empty)
				{
					return DBNull.Value;
				}

				return parameter;
			}

			return DBNull.Value;
		}

		/// <summary>
		/// Adds the parameters and the user context to the SqlCommand.
		/// </summary>
		/// <param name="dbCommand">SqlCommand object.</param>
		/// <param name="parameters">Input parameters for the stored procedure call.</param>
		private void AddParameters(SqlCommand dbCommand, IDictionary<string, object> parameters)
		{
			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> iterator in parameters)
				{
					if (iterator.Value == null)
					{
						dbCommand.Parameters.AddWithValue(iterator.Key, DBNull.Value);
					}
					else if (iterator.Value is DateTime && (DateTime)iterator.Value == default(DateTime))
					{
						dbCommand.Parameters.AddWithValue(iterator.Key, SqlDateTime.MinValue.Value);
					}
					else if (iterator.Value is byte[])
					{
						System.Diagnostics.Debugger.Break(); // TODO : not tested!
						dbCommand.Parameters.Add(new SqlParameter(iterator.Key, SqlDbType.Image)); // TODO : to test
						dbCommand.Parameters[iterator.Key].Value = iterator.Value;
					}
					else if (iterator.Value is IList<int>)
					{
						System.Diagnostics.Debugger.Break(); // TODO : not tested!
						this.AddParameterList<int>(dbCommand, iterator.Key, (IList<int>)iterator.Value); // TODO : to test
					}
					else if (iterator.Value is IList<string>)
					{
						System.Diagnostics.Debugger.Break(); // TODO : not tested!
						this.AddParameterList<string>(dbCommand, iterator.Key, (IList<string>)iterator.Value); // TODO : to test
					}
					else
					{
						dbCommand.Parameters.AddWithValue(iterator.Key, ToDbParameter(iterator.Value));
					}
				}
			}

			dbCommand.Parameters.AddWithValue("@CtxUser", _userContext.Id);
			dbCommand.Parameters.AddWithValue("@CtxCulture", _userContext.Culture);
			dbCommand.Parameters.AddWithValue("@CtxWithContextSecurity", _userContext.Options.WithContextSecurity);
		}

		/// <summary>
		/// Adds a TVP parameter to the SqlCommand.
		/// T must be handled by a user-defined table type on Sql Server side.
		/// </summary>
		/// <typeparam name="T">Object type.</typeparam>
		/// <param name="dbCommand">SqlCommand object.</param>
		/// <param name="parameterName">Parameter name of the stored procedure (ex, @IdList).</param>
		/// <param name="source">Source collection.</param>
		private void AddParameterList<T>(SqlCommand dbCommand, string parameterName, IEnumerable<T> source)
		{
			// SQL Usage:
			// CREATE PROCEDURE [dbo].[MyCustomStoredProc]
			// (
			//   @IdList AS TvpIntCollection READONLY
			// )
			// AS
			// BEGIN
			//   SELECT * FROM X WHERE X.Id IN (SELECT Value FROM @IdList)
			// END

			DataTable dtParam = new DataTable();
			dtParam.Columns.Add("Value", typeof(T));

			if (source != null)
			{
				foreach (var value in source)
				{
					dtParam.Rows.Add(value);
				}
			}

			dbCommand.Parameters.AddWithValue(parameterName, dtParam).SqlDbType = SqlDbType.Structured;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the user context.
		/// </summary>
		protected IUserContext UserContext
		{
			get { return _userContext; }
		}

		#endregion

		#region [ IDisposable Implementation ]

		private bool _isDisposed = false;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">For internal use.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}

		#endregion
	}
}
