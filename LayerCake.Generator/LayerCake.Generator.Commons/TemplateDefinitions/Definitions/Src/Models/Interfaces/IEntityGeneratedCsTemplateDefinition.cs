// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Models
{
	using System.Collections.Generic;

	using LayerCake.Generator.Commons;

	public class IEntityGeneratedCsTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public IEntityGeneratedCsTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{3f8c3005-d7f5-4077-9b57-ce5704e48179}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Models\Interfaces\IEntity.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByTable; }
		}

		public string OutputFileNamePattern
		{
			get { return "I{0}.generated.cs"; }
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
