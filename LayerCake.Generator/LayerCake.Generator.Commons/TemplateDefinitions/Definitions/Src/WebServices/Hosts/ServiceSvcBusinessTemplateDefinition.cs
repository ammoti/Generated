// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.WebServices
{
	using System.Collections.Generic;
	using System.Linq;

	using LayerCake.Generator.Commons;

	public class ServiceSvcBusinessTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public ServiceSvcBusinessTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{d254537d-1523-420c-9584-40a58308c2eb}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\WebServices\Hosts\Service.svc.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.OneByBusiness; }
		}

		public string OutputFileNamePattern
		{
			get { return "{0}Service.svc"; }
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
