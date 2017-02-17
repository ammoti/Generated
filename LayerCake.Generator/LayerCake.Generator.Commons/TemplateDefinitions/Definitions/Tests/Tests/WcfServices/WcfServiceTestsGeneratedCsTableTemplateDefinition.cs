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

	public class WcfServiceTestsGeneratedCsTableTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public WcfServiceTestsGeneratedCsTableTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{4973b409-f3d4-4e0a-8e91-0529e362c3a5}"; }
		}

		public string TemplatePath
		{
			get { return @"Tests\Tests\WcfServices\WcfServiceTests.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByTable; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}WcfServiceTests.generated.cs"; }
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
