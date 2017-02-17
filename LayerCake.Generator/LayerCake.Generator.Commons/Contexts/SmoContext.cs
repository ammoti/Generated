// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Diagnostics;

	using Microsoft.SqlServer.Management.Common;
	using Microsoft.SqlServer.Management.Smo;

	public class SmoContext
	{
		#region [ Members ]

		private bool _isLoaded = false;

		#endregion

		#region [ Constructor ]

		public SmoContext(string databaseHost, string databaseName, string username = null, string password = null)
		{
			this.DatabaseHost = databaseHost;
			this.DatabaseName = databaseName;

			this.Username = username;
			this.Password = password;

			if (this.Username != null && this.Password != null)
			{
				this.ConnectionString = string.Format(@"Data Source={0};Initial Catalog={1};User Id={2};Password={3};",
					this.DatabaseHost, this.DatabaseName, this.Username, this.Password);
			}
			else
			{
				this.ConnectionString = string.Format(@"Data Source={0};Initial Catalog={1};User ID=sa;Password=Objectx04081991;",
					this.DatabaseHost, this.DatabaseName);
			}
		}

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Loads the Smo Context.
		/// </summary>
		public void LoadContext()
		{
			if (!_isLoaded)
			{
				var database = this.GetSmoDatabase();
				if (database == null)
				{
					ThrowException.Throw(
						"Cannot access to database '{0}' on server '{1}' (database not found)",
						this.DatabaseName,
						this.DatabaseHost);
				}

				this.Tables = new List<Table>();
				this.TableNames = new List<string>();

				foreach (Table table in database.Tables)
				{
					if (table.IsSystemObject)
					{
						continue;
					}

					this.Tables.Add(table);
					this.TableNames.Add(table.Name);
				}

				_isLoaded = true;
			}
		}

		/// <summary>
		/// Gets a connection objet to the SQL Server.
		/// </summary>
		/// <param name="opened">Value indicating whether the connection is automatically opened.</param>
		/// <returns>The SqlConnection object.</returns>
		public SqlConnection GetSqlConnection(bool opened)
		{
			SqlConnection dbConnection = new SqlConnection(this.ConnectionString);

			if (opened)
			{
				dbConnection.Open();
			}

			return dbConnection;
		}

		/// <summary>
		/// Gets the Smo Server object.
		/// </summary>
		/// <returns>The Server object.</returns>
		public Server GetSmoConnection()
		{
			ServerConnection serverConnection = new ServerConnection(this.GetSqlConnection(false));
			Server server = new Server(serverConnection);

			if (this.SqlServerFullVersion == null)
				this.SqlServerFullVersion = this.GetSqlConnection(true).ServerVersion;

			if (this.SmoVersion == null)
				this.SmoVersion = FileVersionInfo.GetVersionInfo(server.GetType().Assembly.Location).FileMajorPart.ToString();

			return server;
		}

		/// <summary>
		/// Gets the Smo Database object.
		/// </summary>
		/// <returns>The Database object.</returns>
		public Database GetSmoDatabase()
		{
			Server server = this.GetSmoConnection();
			Database database = server.Databases[this.DatabaseName];

			return database;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Connection string.
		/// </summary>
		public string ConnectionString
		{
			get;
			private set;
		}

		/// <summary>
		/// Hostname of the database.
		/// </summary>
		public string DatabaseHost
		{
			get;
			private set;
		}

		/// <summary>
		/// Database name.
		/// </summary>
		public string DatabaseName
		{
			get;
			private set;
		}

		/// <summary>
		/// Username to connect to the database.
		/// </summary>
		public string Username
		{
			get;
			private set;
		}

		/// <summary>
		/// Password to connect to the database.
		/// </summary>
		public string Password
		{
			get;
			private set;
		}

		/// <summary>
		/// SMO tables of the database.
		/// </summary>
		public IList<Table> Tables
		{
			get;
			private set;
		}

		/// <summary>
		/// Tables of the database.
		/// </summary>
		public IList<string> TableNames
		{
			get;
			private set;
		}

		public string SmoVersion
		{
			get;
			private set;
		}

		/// <summary>
		/// SQL Server version.
		/// </summary>
		public string SqlServerFullVersion
		{
			get;
			private set;
		}

		public string SqlServerMajorVersion
		{
			get
			{
				if (this.SqlServerFullVersion != null)
				{
					int p = this.SqlServerFullVersion.IndexOf('.');
					if (p != -1)
					{
						return this.SqlServerFullVersion.Substring(0, p);
					}
				}

				return "N.C.";
			}
		}

		public string SqlServerYearVersion
		{
			get
			{
				if (!string.IsNullOrEmpty(this.SqlServerFullVersion))
				{
					if (this.SqlServerFullVersion.StartsWith("12.")) return "2014";
					if (this.SqlServerFullVersion.StartsWith("11.")) return "2012";
					if (this.SqlServerFullVersion.StartsWith("10.")) return "2008";
				}

				return "N.C.";
			}
		}

		#endregion
	}
}
