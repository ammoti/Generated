// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Contracts
{
	using System.Collections.Generic;
	using System.Linq;

	using LayerCake.Generator.Commons;

	public class ContractGeneratedCsBusinessTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public ContractGeneratedCsBusinessTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{262f9d5d-4a02-4689-ab73-72c4c361f2f4}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Contracts\Contracts\IContract.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByBusiness; }
		}

		public string OutputFileNamePattern
		{
			get { return "I{0}Contract.generated.cs"; }
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
