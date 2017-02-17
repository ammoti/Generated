// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Database
{
	using System.Collections.Generic;

	using LayerCake.Generator.Commons;

	public class DeleteGeneratedSqlTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public DeleteGeneratedSqlTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{10aca237-bfa0-4384-abbd-04696de91ede}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Database\dbo\Stored Procedures\Delete.generated.sql.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByTable; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}_Delete.generated.sql"; }
		}

		public bool OverrideFileIfExists
		{
			get { return true; }
		}

		public bool AddToProject
		{
			get { return true; }
		}

		public bool ExecuteSqlScript
		{
			get { return false; }
		}

		public void AddParameters(ProcessorContext context, IDictionary<string, object> parameters)
		{
		}

		public bool CanBeExecuted(ProcessorContext context, object parameter = null)
		{
			return true;
		}

		#endregion
	}
}
