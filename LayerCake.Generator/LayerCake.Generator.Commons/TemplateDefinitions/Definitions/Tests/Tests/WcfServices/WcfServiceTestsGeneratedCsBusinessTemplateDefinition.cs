// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Tests
{
	using System.Collections.Generic;
	using System.Linq;

	using LayerCake.Generator.Commons;

	public class WcfServiceTestsGeneratedCsBusinessTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public WcfServiceTestsGeneratedCsBusinessTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{045be133-3970-450e-9d3d-76b9d2ecd09c}"; }
		}

		public string TemplatePath
		{
			get { return @"Tests\Tests\WcfServices\WcfServiceTests.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByBusiness; }
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
			ModelDescriptor modelDescriptor = ((object[])parameter)[0] as ModelDescriptor;
			string name = ((object[])parameter)[1] as string;

			if (modelDescriptor.Schema.Tables.Any(i => i.Name == name))
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
