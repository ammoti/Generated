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

	public class LanguageCollectionTestsGeneratedCsTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public LanguageCollectionTestsGeneratedCsTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{09c1bdb3-5621-43f6-b758-f6d15c2fa2be}"; }
		}

		public string TemplatePath
		{
			get { return @"Tests\Tests\Core\Collections\LanguageCollectionTests.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.Normal; }
		}

		public string OutputFileNamePattern
		{
			get { return "LanguageCollectionTests.generated.cs"; }
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
