// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Tests
{
	using System.Collections.Generic;

	using LayerCake.Generator.Commons;

	public class ServiceTestsGeneratedCsTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public ServiceTestsGeneratedCsTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{62bcfd50-c412-4148-a67e-5225f30aa75c}"; }
		}

		public string TemplatePath
		{
			get { return @"Tests\Tests\Services\ServiceTests.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByTable; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}ServiceTests.generated.cs"; }
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
