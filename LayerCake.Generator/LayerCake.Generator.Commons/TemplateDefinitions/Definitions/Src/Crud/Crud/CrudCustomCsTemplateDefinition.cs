// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Crud
{
	using System.Collections.Generic;

	using LayerCake.Generator.Commons;

	public class CrudCustomCsTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public CrudCustomCsTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{000babbf-f5ef-4273-80f4-ba4097fe76c1}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Crud\Crud\Crud.custom.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByTable; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}Crud.custom.cs"; }
		}

		public bool OverrideFileIfExists
		{
			get { return false; }
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
