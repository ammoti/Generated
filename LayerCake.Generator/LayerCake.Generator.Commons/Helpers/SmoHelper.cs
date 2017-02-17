// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;
	using System.IO;

	using Microsoft.SqlServer.Management.Common;
	using Microsoft.SqlServer.Management.Smo;

	using LayerCake.Generator.Commons;

	static class SmoHelper
	{
		/// <summary>
		/// Executes a SQL script using SMO API.
		/// </summary>
		/// 
		/// <param name="scriptPath">
		/// Path of the script to execute.
		/// </param>
		/// 
		/// <param name="databaseHost">
		/// Name of the instance of the server with which the connection is established.
		/// </param>
		/// 
		/// <param name="databaseHost">
		/// Name of the instance of the server with which the connection is established.
		/// </param>
		/// 
		/// <param name="username">
		/// Username.
		/// </param>
		/// 
		/// <param name="password">
		/// Password.
		/// </param>
		public static void RunSqlScript(string scriptPath, string databaseHost, string databaseName, string username = null, string password = null)
		{
			try
			{
				string scriptContent = File.ReadAllText(scriptPath);

				ServerConnection serverConnection = null;

				if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
				{
					serverConnection = new ServerConnection(databaseHost, username, password);
				}
				else
				{
					serverConnection = new ServerConnection(databaseHost);
				}

				var server = new Server(serverConnection);
				var database = server.Databases[databaseName];

				// Exception
				// SMO Mixed mode assembly is built against version 'v2.0.50727'  of the runtime
				// and cannot be loaded in the 4.0 runtime without additional configuration information.
				database.ExecuteNonQuery(scriptContent);
			}
			catch (Exception x)
			{
				string error = string.Format("Error on {0} -> {1}", scriptPath, x.GetFullMessage(false));
				throw new ExecutionFailureException(error);
			}
		}

		/// <summary>
		/// Executes a SQL script using SMO API.
		/// </summary>
		/// 
		/// <param name="scriptPath">
		/// Path of the script to execute.
		/// </param>
		/// 
		/// <param name="smoContext">
		/// SmoContext instance.
		/// </param>
		public static void RunSqlScript(string scriptPath, SmoContext smoContext)
		{
			SmoHelper.RunSqlScript(scriptPath, smoContext.DatabaseHost, smoContext.DatabaseName, smoContext.Username, smoContext.Password);
		}

		#region [ Column Helpers ]

		/// <summary>
		/// Gets information from a foreign key column.
		/// </summary>
		/// 
		/// <param name="column">
		/// Column.
		/// </param>
		/// 
		/// <returns>
		/// Information about the foreign key.
		/// </returns>
		public static SmoForeignKeyColumnInfo GetForeignKeyColumnInfo(Column column)
		{
			if (column.IsForeignKey)
			{
				Table table = (Table)column.Parent;

				foreach (ForeignKey key in table.ForeignKeys)
				{
					foreach (ForeignKeyColumn fkColumn in key.Columns)
					{
						if (fkColumn.Name == column.Name)
						{
							Database database = table.Parent;

							return new SmoForeignKeyColumnInfo
							{
								ReferencedTable = database.Tables[key.ReferencedTable],
								ReferencedColumn = database.Tables[key.ReferencedTable].Columns[key.Columns[0].ReferencedColumn]
							};
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Indicates whether a column has a default value.
		/// </summary>
		/// 
		/// <param name="column">
		/// Column.
		/// </param>
		/// 
		/// <param name="defaultValue">
		/// Default value (string representation).
		/// </param>
		/// 
		/// <returns>
		/// True if the column has a default value; otherwise, false.
		/// </returns>
		public static bool ParseColumnDefaultValue(Column column, out string defaultValue)
		{
			defaultValue = null;

			if (column.DefaultConstraint == null)
			{
				return false;
			}

			defaultValue = column.DefaultConstraint.Text;

			if (string.IsNullOrEmpty(defaultValue))
			{
				return false;
			}

			if (defaultValue.StartsWith("('") && defaultValue.EndsWith("')"))
			{
				defaultValue = defaultValue.Substring(2);
				defaultValue = defaultValue.Remove(defaultValue.Length - 2, 2);
			}
			else if (defaultValue.StartsWith("((") && defaultValue.EndsWith("))"))
			{
				defaultValue = defaultValue.Substring(2);
				defaultValue = defaultValue.Remove(defaultValue.Length - 2, 2);
			}

			switch (column.DataType.SqlDataType)
			{
				case SqlDataType.VarChar:
				case SqlDataType.VarCharMax:
				case SqlDataType.NVarChar:
				case SqlDataType.NVarCharMax:
				case SqlDataType.Char:
				case SqlDataType.NChar:
				case SqlDataType.Text:
				case SqlDataType.NText:
					defaultValue = string.Format("\"{0}\"", defaultValue);
					break;
				case SqlDataType.Bit:
					defaultValue = (defaultValue.ToUpperInvariant() == "TRUE") ? "true" : "false";
					break;
				case SqlDataType.Float:
				case SqlDataType.Decimal:
				case SqlDataType.Numeric:
				case SqlDataType.Money:
				case SqlDataType.SmallMoney:
					defaultValue = string.Format("{0}M", defaultValue);
					break;
				case SqlDataType.DateTime:
				case SqlDataType.DateTime2:
				case SqlDataType.DateTimeOffset:
				case SqlDataType.SmallDateTime:
					defaultValue = "DateTime.Now";
					break;
				case SqlDataType.Date:
					defaultValue = "DateTime.Today";
					break;
			}

			return true;
		}

		public static string GetColumnDescription(Column column)
		{
			string description = null;

			if (column.ExtendedProperties["MS_Description"] != null &&
				column.ExtendedProperties["MS_Description"].Value != null)
			{
				description = column.ExtendedProperties["MS_Description"].Value.ToString();
			}

			return !string.IsNullOrEmpty(description) ? description : null;
		}

		#endregion
	}
}
