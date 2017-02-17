// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.Tests
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;

	using LayerCake.Generator.Commons;

	public class ServiceModelClientGeneratedConfigTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public ServiceModelClientGeneratedConfigTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{3d493503-2a60-474a-8d8e-d579cc383280}"; }
		}

		public string TemplatePath
		{
			get { return @"Tests\Tests\Config\ServiceModel.Client.generated.config.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.Normal; }
		}

		public string OutputFileNamePattern
		{
			get { return "ServiceModel.Client.generated.config"; }
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
			parameters.Add("CustomEndpoints", this.GetConfigEndpoints(context));
		}

		public bool CanBeExecuted(ProcessorContext context, object parameter = null)
		{
			return true;
		}

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Gets the full path of the deployed ServiceModel.Client.generated.config file.
		/// </summary>
		/// 
		/// <param name="context">
		/// Processor context.
		/// </param>
		/// 
		/// <returns>
		/// The full path of the deployed ServiceModel.Client.generated.config file.
		/// </returns>
		private string GetDeployedFilePath(ProcessorContext context)
		{
			string name = string.Format("{0}.Tests", context.ProjectName);
			string file = Path.Combine(context.SolutionDirectory, "Tests", name, "Config", this.OutputFileNamePattern);

			return Path.GetFullPath(file);
		}

		/// <summary>
		/// Gets the enpoint entries defined in the ServiceModel.Client.custom.config file.
		/// </summary>
		/// 
		/// <param name="context">
		/// Processor context.
		/// </param>
		/// 
		/// <returns>
		/// The enpoint entries defined in the ServiceModel.Client.custom.config file.
		/// </returns>
		private ConfigEndpointItem[] GetConfigEndpoints(ProcessorContext context)
		{
			var list = new List<ConfigEndpointItem>();
			var file = this.GetDeployedFilePath(context).Replace(".generated", ".custom");

			XmlDocument xmlDoc;
			if (XmlHelper.ValidateXml(file, out xmlDoc))
			{
				try
				{
					XmlNode rootNode = xmlDoc.SelectSingleNode("//client");
					if (rootNode != null)
					{
						foreach (XmlNode node in rootNode.ChildNodes)
						{
							if (node.Name == "endpoint")
							{
								list.Add(new ConfigEndpointItem(node));
							}
						}
					}
				}
				catch
				{
					// Nothing.
				}
			}

			return list.ToArray();
		}

		#endregion
	}
}
