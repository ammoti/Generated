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
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Text.RegularExpressions;

	[DoNotPrune]
	[DoNotObfuscate]
	public static class DatabaseHelper
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No security vulnerability in this context")]
		public static IList<CodeRefInfo> GetCodeRefs(SmoContext smoContext, string tableName)
		{
			IList<CodeRefInfo> codeRefs = new List<CodeRefInfo>();
			SqlConnection dbConnection = null;

			string idColumnName = string.Format("{0}_Id", tableName);
			string valueColumnName = string.Format("{0}_CodeRef", tableName);

			try
			{
				dbConnection = smoContext.GetSqlConnection(true);

				using (var dbCommand = new SqlCommand())
				{
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = string.Format("SELECT * FROM [{0}] ORDER BY {1};", tableName, idColumnName);
					dbCommand.Connection = dbConnection;

					using (var dbReader = dbCommand.ExecuteReader())
					{
						while (dbReader.Read())
						{
							var codeRef = new CodeRefInfo
							{
								Id = TypeHelper.To<long>(dbReader, idColumnName),
								Value = TypeHelper.To<string>(dbReader, valueColumnName)
							};

							codeRefs.Add(codeRef);
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dbConnection != null)
				{
					if (dbConnection.State == ConnectionState.Open)
					{
						dbConnection.Close();
					}
					else
					{
						dbConnection.Dispose();
					}
				}
			}

			return codeRefs;
		}

		[DoNotPrune]
		[DoNotObfuscate]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No security vulnerability in this context")]
		public static IList<string> GetTranslationKeyList(SmoContext smoContext)
		{
			IList<string> translationIds = new List<string>();
			SqlConnection dbConnection = null;

			try
			{
				dbConnection = smoContext.GetSqlConnection(true);

				using (var dbCommand = new SqlCommand())
				{
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "SELECT [Translation].[Translation_Key] FROM [Translation] ORDER BY [Translation].[Translation_Key];";
					dbCommand.Connection = dbConnection;

					using (var dbReader = dbCommand.ExecuteReader())
					{
						while (dbReader.Read())
						{
							translationIds.Add(dbReader["Translation_Key"].ToString());
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dbConnection != null)
				{
					if (dbConnection.State == ConnectionState.Open)
					{
						dbConnection.Close();
					}
					else
					{
						dbConnection.Dispose();
					}
				}
			}

			return translationIds;
		}

		[DoNotPrune]
		[DoNotObfuscate]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No security vulnerability in this context")]
		public static IList<Translation> GetTranslations(SmoContext smoContext)
		{
			IList<Translation> translations = new List<Translation>();
			SqlConnection dbConnection = null;

			try
			{
				dbConnection = smoContext.GetSqlConnection(true);

				using (var dbCommand = new SqlCommand())
				{
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "SELECT * FROM [Translation] ORDER BY [Translation].[Translation_Key];";
					dbCommand.Connection = dbConnection;

					using (var dbReader = dbCommand.ExecuteReader())
					{
						while (dbReader.Read())
						{
							for (int i = 0; i < dbReader.FieldCount; i++)
							{
								string columnName = dbReader.GetName(i);
								if (columnName == "Translation_Id" || columnName == "Translation_Key")
								{
									continue;
								}

								Translation translation = new Translation();
								string currentCulture = dbReader.GetName(i).Replace("Translation_Value_", "");

								translation.Id = string.Format("{0}_{1}", dbReader["Translation_Key"].ToString(), currentCulture);
								translation.Text = dbReader[dbReader.GetName(i)].ToString();

								translations.Add(translation);
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dbConnection != null)
				{
					if (dbConnection.State == ConnectionState.Open)
					{
						dbConnection.Close();
					}
					else
					{
						dbConnection.Dispose();
					}
				}
			}

			return translations;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No security vulnerability in this context")]
		public static string[] GetCultures(SmoContext smoContext)
		{
			IList<string> cultures = new List<string>();
			SqlConnection dbConnection = null;

			try
			{
				dbConnection = smoContext.GetSqlConnection(true);

				using (var dbCommand = new SqlCommand())
				{
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "SELECT TOP 1 * FROM [Translation];";
					dbCommand.Connection = dbConnection;

					using (var dbReader = dbCommand.ExecuteReader())
					{
						for (int i = 0; i < dbReader.FieldCount; i++)
						{
							string columnName = dbReader.GetName(i);
							if (columnName == "Translation_Id" || columnName == "Translation_Key")
							{
								continue;
							}

							string currentCulture = columnName.Replace("Translation_Value_", string.Empty);

							Regex rx = new Regex("^[a-zA-Z]{2}$");
							if (!rx.IsMatch(currentCulture))
							{
								ThrowException.Throw("The '{0}' column is not correct!", columnName);
							}

							cultures.Add(currentCulture);
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dbConnection != null)
				{
					if (dbConnection.State == ConnectionState.Open)
					{
						dbConnection.Close();
					}
					else
					{
						dbConnection.Dispose();
					}
				}
			}

			return cultures.ToArray();
		}
	}
}
