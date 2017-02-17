// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons.TemplateDefinitions.WebServices
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;

	using LayerCake.Generator.Commons;

	public class ServiceModelServicesGeneratedConfigTemplateDefinition : ITemplateDefinition
	{
		#region [ Constructor ]

		[DoNotPrune]
		public ServiceModelServicesGeneratedConfigTemplateDefinition()
		{
		}

		#endregion

		#region [ ITemplateDefinition Implementation ]

		public string Guid
		{
			get { return "{f36d627d-43e1-4b90-97f8-5b3d17de7d2b}"; }
		}

		public string TemplatePath
		{
			get { return @"Src\WebServices\Config\ServiceModel.Services.generated.config.tt"; }
		}

		public TemplateDefinitionType Type
		{
			get { return TemplateDefinitionType.Normal; }
		}

		public string OutputFileNamePattern
		{
			get { return "ServiceModel.Services.generated.config"; }
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
			parameters.Add("CustomServices", this.GetConfigServices(context));
		}

		public bool CanBeExecuted(ProcessorContext context, object parameter = null)
		{
			return true;
		}

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Gets the full path of the deployed ServiceModel.Services.generated.config file.
		/// </summary>
		/// 
		/// <param name="context">
		/// Processor context.
		/// </param>
		/// 
		/// <returns>
		/// The full path of the deployed ServiceModel.Services.generated.config file.
		/// </returns>
		private string GetDeployedFilePath(ProcessorContext context)
		{
			string name = string.Format("{0}.WebServices", context.ProjectName);
			string file = Path.Combine(context.SrcDirectory, name, "Config", this.OutputFileNamePattern);

			return Path.GetFullPath(file);
		}

		/// <summary>
		/// Gets the enpoint entries defined in the ServiceModel.Services.generated.config file.
		/// </summary>
		/// 
		/// <param name="context">
		/// Processor context.
		/// </param>
		/// 
		/// <returns>
		/// The enpoint entries defined in the ServiceModel.Services.generated.config file.
		/// </returns>
		private ConfigServiceItem[] GetConfigServices(ProcessorContext context)
		{
			var list = new List<ConfigServiceItem>();
			var file = this.GetDeployedFilePath(context).Replace(".generated", ".custom");

			XmlDocument xmlDoc;
			if (XmlHelper.ValidateXml(file, out xmlDoc))
			{
				try
				{
					XmlNode rootNode = xmlDoc.SelectSingleNode("//services");
					if (rootNode != null)
					{
						foreach (XmlNode node in rootNode.ChildNodes)
						{
							if (node.Name == "service")
							{
								list.Add(new ConfigServiceItem(node));
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