// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.Collections.Generic;

	public class ProcessorParameters
	{
		#region [ Members ]

		private string _compilationMode = "Debug";

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ProcessorParameters()
		{
			this.WithSqlProcedureIntegration = true;
			this.CompilationMode = "Debug";
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="withSqlProcedureIntegration">
		/// Value indicating whether the process executes the SQL Procedure Integration.
		/// </param>
		/// 
		/// <param name="compilationMode">
		/// Compilation mode (Debug, Release - default value, Debug).
		/// </param>
		/// 
		/// <param name="tableNames">
		/// Tables to process.
		/// </param>
		public ProcessorParameters(bool withSqlProcedureIntegration, string compilationMode, params string[] tableNames)
			: this()
		{
			this.WithSqlProcedureIntegration = withSqlProcedureIntegration;
			this.CompilationMode = compilationMode;
			this.TableNames = tableNames;
		}

		/// <summary>
		/// Value indicating whether the process executes the SQL Procedure Integration action.
		/// </summary>
		public bool WithSqlProcedureIntegration { get; set; }

		/// <summary>
		/// Compilation mode (Debug, Release - default value, Debug).
		/// </summary>
		public string CompilationMode
		{
			get { return _compilationMode; }
			set
			{
				if (value.ToUpperInvariant() == "RELEASE")
				{
					_compilationMode = "Release";
				}
				else
				{
					_compilationMode = "Debug";
				}
			}
		}

		/// <summary>
		/// Tables to process.
		/// </summary>
		public IEnumerable<string> TableNames { get; set; }
	}
}
