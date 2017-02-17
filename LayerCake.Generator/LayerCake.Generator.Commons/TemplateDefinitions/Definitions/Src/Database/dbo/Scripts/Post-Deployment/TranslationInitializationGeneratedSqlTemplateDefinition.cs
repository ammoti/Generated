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

	public class TranslationInitializationGeneratedSqlTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public TranslationInitializationGeneratedSqlTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{128b86ea-9a54-4b4f-83da-3a4c27569743}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Database\dbo\Scripts\Post-Deployment\TranslationInitialization.generated.sql.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.Normal; }
		}

		public string OutputFileNamePattern
		{
			get { return "TranslationInitialization.generated.sql"; }
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
		}

		public bool CanBeExecuted(ProcessorContext context, object parameter = null)
		{
			return true;
		}

		#endregion
	}
}
