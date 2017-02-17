// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Services
{
	using System.Collections.Generic;
	using System.Linq;

	using LayerCake.Generator.Commons;

	public class ServiceGeneratedCsBusinessTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public ServiceGeneratedCsBusinessTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{3fef1d59-71e8-4507-ad23-4999b3dda58e}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\Services\Services\Service.generated.cs.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByBusiness; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}Service.generated.cs"; }
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
