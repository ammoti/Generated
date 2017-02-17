// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Database
{
	using System.Collections.Generic;
	using System.IO;

	using LayerCake.Generator.Commons;

	public class InsertAllStoredProceduresGeneratedSqlTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public InsertAllStoredProceduresGeneratedSqlTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{2ebe730d-77f4-4bad-9ba5-164588c37328}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Database\dbo\Scripts\InsertAllStoredProcedures.generated.sql.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.Normal; }
		}

		public string OutputFileNamePattern
		{
			get { return "InsertAllStoredProcedures.generated.sql"; }
		}

		public bool OverrideFileIfExists
		{
			get { return true; }
		}

		public bool AddToProject
		{
			get { return false; }
		}

		public bool ExecuteSqlScript
		{
			get { return true; }
		}

		public void AddParameters(ProcessorContext context, IDictionary<string, object> parameters)
		{
			string path = context.SrcDirectory;
			string project = string.Format("{0}.Database", context.ProjectName);

			path = Path.Combine(path, project, @"dbo\Stored Procedures");

			parameters.Add("ParamStoredProcedurePath", path);
		}

		public bool CanBeExecuted(ProcessorContext context, object parameter = null)
		{
			return true;
		}

		#endregion
	}
}
