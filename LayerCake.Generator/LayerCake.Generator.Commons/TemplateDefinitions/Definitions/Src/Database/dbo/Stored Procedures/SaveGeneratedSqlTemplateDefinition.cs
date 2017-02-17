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

	public class SaveGeneratedSqlTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public SaveGeneratedSqlTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{c731b690-daf0-4ef6-b34d-5548c2e6e282}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Database\dbo\Stored Procedures\Save.generated.sql.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByTable; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}_Save.generated.sql"; }
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

		public bool CanBeExecuted(ProcessorContext context, object parameter = null)
		{
			return true;
		}

		public void AddParameters(ProcessorContext context, IDictionary<string, object> parameters)
		{
		}

		#endregion
	}
}
